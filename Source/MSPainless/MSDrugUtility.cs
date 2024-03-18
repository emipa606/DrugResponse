using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace MSPainless;

public class MSDrugUtility
{
    public static bool IsValidPawnMod(Pawn pawn, bool painmode)
    {
        if (pawn.IsPrisoner)
        {
            if (painmode)
            {
                if (!DRSettings.DoIfPrisoner)
                {
                    return false;
                }
            }
            else if (!DRSettings.DoDRIfPrisoner)
            {
                return false;
            }
        }
        else if (!pawn.IsColonist)
        {
            return false;
        }

        return (pawn.Spawned || pawn.IsCaravanMember()) && !pawn.Dead && !pawn.InMentalState;
    }

    public static bool IsViolation(Pawn p, ThingDef t)
    {
        if (!t.IsDrug)
        {
            return true;
        }

        if (p?.Faction != null && p.Faction == Faction.OfPlayer)
        {
            return p.IsTeetotaler() && t.IsNonMedicalDrug;
        }

        return t.IsNonMedicalDrug;
    }

    public static bool IsOKtoAdmin(Pawn pawn, HediffDef hdef, ThingDef def)
    {
        DrugPolicy drugPolicy;
        if (pawn == null)
        {
            drugPolicy = null;
        }
        else
        {
            var drugs = pawn.drugs;
            drugPolicy = drugs?.CurrentPolicy;
        }

        var policy = drugPolicy;
        if (policy != null)
        {
            if (!DPExists(policy, def))
            {
                Messages.Message("MSPainless.ErrDrugPolicy".Translate(pawn.LabelShort, def?.label), pawn,
                    MessageTypeDefOf.NeutralEvent, false);
                return false;
            }

            if (policy[def] != null)
            {
                var entry = policy[def];
                if (entry is { allowScheduled: true, daysFrequency: > 0f })
                {
                    return false;
                }
            }
        }

        if (!DRSettings.DoIfImmune && ImmuneNow(pawn, hdef))
        {
            return false;
        }

        if (hdef is { defName: "Anesthetic" })
        {
            HediffSet hediffSet;
            if (pawn == null)
            {
                hediffSet = null;
            }
            else
            {
                var health = pawn.health;
                hediffSet = health?.hediffSet;
            }

            var set = hediffSet;
            var Anesthetic = set?.GetFirstHediffOfDef(hdef);
            if (Anesthetic is { Severity: >= 0.8f })
            {
                return false;
            }
        }

        if (!def.IsIngestible)
        {
            return false;
        }

        var ODs = def.ingestible.outcomeDoers;
        if (ODs.Count <= 0)
        {
            return true;
        }

        var toohighsev = false;
        HediffSet hediffSet2;
        if (pawn == null)
        {
            hediffSet2 = null;
        }
        else
        {
            var health2 = pawn.health;
            hediffSet2 = health2?.hediffSet;
        }

        var hediffset = hediffSet2;
        if (hediffset == null)
        {
            return true;
        }

        foreach (var OD in ODs)
        {
            if (OD is not IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoerGiveHediff)
            {
                continue;
            }

            var ODhediffdef = ingestionOutcomeDoerGiveHediff.hediffDef;
            if (ODhediffdef == null)
            {
                continue;
            }

            var ODSev = ingestionOutcomeDoerGiveHediff.severity;
            if (!(ODSev > 0f))
            {
                continue;
            }

            var ODhediff = hediffset.GetFirstHediffOfDef(ODhediffdef);
            if (ODhediff != null && ODhediff.Severity / ODSev > 0.75f)
            {
                toohighsev = true;
            }
        }

        return !toohighsev;
    }

    private static bool DPExists(DrugPolicy policy, ThingDef drugDef)
    {
        if (policy.Count <= 0)
        {
            return false;
        }

        for (var i = 0; i < policy.Count; i++)
        {
            var drugPolicyEntry = policy[i];
            if (drugPolicyEntry?.drug == drugDef)
            {
                return true;
            }
        }

        return false;
    }

    public static Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
    {
        var innerContainer = pawn.inventory.innerContainer;
        foreach (var defThing in innerContainer)
        {
            if (defThing.def == drugDef && DrugValidator(pawn, defThing))
            {
                return defThing;
            }
        }

        if (!pawn.IsPrisoner)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef),
                PathEndMode.ClosestTouch, TraverseParms.For(pawn), 9999f, x => DrugValidator(pawn, x));
        }

        return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef),
            PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors), 9999f,
            x => DrugValidator(pawn, x));
    }

    private static bool DrugValidator(Pawn pawn, Thing drug)
    {
        if (!drug.def.IsDrug)
        {
            return false;
        }

        if (pawn.IsPrisoner || !drug.Spawned)
        {
            return true;
        }

        if (drug.IsForbidden(pawn))
        {
            return false;
        }

        return pawn.CanReserve(drug) && drug.IsSociallyProper(pawn);
    }

    private static bool ImmuneNow(Pawn pawn, HediffDef chkhdef)
    {
        if (pawn == null || chkhdef == null)
        {
            return false;
        }

        var health = pawn.health;
        var hediffSet = health?.hediffSet;
        var chk = hediffSet?.GetFirstHediffOfDef(chkhdef);
        return chk != null && chk.FullyImmune();
    }
}