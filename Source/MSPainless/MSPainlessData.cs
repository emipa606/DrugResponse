using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MSPainless
{
	// Token: 0x02000009 RID: 9
	public class MSPainlessData : ThingComp
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00006270 File Offset: 0x00004470
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.LastPainReliefTick, "LastPainReliefTick", 0, false);
			Scribe_Collections.Look<string>(ref this.DRResponse, "DRResponse", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving && this.DRResponse == null)
			{
				this.DRResponse = new List<string>();
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000062C6 File Offset: 0x000044C6
		private Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000062D4 File Offset: 0x000044D4
		public override void CompTick()
		{
			int responseTicks = 150;
			if (DRSettings.UsePainManagement && MSDrugUtility.IsValidPawnMod(this.Pawn, true) && Gen.IsHashIntervalTick(this.Pawn, responseTicks))
			{
				Pawn pawn = this.Pawn;
				if ((pawn?.Map) != null)
				{
					Pawn pawn2 = this.Pawn;
					if ((pawn2?.CurJob) != null && this.Pawn.CurJob.def != JobDefOf.Ingest)
					{
						MSPainResponse.CheckPainResponse(this.Pawn);
					}
				}
				else if (CaravanUtility.IsCaravanMember(this.Pawn))
				{
					MSPainResponse.CheckCaravanPainResponse(this.Pawn);
				}
			}
			if (DRSettings.UseDrugResponse && MSDrugUtility.IsValidPawnMod(this.Pawn, false) && Gen.IsHashIntervalTick(this.Pawn, responseTicks + 50))
			{
				Pawn pawn3 = this.Pawn;
				if ((pawn3?.Map) != null)
				{
					Pawn pawn4 = this.Pawn;
					if ((pawn4?.CurJob) == null || this.Pawn.CurJob.def == JobDefOf.Ingest)
					{
						return;
					}
					Pawn pawn5 = this.Pawn;
					HediffSet hediffSet;
					if (pawn5 == null)
					{
						hediffSet = null;
					}
					else
					{
						Pawn_HealthTracker health = pawn5.health;
						hediffSet = (health?.hediffSet);
					}
					HediffSet hedSet = hediffSet;
					if (hedSet == null || hedSet == null || hedSet.hediffs.Count <= 0)
					{
						return;
					}
					using (List<Hediff>.Enumerator enumerator = hedSet.hediffs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Hediff hediff = enumerator.Current;
							if (hediff.Visible && !MSPainlessData.IsProsthetic(hediff) && MSDRUtility.MaladyUsed(hediff.def.defName))
							{
								for (int i = 1; i <= 2; i++)
								{
                                    if (MSPainlessData.CheckIfResponse(this.Pawn, hediff.def.defName, this.DRResponse, i, out ThingDef drugdef) && MSDrugUtility.IsOKtoAdmin(this.Pawn, hediff.def, drugdef) && !MSDrugUtility.IsViolation(this.Pawn, drugdef))
                                    {
                                        Thing drug = MSDrugUtility.FindDrugFor(this.Pawn, drugdef);
                                        if (drug != null)
                                        {
                                            if ((i < 2 && MSDRUtility.GetValueDRBills(hediff.def.defName, DRSettings.MSDRValues)) || (i > 1 && MSDRUtility.GetValueDRBills(hediff.def.defName, DRSettings.MSDRValues2)) || (this.Pawn.IsPrisoner && DRSettings.DoDRIfPrisoner))
                                            {
                                                if ((i < 2 && MSAddDrugBill.GenDrugResponse(true, this.Pawn, hediff.def, drugdef, null, DRSettings.MSDRValues, i)) || (i > 1 && MSAddDrugBill.GenDrugResponse(true, this.Pawn, hediff.def, drugdef, null, DRSettings.MSDRValues2, i)))
                                                {
                                                    MSPainlessData.SetDRResponseData(this.Pawn, hediff.def.defName, drugdef.defName, Find.TickManager.TicksGame, this.DRResponse, i, out List<string> newDRResponse);
                                                    this.DRResponse = newDRResponse;
                                                    MSPainlessData.DoDRResponseMsg(this.Pawn, hediff.def, drugdef);
                                                }
                                            }
                                            else if (MSPainlessData.IsDRCapable(this.Pawn))
                                            {
                                                Job job = new Job(JobDefOf.Ingest, drug)
                                                {
                                                    count = Mathf.Min(new int[]
                                                {
                                                    drug.stackCount,
                                                    drug.def.ingestible.maxNumToIngestAtOnce,
                                                    1
                                                })
                                                };
                                                if (drug.Spawned && this.Pawn.drugs != null && !this.Pawn.inventory.innerContainer.Contains(drug.def))
                                                {
                                                    DrugPolicyEntry drugPolicyEntry = this.Pawn.drugs.CurrentPolicy[drug.def];
                                                    if (drugPolicyEntry.takeToInventory > 0)
                                                    {
                                                        job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
                                                    }
                                                }
                                                if (this.Pawn.jobs != null && this.Pawn.jobs.curJob != null)
                                                {
                                                    this.Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
                                                    this.Pawn.jobs.ClearQueuedJobs(true);
                                                }
                                                MSPainlessData.SetDRResponseData(this.Pawn, hediff.def.defName, drugdef.defName, Find.TickManager.TicksGame, this.DRResponse, i, out List<string> newDRResponse2);
                                                this.DRResponse = newDRResponse2;
                                                MSPainlessData.DoDRResponseMsg(this.Pawn, hediff.def, drugdef);
                                                this.Pawn.jobs.TryTakeOrderedJob(job, 0);
                                            }
                                        }
                                    }
                                }
								Pawn_JobTracker jobs = this.Pawn.jobs;
								if ((jobs?.curJob) != null && this.Pawn.jobs.curJob.def == JobDefOf.Ingest)
								{
									break;
								}
							}
						}
						return;
					}
				}
				if (CaravanUtility.IsCaravanMember(this.Pawn))
				{
					Pawn pawn6 = this.Pawn;
					HediffSet hediffSet2;
					if (pawn6 == null)
					{
						hediffSet2 = null;
					}
					else
					{
						Pawn_HealthTracker health2 = pawn6.health;
						hediffSet2 = (health2?.hediffSet);
					}
					HediffSet hedSet2 = hediffSet2;
					if (hedSet2 != null && hedSet2 != null && hedSet2.hediffs.Count > 0)
					{
						foreach (Hediff hediff2 in hedSet2.hediffs)
						{
							if (hediff2.Visible && !MSPainlessData.IsProsthetic(hediff2) && MSDRUtility.MaladyUsed(hediff2.def.defName))
							{
								for (int j = 1; j <= 2; j++)
								{
                                    if (MSPainlessData.CheckIfResponse(this.Pawn, hediff2.def.defName, this.DRResponse, j, out ThingDef drugdef2) && MSDrugUtility.IsOKtoAdmin(this.Pawn, hediff2.def, drugdef2) && !MSDrugUtility.IsViolation(this.Pawn, drugdef2))
                                    {
                                        Caravan car = CaravanUtility.GetCaravan(this.Pawn);
                                        if (car != null && MSCaravanUtility.CaravanHasDrug(car, drugdef2, out Thing drug2, out Pawn owner))
                                        {
                                            MSPainlessData.SetDRResponseData(this.Pawn, hediff2.def.defName, drugdef2.defName, Find.TickManager.TicksGame, this.DRResponse, j, out List<string> newDRResponse3);
                                            this.DRResponse = newDRResponse3;
                                            MSCaravanUtility.PawnOnCaravanTakeDrug(car, this.Pawn, drug2, owner);
                                            MSPainlessData.DoDRResponseMsg(this.Pawn, hediff2.def, drugdef2);
                                        }
                                    }
                                }
							}
						}
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000695C File Offset: 0x00004B5C
		public static bool IsProsthetic(Hediff h)
		{
			return h is Hediff_AddedPart;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000696C File Offset: 0x00004B6C
		public static void DoDRResponseMsg(Pawn pawn, HediffDef hediffdef, ThingDef thingdef)
		{
			if (DRSettings.ShowResponseMsg)
			{
				Messages.Message(TranslatorFormattedStringExtensions.Translate("MSPainless.ResponseMsg", GenText.CapitalizeFirst(pawn.LabelShort), GenText.CapitalizeFirst(hediffdef.label), GenText.CapitalizeFirst(thingdef.label)), pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000069D0 File Offset: 0x00004BD0
		public static bool CheckIfResponse(Pawn pawn, string hdefname, List<string> ResponseList, int num, out ThingDef drugdef)
		{
			bool result = false;
			drugdef = null;
			string drugdefname;
			if (num < 2)
			{
				drugdefname = MSDRUtility.GetValueDRDrug(hdefname, DRSettings.MSDRValues, num);
			}
			else
			{
				drugdefname = MSDRUtility.GetValueDRDrug(hdefname, DRSettings.MSDRValues2, num);
			}
			if (drugdefname != null)
			{
				int ticksDR;
				if (num < 2)
				{
					ticksDR = MSDRUtility.GetValueDRTime(hdefname, DRSettings.MSDRValues, num) * 2500;
				}
				else
				{
					ticksDR = MSDRUtility.GetValueDRTime(hdefname, DRSettings.MSDRValues2, num) * 2500;
				}
				int lasttick = MSPainlessData.GetDRResponseLastTick(pawn, hdefname, ResponseList, num);
				if (lasttick > 0)
				{
					if (lasttick + ticksDR < Find.TickManager.TicksGame)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			if (result)
			{
				ThingDef chkdef = DefDatabase<ThingDef>.GetNamed(drugdefname, false);
				if (chkdef != null)
				{
					drugdef = chkdef;
				}
				else
				{
					result = false;
					Log.Message(string.Concat(new string[]
					{
						"Warning DR: Missing ThingDef for ",
						drugdefname,
						" as response to malady ",
						hdefname,
						"(Possible mod list change)"
					}), false);
				}
			}
			return result;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00006AA4 File Offset: 0x00004CA4
		public static bool IsDRCapable(Pawn pawn)
		{
			return (pawn.IsColonistPlayerControlled || pawn.IsPrisoner) && !pawn.Downed && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !MSPainlessData.MedicallyPrevented(pawn) && !FireUtility.IsBurning(pawn) && !pawn.InMentalState;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00006AFF File Offset: 0x00004CFF
		public static bool MedicallyPrevented(Pawn pawn)
		{
			return HealthAIUtility.ShouldSeekMedicalRest(pawn) && !MSPainlessData.RelativeHealthOk(pawn);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00006B18 File Offset: 0x00004D18
		private static bool RelativeHealthOk(Pawn pawn)
		{
			if (pawn != null)
			{
				Pawn_HealthTracker health = pawn.health;
                bool flag;
                if (health == null)
				{
					flag = false;
				}
				else
				{
					SummaryHealthHandler summaryHealth = health.summaryHealth;
					if (summaryHealth == null)
					{
						flag = false;
					}
					else
					{
                        _ = summaryHealth.SummaryHealthPercent;
                        flag = true;
					}
				}
				if (flag && pawn.health.summaryHealth.SummaryHealthPercent >= 0.75f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00006B64 File Offset: 0x00004D64
		public static bool IsPainManager(Pawn pawn)
		{
			return ThingCompUtility.TryGetComp<MSPainlessData>(pawn) != null;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00006B74 File Offset: 0x00004D74
		public static int GetLastPainReliefTick(Pawn pawn)
		{
			ThingComp PComp = (pawn != null) ? ThingCompUtility.TryGetComp<MSPainlessData>(pawn) : null;
			if (PComp != null)
			{
				return (PComp as MSPainlessData).LastPainReliefTick;
			}
			return 0;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00006BA0 File Offset: 0x00004DA0
		public static void MSPainlessDataTickSet(Pawn pawn)
		{
			ThingComp PComp = (pawn != null) ? ThingCompUtility.TryGetComp<MSPainlessData>(pawn) : null;
			if (PComp != null)
			{
                if (PComp is MSPainlessData PData)
                {
                    PData.LastPainReliefTick = Find.TickManager.TicksGame;
                }
            }
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006BD8 File Offset: 0x00004DD8
		public static int GetDRResponseLastTick(Pawn pawn, string m, List<string> responseList, int num)
		{
			if (responseList != null && responseList.Count > 0)
			{
				foreach (string response in responseList)
				{
					string hdefname = MSPainlessData.HFromDRResponse(response);
					int numof = MSPainlessData.NumFromDRResponse(response);
					if (hdefname == m && numof == num)
					{
						return MSPainlessData.LTFromDRResponse(response);
					}
				}
				return 0;
			}
			return 0;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006C54 File Offset: 0x00004E54
		public static void SetDRResponseData(Pawn pawn, string m, string d, int last, List<string> master, int num, out List<string> newMaster)
		{
			newMaster = new List<string>();
			string newValue = MSPainlessData.ConvertToDRRData(m, last, num);
			if (master != null)
			{
				bool beenSet = false;
				if (master.Count > 0)
				{
					foreach (string Response in master)
					{
						string hdefname = MSPainlessData.HFromDRResponse(Response);
						int numof = MSPainlessData.NumFromDRResponse(Response);
						if (hdefname == m && numof == num)
						{
							GenCollection.AddDistinct<string>(newMaster, newValue);
							beenSet = true;
						}
						else
						{
							int time = MSPainlessData.LTFromDRResponse(Response);
							string oldValue = MSPainlessData.ConvertToDRRData(hdefname, time, numof);
							GenCollection.AddDistinct<string>(newMaster, oldValue);
						}
					}
				}
				if (!beenSet)
				{
					GenCollection.AddDistinct<string>(newMaster, newValue);
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006D1C File Offset: 0x00004F1C
		public static int NumFromDRResponse(string Response)
		{
			char[] divider = new char[]
			{
				','
			};
			string[] segments = Response.Split(divider);
			try
			{
				if (segments.Count<string>() > 2)
				{
					return int.Parse(segments[2]);
				}
				return 0;
			}
			catch (FormatException)
			{
				Log.Message("Unable to parse Seg0: '" + segments[0] + "'", false);
			}
			return 0;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006D84 File Offset: 0x00004F84
		public static int LTFromDRResponse(string Response)
		{
			char[] divider = new char[]
			{
				','
			};
			string[] segments = Response.Split(divider);
			try
			{
				return int.Parse(segments[0]);
			}
			catch (FormatException)
			{
				Log.Message("Unable to parse Seg0: '" + segments[0] + "'", false);
			}
			return 0;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public static string HFromDRResponse(string Response)
		{
			char[] divider = new char[]
			{
				','
			};
			return Response.Split(divider)[1];
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00006E02 File Offset: 0x00005002
		public static string ConvertToDRRData(string m, int last, int num)
		{
			return string.Concat(new string[]
			{
				last.ToString(),
				",",
				m,
				",",
				num.ToString()
			}).Trim();
		}

		// Token: 0x04000021 RID: 33
		public int LastPainReliefTick;

		// Token: 0x04000022 RID: 34
		public List<string> DRResponse = new List<string>();

		// Token: 0x0200001E RID: 30
		public class CompProperties_MSPainlessData : CompProperties
		{
			// Token: 0x060000E5 RID: 229 RVA: 0x00007CA6 File Offset: 0x00005EA6
			public CompProperties_MSPainlessData()
			{
				this.compClass = typeof(MSPainlessData);
			}
		}

		// Token: 0x0200001F RID: 31
		[StaticConstructorOnStartup]
		private static class MSPainlessData_Setup
		{
			// Token: 0x060000E6 RID: 230 RVA: 0x00007CBE File Offset: 0x00005EBE
			static MSPainlessData_Setup()
			{
				MSPainlessData.MSPainlessData_Setup.MSPainlessData_Setup_Pawns();
			}

			// Token: 0x060000E7 RID: 231 RVA: 0x00007CC8 File Offset: 0x00005EC8
			private static void MSPainlessData_Setup_Pawns()
			{
				HediffGiverSetDef Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard", true);
				MSPainlessData.MSPainlessData_Setup.MSPainlessDataSetup_Comp(typeof(MSPainlessData.CompProperties_MSPainlessData), delegate(ThingDef def)
				{
					RaceProperties race = def.race;
					if (race != null && race.Humanlike)
					{
						RaceProperties race2 = def.race;
						return (race2?.hediffGiverSets) != null && def.race.hediffGiverSets.Contains(Organic);
					}
					return false;
				});
			}

			// Token: 0x060000E8 RID: 232 RVA: 0x00007D08 File Offset: 0x00005F08
			private static void MSPainlessDataSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
			{
				List<ThingDef> list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
				GenList.RemoveDuplicates<ThingDef>(list);
				foreach (ThingDef def in list)
				{
					if (def.comps != null && !GenCollection.Any<CompProperties>(def.comps, (Predicate<CompProperties>)((CompProperties c) => ((object)c).GetType() == compType)))
					{
						def.comps.Add((CompProperties)(object)(CompProperties)Activator.CreateInstance(compType));
					}
				}
			}
		}
	}
}
