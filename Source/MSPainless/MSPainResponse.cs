using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MSPainless
{
	// Token: 0x0200000A RID: 10
	public class MSPainResponse
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00006E50 File Offset: 0x00005050
		public static void CheckPainResponse(Pawn pawn)
		{
			if ((pawn.IsColonist || (pawn.IsPrisoner && DRSettings.DoIfPrisoner)) && MSPainlessData.IsPainManager(pawn) && MSPainUtility.IsInPain(pawn) && !FireUtility.IsBurning(pawn) && !pawn.InMentalState && !pawn.Drafted && ((RestUtility.Awake(pawn) && !pawn.IsPrisoner) || pawn.IsPrisoner))
			{
				Job painjob = MSPainResponse.DoPainReliefResponse(pawn);
				if (painjob != null)
				{
					if (pawn.jobs != null && pawn.jobs.curJob != null)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						pawn.jobs.ClearQueuedJobs(true);
					}
					pawn.jobs.TryTakeOrderedJob(painjob, 0);
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00006F04 File Offset: 0x00005104
		public static void CheckCaravanPainResponse(Pawn pawn)
		{
			if (CaravanUtility.IsCaravanMember(pawn))
			{
				Caravan car = CaravanUtility.GetCaravan(pawn);
				if (car != null && (pawn.IsColonist || (pawn.IsPrisoner && DRSettings.DoIfPrisoner)) && MSPainlessData.IsPainManager(pawn) && MSPainUtility.IsInPain(pawn) && !pawn.InMentalState && !car.NightResting)
				{
					MSPainResponse.DoCaravanPainReliefResponse(car, pawn);
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00006F64 File Offset: 0x00005164
		public static void DoCaravanPainReliefResponse(Caravan car, Pawn pawn)
		{
			if (MSPainlessData.GetLastPainReliefTick(pawn) + 2500 * DRSettings.PainReliefWaitPeriod > Find.TickManager.TicksGame)
			{
				return;
			}
            ThingDef painDrugDef = MSPainUtility.NeedsPainReliefNow(pawn, out bool highpain);
            if (painDrugDef != null && MSDrugUtility.IsOKtoAdmin(pawn, null, painDrugDef) && !MSDrugUtility.IsViolation(pawn, painDrugDef) && MSCaravanUtility.CaravanHasDrug(car, painDrugDef, out Thing drug, out Pawn owner))
            {
                MSPainlessData.MSPainlessDataTickSet(pawn);
                MSCaravanUtility.PawnOnCaravanTakeDrug(car, pawn, drug, owner);
                if (DRSettings.ShowReliefMsg)
                {
                    Messages.Message(TranslatorFormattedStringExtensions.Translate("MSPainless.ReliefMsg", GenText.CapitalizeFirst(pawn.LabelShort), GenText.CapitalizeFirst(drug.def.label)), pawn, MessageTypeDefOf.NeutralEvent, false);
                }
            }
        }

		// Token: 0x0600007E RID: 126 RVA: 0x00007018 File Offset: 0x00005218
		public static Job DoPainReliefResponse(Pawn pawn)
		{
			if ((pawn?.CurJob) != null && pawn.CurJob.def == JobDefOf.Ingest)
			{
				return null;
			}
			if (MSPainlessData.GetLastPainReliefTick(pawn) + 2500 * DRSettings.PainReliefWaitPeriod > Find.TickManager.TicksGame)
			{
				return null;
			}
            ThingDef painDrugDef = MSPainUtility.NeedsPainReliefNow(pawn, out bool highpain);
            if (painDrugDef != null && MSDrugUtility.IsOKtoAdmin(pawn, null, painDrugDef) && !MSDrugUtility.IsViolation(pawn, painDrugDef))
			{
				Thing drug = MSDrugUtility.FindDrugFor(pawn, painDrugDef);
				if (drug != null)
				{
					if (DRSettings.ShowReliefMsg)
					{
						Messages.Message(TranslatorFormattedStringExtensions.Translate("MSPainless.ReliefMsg", GenText.CapitalizeFirst(pawn.LabelShort), GenText.CapitalizeFirst(drug.def.label)), pawn, MessageTypeDefOf.NeutralEvent, false);
					}
                    Job job = new Job(JobDefOf.Ingest, drug)
                    {
                        count = Mathf.Min(new int[]
                    {
                        drug.stackCount,
                        drug.def.ingestible.maxNumToIngestAtOnce,
                        1
                    })
                    };
                    if (drug.Spawned && pawn.drugs != null && !pawn.inventory.innerContainer.Contains(drug.def))
					{
						DrugPolicyEntry drugPolicyEntry = pawn.drugs.CurrentPolicy[drug.def];
						if (drugPolicyEntry.takeToInventory > 0)
						{
							job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
						}
					}
					if (MSPainResponse.IsUsingPainJob(pawn, highpain) && !pawn.IsPrisoner)
					{
						MSPainlessData.MSPainlessDataTickSet(pawn);
						return job;
					}
					if (MSAddDrugBill.GenDrugResponse(false, pawn, null, painDrugDef, null, null, 1))
					{
						MSPainlessData.MSPainlessDataTickSet(pawn);
						return null;
					}
				}
			}
			return null;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000071B4 File Offset: 0x000053B4
		private static bool IsUsingPainJob(Pawn p, bool highpain)
		{
			return !DRSettings.UseReliefBills && (!highpain || !DRSettings.BillsHighPain) && !p.Downed && p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !MSPainlessData.MedicallyPrevented(p);
		}
	}
}
