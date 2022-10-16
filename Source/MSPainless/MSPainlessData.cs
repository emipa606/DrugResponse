using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace MSPainless;

public class MSPainlessData : ThingComp
{
    private List<string> DRResponse = new List<string>();

    private int LastPainReliefTick;

    private Pawn Pawn => (Pawn)parent;

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

            using var enumerator = hedSet.hediffs.GetEnumerator();
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

    private static bool IsProsthetic(Hediff h)
    {
        return h is Hediff_AddedPart;
    }

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
            Log.Message(
                $"Warning DR: Missing ThingDef for {drugdefname} as response to malady {hdefname}(Possible mod list change)");
        }

        return result;
    }

    private static bool IsDRCapable(Pawn pawn)
    {
        return (pawn.IsColonistPlayerControlled || pawn.IsPrisoner) && !pawn.Downed &&
               pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving) && !MedicallyPrevented(pawn) &&
               !pawn.IsBurning() && !pawn.InMentalState;
    }

    public static bool MedicallyPrevented(Pawn pawn)
    {
        return HealthAIUtility.ShouldSeekMedicalRest(pawn) && !RelativeHealthOk(pawn);
    }

    private static bool RelativeHealthOk(Pawn pawn)
    {
        if (pawn == null)
        {
            return false;
        }

        var health = pawn.health;
        bool b;
        if (health == null)
        {
            b = false;
        }
        else
        {
            var summaryHealth = health.summaryHealth;
            if (summaryHealth == null)
            {
                b = false;
            }
            else
            {
                _ = summaryHealth.SummaryHealthPercent;
                b = true;
            }
        }

        return b && pawn.health.summaryHealth.SummaryHealthPercent >= 0.75f;
    }

    public static bool IsPainManager(Pawn pawn)
    {
        return pawn.TryGetComp<MSPainlessData>() != null;
    }

    public static int GetLastPainReliefTick(Pawn pawn)
    {
        ThingComp PComp = pawn?.TryGetComp<MSPainlessData>();
        return ((MSPainlessData)PComp)?.LastPainReliefTick ?? 0;
    }

    public static void MSPainlessDataTickSet(Pawn pawn)
    {
        ThingComp PComp = pawn?.TryGetComp<MSPainlessData>();
        if (PComp is MSPainlessData PData)
        {
            PData.LastPainReliefTick = Find.TickManager.TicksGame;
        }
    }

    private static int GetDRResponseLastTick(Pawn pawn, string m, List<string> responseList, int num)
    {
        if (responseList is not { Count: > 0 })
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

    private static int NumFromDRResponse(string Response)
    {
        var divider = new[]
        {
            ','
        };
        var segments = Response.Split(divider);
        try
        {
            return segments.Length > 2 ? int.Parse(segments[2]) : 0;
        }
        catch (FormatException)
        {
            Log.Message($"Unable to parse Seg0: '{segments[0]}'");
        }

        return 0;
    }

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
            Log.Message($"Unable to parse Seg0: '{segments[0]}'");
        }

        return 0;
    }

    private static string HFromDRResponse(string Response)
    {
        var divider = new[]
        {
            ','
        };
        return Response.Split(divider)[1];
    }

    private static string ConvertToDRRData(string m, int last, int num)
    {
        return $"{last},{m},{num}".Trim();
    }

    private class CompProperties_MSPainlessData : CompProperties
    {
        public CompProperties_MSPainlessData()
        {
            compClass = typeof(MSPainlessData);
        }
    }

    [StaticConstructorOnStartup]
    private static class MSPainlessData_Setup
    {
        static MSPainlessData_Setup()
        {
            MSPainlessData_Setup_Pawns();
        }

        private static void MSPainlessData_Setup_Pawns()
        {
            var Organic = DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard");
            MSPainlessDataSetup_Comp(typeof(CompProperties_MSPainlessData), delegate(ThingDef def)
            {
                var race = def.race;
                if (race is not { Humanlike: true })
                {
                    return false;
                }

                var race2 = def.race;
                return race2?.hediffGiverSets != null && def.race.hediffGiverSets.Contains(Organic);
            });
        }

        private static void MSPainlessDataSetup_Comp(Type compType, Func<ThingDef, bool> qualifier)
        {
            var list = DefDatabase<ThingDef>.AllDefsListForReading.Where(qualifier).ToList();
            list.RemoveDuplicates();
            foreach (var def in list)
            {
                if (def.comps != null && !def.comps.Any(c => c.GetType() == compType))
                {
                    def.comps.Add((CompProperties)Activator.CreateInstance(compType));
                }
            }
        }
    }
}