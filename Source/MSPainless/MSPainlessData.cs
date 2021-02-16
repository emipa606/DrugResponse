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
        // Token: 0x04000022 RID: 34
        private List<string> DRResponse = new List<string>();

        // Token: 0x04000021 RID: 33
        private int LastPainReliefTick;

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000069 RID: 105 RVA: 0x000062C6 File Offset: 0x000044C6
        private Pawn Pawn => (Pawn) parent;

        // Token: 0x06000068 RID: 104 RVA: 0x00006270 File Offset: 0x00004470
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref LastPainReliefTick, "LastPainReliefTick");
            Scribe_Collections.Look(ref DRResponse, "DRResponse", LookMode.Value, Array.Empty<object>());
            if (Scribe.mode != LoadSaveMode.Saving && DRResponse == null)
            {
                DRResponse = new List<string>();
            }
        }

        // Token: 0x0600006A RID: 106 RVA: 0x000062D4 File Offset: 0x000044D4
        public override void CompTick()
        {
            var responseTicks = 150;
            if (DRSettings.UsePainManagement && MSDrugUtility.IsValidPawnMod(Pawn, true) &&
                Pawn.IsHashIntervalTick(responseTicks))
            {
                var pawn = Pawn;
                if (pawn?.Map != null)
                {
                    var pawn2 = Pawn;
                    if (pawn2?.CurJob != null && Pawn.CurJob.def != JobDefOf.Ingest)
                    {
                        MSPainResponse.CheckPainResponse(Pawn);
                    }
                }
                else if (Pawn.IsCaravanMember())
                {
                    MSPainResponse.CheckCaravanPainResponse(Pawn);
                }
            }

            if (!DRSettings.UseDrugResponse || !MSDrugUtility.IsValidPawnMod(Pawn, false) ||
                !Pawn.IsHashIntervalTick(responseTicks + 50))
            {
                return;
            }

            var pawn3 = Pawn;
            if (pawn3?.Map != null)
            {
                var pawn4 = Pawn;
                if (pawn4?.CurJob == null || Pawn.CurJob.def == JobDefOf.Ingest)
                {
                    return;
                }

                var pawn5 = Pawn;
                HediffSet hediffSet;
                if (pawn5 == null)
                {
                    hediffSet = null;
                }
                else
                {
                    var health = pawn5.health;
                    hediffSet = health?.hediffSet;
                }

                var hedSet = hediffSet;
                if (hedSet == null || hedSet.hediffs.Count <= 0)
                {
                    return;
                }

                using (var enumerator = hedSet.hediffs.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        var hediff = enumerator.Current;
                        if (!hediff.Visible || IsProsthetic(hediff) || !MSDRUtility.MaladyUsed(hediff.def.defName))
                        {
                            continue;
                        }

                        for (var i = 1; i <= 2; i++)
                        {
                            if (!CheckIfResponse(Pawn, hediff.def.defName, DRResponse, i, out var drugdef) ||
                                !MSDrugUtility.IsOKtoAdmin(Pawn, hediff.def, drugdef) ||
                                MSDrugUtility.IsViolation(Pawn, drugdef))
                            {
                                continue;
                            }

                            var drug = MSDrugUtility.FindDrugFor(Pawn, drugdef);
                            if (drug == null)
                            {
                                continue;
                            }

                            if (i < 2 && MSDRUtility.GetValueDRBills(hediff.def.defName, DRSettings.MSDRValues) ||
                                i > 1 && MSDRUtility.GetValueDRBills(hediff.def.defName, DRSettings.MSDRValues2) ||
                                Pawn.IsPrisoner && DRSettings.DoDRIfPrisoner)
                            {
                                if ((i >= 2 || !MSAddDrugBill.GenDrugResponse(true, Pawn, hediff.def, drugdef, null,
                                    DRSettings.MSDRValues, i)) && (i <= 1 || !MSAddDrugBill.GenDrugResponse(true, Pawn,
                                    hediff.def, drugdef, null, DRSettings.MSDRValues2, i)))
                                {
                                    continue;
                                }

                                SetDRResponseData(Pawn, hediff.def.defName, drugdef.defName, Find.TickManager.TicksGame,
                                    DRResponse, i, out var newDRResponse);
                                DRResponse = newDRResponse;
                                DoDRResponseMsg(Pawn, hediff.def, drugdef);
                            }
                            else if (IsDRCapable(Pawn))
                            {
                                var job = new Job(JobDefOf.Ingest, drug)
                                {
                                    count = Mathf.Min(drug.stackCount, drug.def.ingestible.maxNumToIngestAtOnce, 1)
                                };
                                if (drug.Spawned && Pawn.drugs != null &&
                                    !Pawn.inventory.innerContainer.Contains(drug.def))
                                {
                                    var drugPolicyEntry = Pawn.drugs.CurrentPolicy[drug.def];
                                    if (drugPolicyEntry.takeToInventory > 0)
                                    {
                                        job.takeExtraIngestibles = drugPolicyEntry.takeToInventory;
                                    }
                                }

                                if (Pawn.jobs?.curJob != null)
                                {
                                    Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                                    Pawn.jobs.ClearQueuedJobs();
                                }

                                SetDRResponseData(Pawn, hediff.def.defName, drugdef.defName, Find.TickManager.TicksGame,
                                    DRResponse, i, out var newDRResponse2);
                                DRResponse = newDRResponse2;
                                DoDRResponseMsg(Pawn, hediff.def, drugdef);
                                Pawn.jobs.TryTakeOrderedJob(job);
                            }
                        }

                        var jobs = Pawn.jobs;
                        if (jobs?.curJob != null && Pawn.jobs.curJob.def == JobDefOf.Ingest)
                        {
                            break;
                        }
                    }

                    return;
                }
            }

            if (!Pawn.IsCaravanMember())
            {
                return;
            }

            var pawn6 = Pawn;
            HediffSet hediffSet2;
            if (pawn6 == null)
            {
                hediffSet2 = null;
            }
            else
            {
                var health2 = pawn6.health;
                hediffSet2 = health2?.hediffSet;
            }

            var hedSet2 = hediffSet2;
            if (hedSet2 == null || hedSet2.hediffs.Count <= 0)
            {
                return;
            }

            foreach (var hediff2 in hedSet2.hediffs)
            {
                if (!hediff2.Visible || IsProsthetic(hediff2) || !MSDRUtility.MaladyUsed(hediff2.def.defName))
                {
                    continue;
                }

                for (var j = 1; j <= 2; j++)
                {
                    if (!CheckIfResponse(Pawn, hediff2.def.defName, DRResponse, j, out var drugdef2) ||
                        !MSDrugUtility.IsOKtoAdmin(Pawn, hediff2.def, drugdef2) ||
                        MSDrugUtility.IsViolation(Pawn, drugdef2))
                    {
                        continue;
                    }

                    var car = Pawn.GetCaravan();
                    if (car == null || !MSCaravanUtility.CaravanHasDrug(car, drugdef2, out var drug2, out var owner))
                    {
                        continue;
                    }

                    SetDRResponseData(Pawn, hediff2.def.defName, drugdef2.defName, Find.TickManager.TicksGame,
                        DRResponse, j, out var newDRResponse3);
                    DRResponse = newDRResponse3;
                    MSCaravanUtility.PawnOnCaravanTakeDrug(car, Pawn, drug2, owner);
                    DoDRResponseMsg(Pawn, hediff2.def, drugdef2);
                }
            }
        }

        // Token: 0x0600006B RID: 107 RVA: 0x0000695C File Offset: 0x00004B5C
        private static bool IsProsthetic(Hediff h)
        {
            return h is Hediff_AddedPart;
        }

        // Token: 0x0600006C RID: 108 RVA: 0x0000696C File Offset: 0x00004B6C
        private static void DoDRResponseMsg(Pawn pawn, HediffDef hediffdef, ThingDef thingdef)
        {
            if (DRSettings.ShowResponseMsg)
            {
                Messages.Message(
                    "MSPainless.ResponseMsg".Translate(pawn.LabelShort.CapitalizeFirst(),
                        hediffdef.label.CapitalizeFirst(), thingdef.label.CapitalizeFirst()), pawn,
                    MessageTypeDefOf.NeutralEvent);
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000069D0 File Offset: 0x00004BD0
        private static bool CheckIfResponse(Pawn pawn, string hdefname, List<string> ResponseList, int num,
            out ThingDef drugdef)
        {
            var result = false;
            drugdef = null;
            var drugdefname =
                MSDRUtility.GetValueDRDrug(hdefname, num < 2 ? DRSettings.MSDRValues : DRSettings.MSDRValues2, num);
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

                var lasttick = GetDRResponseLastTick(pawn, hdefname, ResponseList, num);
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

            if (!result)
            {
                return false;
            }

            var chkdef = DefDatabase<ThingDef>.GetNamed(drugdefname, false);
            if (chkdef != null)
            {
                drugdef = chkdef;
            }
            else
            {
                result = false;
                Log.Message(string.Concat("Warning DR: Missing ThingDef for ", drugdefname, " as response to malady ",
                    hdefname, "(Possible mod list change)"));
            }

            return result;
        }

        // Token: 0x0600006E RID: 110 RVA: 0x00006AA4 File Offset: 0x00004CA4
        private static bool IsDRCapable(Pawn pawn)
        {
            return (pawn.IsColonistPlayerControlled || pawn.IsPrisoner) && !pawn.Downed &&
                   pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !MedicallyPrevented(pawn) &&
                   !pawn.IsBurning() && !pawn.InMentalState;
        }

        // Token: 0x0600006F RID: 111 RVA: 0x00006AFF File Offset: 0x00004CFF
        public static bool MedicallyPrevented(Pawn pawn)
        {
            return HealthAIUtility.ShouldSeekMedicalRest(pawn) && !RelativeHealthOk(pawn);
        }

        // Token: 0x06000070 RID: 112 RVA: 0x00006B18 File Offset: 0x00004D18
        private static bool RelativeHealthOk(Pawn pawn)
        {
            if (pawn == null)
            {
                return false;
            }

            var health = pawn.health;
            bool flag;
            if (health == null)
            {
                flag = false;
            }
            else
            {
                var summaryHealth = health.summaryHealth;
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

            return false;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x00006B64 File Offset: 0x00004D64
        public static bool IsPainManager(Pawn pawn)
        {
            return pawn.TryGetComp<MSPainlessData>() != null;
        }

        // Token: 0x06000072 RID: 114 RVA: 0x00006B74 File Offset: 0x00004D74
        public static int GetLastPainReliefTick(Pawn pawn)
        {
            ThingComp PComp = pawn?.TryGetComp<MSPainlessData>();
            if (PComp != null)
            {
                return ((MSPainlessData) PComp).LastPainReliefTick;
            }

            return 0;
        }

        // Token: 0x06000073 RID: 115 RVA: 0x00006BA0 File Offset: 0x00004DA0
        public static void MSPainlessDataTickSet(Pawn pawn)
        {
            ThingComp PComp = pawn?.TryGetComp<MSPainlessData>();
            if (PComp is MSPainlessData PData)
            {
                PData.LastPainReliefTick = Find.TickManager.TicksGame;
            }
        }

        // Token: 0x06000074 RID: 116 RVA: 0x00006BD8 File Offset: 0x00004DD8
        private static int GetDRResponseLastTick(Pawn pawn, string m, List<string> responseList, int num)
        {
            if (responseList == null || responseList.Count <= 0)
            {
                return 0;
            }

            foreach (var response in responseList)
            {
                var hdefname = HFromDRResponse(response);
                var numof = NumFromDRResponse(response);
                if (hdefname == m && numof == num)
                {
                    return LTFromDRResponse(response);
                }
            }

            return 0;
        }

        // Token: 0x06000075 RID: 117 RVA: 0x00006C54 File Offset: 0x00004E54
        private static void SetDRResponseData(Pawn pawn, string m, string d, int last, List<string> master, int num,
            out List<string> newMaster)
        {
            newMaster = new List<string>();
            var newValue = ConvertToDRRData(m, last, num);
            if (master == null)
            {
                return;
            }

            var beenSet = false;
            if (master.Count > 0)
            {
                foreach (var Response in master)
                {
                    var hdefname = HFromDRResponse(Response);
                    var numof = NumFromDRResponse(Response);
                    if (hdefname == m && numof == num)
                    {
                        newMaster.AddDistinct(newValue);
                        beenSet = true;
                    }
                    else
                    {
                        var time = LTFromDRResponse(Response);
                        var oldValue = ConvertToDRRData(hdefname, time, numof);
                        newMaster.AddDistinct(oldValue);
                    }
                }
            }

            if (!beenSet)
            {
                newMaster.AddDistinct(newValue);
            }
        }

        // Token: 0x06000076 RID: 118 RVA: 0x00006D1C File Offset: 0x00004F1C
        private static int NumFromDRResponse(string Response)
        {
            var divider = new[]
            {
                ','
            };
            var segments = Response.Split(divider);
            try
            {
                if (segments.Length > 2)
                {
                    return int.Parse(segments[2]);
                }

                return 0;
            }
            catch (FormatException)
            {
                Log.Message("Unable to parse Seg0: '" + segments[0] + "'");
            }

            return 0;
        }

        // Token: 0x06000077 RID: 119 RVA: 0x00006D84 File Offset: 0x00004F84
        private static int LTFromDRResponse(string Response)
        {
            var divider = new[]
            {
                ','
            };
            var segments = Response.Split(divider);
            try
            {
                return int.Parse(segments[0]);
            }
            catch (FormatException)
            {
                Log.Message("Unable to parse Seg0: '" + segments[0] + "'");
            }

            return 0;
        }

        // Token: 0x06000078 RID: 120 RVA: 0x00006DE0 File Offset: 0x00004FE0
        private static string HFromDRResponse(string Response)
        {
            var divider = new[]
            {
                ','
            };
            return Response.Split(divider)[1];
        }

        // Token: 0x06000079 RID: 121 RVA: 0x00006E02 File Offset: 0x00005002
        private static string ConvertToDRRData(string m, int last, int num)
        {
            return string.Concat(last.ToString(), ",", m, ",", num.ToString()).Trim();
        }

        // Token: 0x0200001E RID: 30
        private class CompProperties_MSPainlessData : CompProperties
        {
            // Token: 0x060000E5 RID: 229 RVA: 0x00007CA6 File Offset: 0x00005EA6
            public CompProperties_MSPainlessData()
            {
                compClass = typeof(MSPainlessData);
            }
        }

        // Token: 0x0200001F RID: 31
        [StaticConstructorOnStartup]
        private static class MSPainlessData_Setup
        {
            // Token: 0x060000E6 RID: 230 RVA: 0x00007CBE File Offset: 0x00005EBE
            static MSPainlessData_Setup()
            {
                MSPainlessData_Setup_Pawns();
            }

            // Token: 0x060000E7 RID: 231 RVA: 0x00007CC8 File Offset: 0x00005EC8
            private static void MSPainlessData_Setup_Pawns()
            {
                var Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard");
                MSPainlessDataSetup_Comp(typeof(CompProperties_MSPainlessData), delegate(ThingDef def)
                {
                    var race = def.race;
                    if (race == null || !race.Humanlike)
                    {
                        return false;
                    }

                    var race2 = def.race;
                    return race2?.hediffGiverSets != null && def.race.hediffGiverSets.Contains(Organic);
                });
            }

            // Token: 0x060000E8 RID: 232 RVA: 0x00007D08 File Offset: 0x00005F08
            private static void MSPainlessDataSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
            {
                var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
                list.RemoveDuplicates();
                foreach (var def in list)
                {
                    if (def.comps != null && !def.comps.Any(c => c.GetType() == compType))
                    {
                        def.comps.Add((CompProperties) Activator.CreateInstance(compType));
                    }
                }
            }
        }
    }
}