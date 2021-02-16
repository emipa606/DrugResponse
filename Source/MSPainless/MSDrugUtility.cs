using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace MSPainless
{
    // Token: 0x02000006 RID: 6
    public class MSDrugUtility
    {
        // Token: 0x0600004A RID: 74 RVA: 0x000050B8 File Offset: 0x000032B8
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

        // Token: 0x0600004B RID: 75 RVA: 0x00005114 File Offset: 0x00003314
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

        // Token: 0x0600004C RID: 76 RVA: 0x00005168 File Offset: 0x00003368
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
                    if (entry != null && entry.allowScheduled && entry.daysFrequency > 0f)
                    {
                        return false;
                    }
                }
            }

            if (!DRSettings.DoIfImmune && ImmuneNow(pawn, hdef))
            {
                return false;
            }

            if (hdef != null && hdef.defName == "Anesthetic")
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
                if (Anesthetic != null && Anesthetic.Severity >= 0.8f)
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
                if (!(OD is IngestionOutcomeDoer_GiveHediff))
                {
                    continue;
                }

                var ingestionOutcomeDoer_GiveHediff = OD as IngestionOutcomeDoer_GiveHediff;
                var ODhediffdef = ingestionOutcomeDoer_GiveHediff.hediffDef;
                if (ODhediffdef == null)
                {
                    continue;
                }

                var ODSev = (OD as IngestionOutcomeDoer_GiveHediff).severity;
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

            if (toohighsev)
            {
                return false;
            }

            return true;
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00005358 File Offset: 0x00003558
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

        // Token: 0x0600004E RID: 78 RVA: 0x00005398 File Offset: 0x00003598
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

        // Token: 0x0600004F RID: 79 RVA: 0x00005498 File Offset: 0x00003698
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

            if (!pawn.CanReserve(drug))
            {
                return false;
            }

            if (!drug.IsSociallyProper(pawn))
            {
                return false;
            }

            return true;
        }

        // Token: 0x06000050 RID: 80 RVA: 0x000054F0 File Offset: 0x000036F0
        private static bool ImmuneNow(Pawn pawn, HediffDef chkhdef)
        {
            if (pawn == null || chkhdef == null)
            {
                return false;
            }

            var health = pawn.health;
            var hediffSet = health?.hediffSet;
            var set = hediffSet;
            var chk = set?.GetFirstHediffOfDef(chkhdef);
            if (chk != null && chk.FullyImmune())
            {
                return true;
            }

            return false;
        }
    }
}