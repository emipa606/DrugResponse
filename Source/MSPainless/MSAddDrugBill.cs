using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless;

public class MSAddDrugBill
{
    private static readonly List<ThingDef> tmpMedicineBestToWorst = [];

    public static bool GenDrugResponse(bool isDR, Pawn pawn, HediffDef hdef, ThingDef drugDef = null,
        BodyPartRecord part = null, List<string> Master = null, int num = 1)
    {
        var result = false;
        if (drugDef != null)
        {
            if (GenMSRecipe(pawn, hdef, drugDef, part))
            {
                result = true;
            }
        }
        else if (isDR)
        {
            var Hdefname = hdef?.defName;
            if (Hdefname == null)
            {
                return false;
            }

            var drugdefname = MSDRUtility.GetValueDRDrug(Hdefname, Master, num);
            if (drugdefname == null)
            {
                return false;
            }

            drugDef = DefDatabase<ThingDef>.GetNamed(drugdefname, false);
            if (drugDef != null && GenMSRecipe(pawn, hdef, drugDef, part))
            {
                result = true;
            }
        }

        return result;
    }

    private static bool GenMSRecipe(Pawn pawn, HediffDef hdef, ThingDef drugdef, BodyPartRecord part = null)
    {
        var result = false;
        if (!MSDrugUtility.IsOKtoAdmin(pawn, hdef, drugdef))
        {
            return false;
        }

        var recipeDef = DefDatabase<RecipeDef>.GetNamed($"Administer_{drugdef.defName}");
        if (recipeDef == null)
        {
            return false;
        }

        if (!IsViolation(pawn, recipeDef, out var reason))
        {
            if (!ChkDuplication(pawn, recipeDef) && GenAdminOption(pawn, recipeDef, part))
            {
                result = true;
            }
        }
        else
        {
            Messages.Message(
                "MSPainless.ViolationMsg".Translate(pawn.LabelShort.CapitalizeFirst(),
                    drugdef.label.CapitalizeFirst(), reason), pawn, MessageTypeDefOf.NeutralEvent, false);
        }

        return result;
    }

    private static bool IsViolation(Pawn p, RecipeDef r, out string reason)
    {
        reason = "";
        if (p?.Faction != null && p.Faction == Faction.OfPlayer)
        {
            if (!p.IsTeetotaler() || !r.ingredients[0].filter.AllowedThingDefs.First().IsNonMedicalDrug)
            {
                return false;
            }

            reason = "MSPainless.ViolationTeetotaler".Translate();
            return true;
        }

        if (!r.ingredients[0].filter.AllowedThingDefs.First().IsNonMedicalDrug)
        {
            return false;
        }

        reason = "MSPainless.ViolationNonPlayer".Translate();
        return true;
    }

    private static bool ChkDuplication(Pawn pawn, RecipeDef recipedef)
    {
        bool b;
        if (pawn == null)
        {
            b = false;
        }
        else
        {
            var billStack = pawn.BillStack;
            b = billStack?.Bills != null;
        }

        if (!b || pawn.BillStack.Bills.Count <= 0)
        {
            return false;
        }

        foreach (var bill in pawn.BillStack.Bills)
        {
            if (bill?.recipe != null && bill.recipe == recipedef)
            {
                return true;
            }
        }

        return false;
    }

    private static bool GenAdminOption(Pawn patient, RecipeDef recipe, BodyPartRecord part = null)
    {
        if (patient == null)
        {
            return false;
        }

        var bill_Medical = new Bill_Medical(recipe, null);
        patient.BillStack.AddBill(bill_Medical);
        bill_Medical.Part = part;
        if (recipe.conceptLearned != null)
        {
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
        }

        var map = patient.Map;
        if (map == null)
        {
            return true;
        }

        if (!map.mapPawns.FreeColonists.Any(recipe.PawnSatisfiesSkillRequirements))
        {
            Bill.CreateNoPawnsWithSkillDialog(recipe);
        }

        if (!patient.InBed() && patient.RaceProps.IsFlesh)
        {
            if (patient.RaceProps.Humanlike)
            {
                if (!map.listerBuildings.allBuildingsColonist.Any(x =>
                        x is Building_Bed bed && RestUtility.CanUseBedEver(patient, x.def) && bed.Medical))
                {
                    Messages.Message("MessageNoMedicalBeds".Translate(), patient, MessageTypeDefOf.CautionInput,
                        false);
                }
            }
            else if (!map.listerBuildings.allBuildingsColonist.Any(x =>
                         x is Building_Bed && RestUtility.CanUseBedEver(patient, x.def)))
            {
                Messages.Message("MessageNoAnimalBeds".Translate(), patient, MessageTypeDefOf.CautionInput, false);
            }
        }

        if (patient.Faction != null && !patient.Faction.def.hidden &&
            !patient.Faction.HostileTo(Faction.OfPlayer) &&
            recipe.Worker.IsViolationOnPawn(patient, part, Faction.OfPlayer))
        {
            Messages.Message("MessageMedicalOperationWillAngerFaction".Translate(patient.Faction), patient,
                MessageTypeDefOf.CautionInput, false);
        }

        var minRequiredMedicine = GetMinRequiredMedicine(recipe);
        if (minRequiredMedicine != null && patient.playerSettings != null &&
            !patient.playerSettings.medCare.AllowsMedicine(minRequiredMedicine))
        {
            Messages.Message(
                "MessageTooLowMedCare".Translate(minRequiredMedicine.label, patient.LabelShort,
                    patient.playerSettings.medCare.GetLabel(), patient.Named("PAWN")), patient,
                MessageTypeDefOf.CautionInput, false);
        }

        return true;
    }

    private static ThingDef GetMinRequiredMedicine(RecipeDef recipe)
    {
        tmpMedicineBestToWorst.Clear();
        var allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
        foreach (var def in allDefsListForReading)
        {
            if (def.IsMedicine)
            {
                tmpMedicineBestToWorst.Add(def);
            }
        }

        tmpMedicineBestToWorst.SortByDescending(x => x.GetStatValueAbstract(StatDefOf.MedicalPotency));
        ThingDef thingDef = null;
        foreach (var ingredient in recipe.ingredients)
        {
            ThingDef thingDef2 = null;
            foreach (var medicine in tmpMedicineBestToWorst)
            {
                if (ingredient.filter.Allows(medicine))
                {
                    thingDef2 = medicine;
                }
            }

            if (thingDef2 != null && (thingDef == null || thingDef2.GetStatValueAbstract(StatDefOf.MedicalPotency) >
                    thingDef.GetStatValueAbstract(StatDefOf.MedicalPotency)))
            {
                thingDef = thingDef2;
            }
        }

        tmpMedicineBestToWorst.Clear();
        return thingDef;
    }
}