using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless
{
    // Token: 0x02000004 RID: 4
    public class MSAddDrugBill
    {
        // Token: 0x0400001F RID: 31
        private static readonly List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();

        // Token: 0x0600003F RID: 63 RVA: 0x00004A60 File Offset: 0x00002C60
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

        // Token: 0x06000040 RID: 64 RVA: 0x00004AC0 File Offset: 0x00002CC0
        private static bool GenMSRecipe(Pawn pawn, HediffDef hdef, ThingDef drugdef, BodyPartRecord part = null)
        {
            var result = false;
            if (!MSDrugUtility.IsOKtoAdmin(pawn, hdef, drugdef))
            {
                return false;
            }

            var recipeDef = DefDatabase<RecipeDef>.GetNamed("Administer_" + drugdef.defName);
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

        // Token: 0x06000041 RID: 65 RVA: 0x00004B60 File Offset: 0x00002D60
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

        // Token: 0x06000042 RID: 66 RVA: 0x00004C04 File Offset: 0x00002E04
        private static bool ChkDuplication(Pawn pawn, RecipeDef recipedef)
        {
            bool flag;
            if (pawn == null)
            {
                flag = false;
            }
            else
            {
                var billStack = pawn.BillStack;
                flag = billStack?.Bills != null;
            }

            if (!flag || pawn.BillStack.Bills.Count <= 0)
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

        // Token: 0x06000043 RID: 67 RVA: 0x00004CA0 File Offset: 0x00002EA0
        private static bool GenAdminOption(Pawn patient, RecipeDef recipe, BodyPartRecord part = null)
        {
            if (patient == null)
            {
                return false;
            }

            var bill_Medical = new Bill_Medical(recipe);
            patient.BillStack.AddBill(bill_Medical);
            bill_Medical.Part = part;
            if (recipe.conceptLearned != null)
            {
                PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
            }

            var patient2 = patient;
            var map = patient2.Map;
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

        // Token: 0x06000044 RID: 68 RVA: 0x00004F50 File Offset: 0x00003150
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
}