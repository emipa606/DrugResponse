using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MSPainless;

public class MSPainResponse
{
    public static void CheckPainResponse(Pawn pawn)
    {
        if (!pawn.IsColonist && (!pawn.IsPrisoner || !DRSettings.DoIfPrisoner) ||
            !MSPainlessData.IsPainManager(pawn) || !MSPainUtility.IsInPain(pawn) || pawn.IsBurning() ||
            pawn.InMentalState || pawn.Drafted || (!pawn.Awake() || pawn.IsPrisoner) && !pawn.IsPrisoner)
        {
            return;
        }

        var painjob = DoPainReliefResponse(pawn);
        if (painjob == null)
        {
            return;
        }

        if (pawn.jobs?.curJob != null)
        {
            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            pawn.jobs.ClearQueuedJobs();
        }

        pawn.jobs?.TryTakeOrderedJob(painjob);
    }

    public static void CheckCaravanPainResponse(Pawn pawn)
    {
        if (!pawn.IsCaravanMember())
        {
            return;
        }

        var car = pawn.GetCaravan();
        if (car != null && (pawn.IsColonist || pawn.IsPrisoner && DRSettings.DoIfPrisoner) &&
            MSPainlessData.IsPainManager(pawn) && MSPainUtility.IsInPain(pawn) && !pawn.InMentalState &&
            !car.NightResting)
        {
            DoCaravanPainReliefResponse(car, pawn);
        }
    }

    private static void DoCaravanPainReliefResponse(Caravan car, Pawn pawn)
    {
        if (MSPainlessData.GetLastPainReliefTick(pawn) + (2500 * DRSettings.PainReliefWaitPeriod) >
            Find.TickManager.TicksGame)
        {
            return;
        }

        var painDrugDef = MSPainUtility.NeedsPainReliefNow(pawn, out _);
        if (painDrugDef == null || !MSDrugUtility.IsOKtoAdmin(pawn, null, painDrugDef) ||
            MSDrugUtility.IsViolation(pawn, painDrugDef) ||
            !MSCaravanUtility.CaravanHasDrug(car, painDrugDef, out var drug, out var owner))
        {
            return;
        }

        MSPainlessData.MSPainlessDataTickSet(pawn);
        MSCaravanUtility.PawnOnCaravanTakeDrug(car, pawn, drug, owner);
        if (DRSettings.ShowReliefMsg)
        {
            Messages.Message(
                "MSPainless.ReliefMsg".Translate(pawn.LabelShort.CapitalizeFirst(),
                    drug.def.label.CapitalizeFirst()), pawn, MessageTypeDefOf.NeutralEvent, false);
        }
    }

    private static Job DoPainReliefResponse(Pawn pawn)
    {
        if (pawn?.CurJob != null && pawn.CurJob.def == JobDefOf.Ingest)
        {
            return null;
        }

        if (MSPainlessData.GetLastPainReliefTick(pawn) + (2500 * DRSettings.PainReliefWaitPeriod) >
            Find.TickManager.TicksGame)
        {
            return null;
        }

        var painDrugDef = MSPainUtility.NeedsPainReliefNow(pawn, out var highpain);
        if (painDrugDef == null || !MSDrugUtility.IsOKtoAdmin(pawn, null, painDrugDef) ||
            MSDrugUtility.IsViolation(pawn, painDrugDef))
        {
            return null;
        }

        var drug = MSDrugUtility.FindDrugFor(pawn, painDrugDef);
        if (drug == null)
        {
            return null;
        }

        if (DRSettings.ShowReliefMsg)
        {
            if (pawn != null)
            {
                Messages.Message(
                    "MSPainless.ReliefMsg".Translate(pawn.LabelShort.CapitalizeFirst(),
                        drug.def.label.CapitalizeFirst()), pawn, MessageTypeDefOf.NeutralEvent, false);
            }
        }

        var job = new Job(JobDefOf.Ingest, drug)
        {
            count = Mathf.Min(drug.stackCount, drug.def.ingestible.maxNumToIngestAtOnce, 1)
        };
        if (pawn != null && drug.Spawned && pawn.drugs != null && !pawn.inventory.innerContainer.Contains(drug.def))
        {
            var drugPolicyEntry = pawn.drugs.CurrentPolicy[drug.def];
            if (drugPolicyEntry.takeToInventory > 0)
            {
                job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
            }
        }

        if (pawn != null && IsUsingPainJob(pawn, highpain) && !pawn.IsPrisoner)
        {
            MSPainlessData.MSPainlessDataTickSet(pawn);
            return job;
        }

        if (!MSAddDrugBill.GenDrugResponse(false, pawn, null, painDrugDef))
        {
            return null;
        }

        MSPainlessData.MSPainlessDataTickSet(pawn);
        return null;
    }

    private static bool IsUsingPainJob(Pawn p, bool highpain)
    {
        return !DRSettings.UseReliefBills && (!highpain || !DRSettings.BillsHighPain) && !p.Downed &&
               p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !MSPainlessData.MedicallyPrevented(p);
    }
}