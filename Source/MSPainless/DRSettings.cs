using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace MSPainless;

public class DRSettings(World world) : WorldComponent(world)
{
    public static bool DoIfPrisoner;

    public static bool DoDRIfPrisoner;

    public static bool UseNonMedical;

    public static bool ShowResearched = true;

    public static bool BillsHighPain;

    public static bool DoIfImmune = true;

    public static bool UsePainManagement;

    public static int PainReliefWaitPeriod = 24;

    public static bool ShowReliefMsg = true;

    public static bool UseReliefBills;

    public static bool UseDrugResponse;

    public static int DRWaitPeriod = 24;

    public static int DRWaitPeriod2 = 24;

    public static bool ShowResponseMsg = true;

    public static bool UseDRBills = true;

    public static bool UseDRBills2 = true;

    private static ThingDef cachedMinorDef;

    private static ThingDef cachedSeriousDef;

    private static ThingDef cachedIntenseDef;

    private static ThingDef cachedExtremeDef;

    public static string MSDrugMinor;

    public static string MSDrugSerious;

    public static string MSDrugIntense;

    public static string MSDrugExtreme;

    public static string MSDRHed;

    public static string MSDRHed2;

    public static string MSDRThg;

    public static string MSDRThg2;

    public static List<string> MSDRValues = [];

    public static List<string> MSDRValues2 = [];

    public static ThingDef MSDrugMinorDef => GetCachedValue(MSDrugMinor, 1);

    public static ThingDef MSDrugSeriousDef => GetCachedValue(MSDrugSerious, 2);

    public static ThingDef MSDrugIntenseDef => GetCachedValue(MSDrugIntense, 3);

    public static ThingDef MSDrugExtremeDef => GetCachedValue(MSDrugExtreme, 4);

    public static string MSDrugMinorLabel => MSDrugMinorDef == null ? "None" : MSDrugMinorDef?.label.CapitalizeFirst();

    public static string MSDrugSeriousLabel =>
        MSDrugSeriousDef == null ? "None" : MSDrugSeriousDef?.label.CapitalizeFirst();

    public static string MSDrugIntenseLabel =>
        MSDrugIntenseDef == null ? "None" : MSDrugIntenseDef?.label.CapitalizeFirst();

    public static string MSDrugExtremeLabel =>
        MSDrugExtremeDef == null ? "None" : MSDrugExtremeDef?.label.CapitalizeFirst();

    public static string MSDRHedLabel => MSDRHedDef == null ? "None" : MSDRHedDef?.label.CapitalizeFirst();

    public static string MSDRHedLabel2 => MSDRHedDef2 == null ? "None" : MSDRHedDef2?.label.CapitalizeFirst();

    public static HediffDef MSDRHedDef => MSDRHed == null ? null : GetDRHedDef(MSDRHed);

    public static HediffDef MSDRHedDef2 => MSDRHed2 == null ? null : GetDRHedDef(MSDRHed2);

    public static string MSDRThgLabel => MSDRThgDef == null ? "None" : MSDRThgDef?.label.CapitalizeFirst();

    public static string MSDRThgLabel2 => MSDRThgDef2 == null ? "None" : MSDRThgDef2?.label.CapitalizeFirst();

    public static ThingDef MSDRThgDef => MSDRThg == null ? null : GetDRThgDef(MSDRThg);

    public static ThingDef MSDRThgDef2 => MSDRThg2 == null ? null : GetDRThgDef(MSDRThg2);

    public static void SetCachedValue(string defName, int type)
    {
        switch (type)
        {
            case 1:
                cachedMinorDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                return;
            case 2:
                cachedSeriousDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                return;
            case 3:
                cachedIntenseDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                return;
            case 4:
                cachedExtremeDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                return;
            default:
                return;
        }
    }

    private static ThingDef GetCachedValue(string defName, int type)
    {
        ThingDef returnCache = null;
        if (defName == null)
        {
            return null;
        }

        switch (type)
        {
            case 1:
                cachedMinorDef ??= DefDatabase<ThingDef>.GetNamed(defName, false);

                returnCache = cachedMinorDef;
                break;
            case 2:
                cachedSeriousDef ??= DefDatabase<ThingDef>.GetNamed(defName, false);

                returnCache = cachedSeriousDef;
                break;
            case 3:
                cachedIntenseDef ??= DefDatabase<ThingDef>.GetNamed(defName, false);

                returnCache = cachedIntenseDef;
                break;
            case 4:
                cachedExtremeDef ??= DefDatabase<ThingDef>.GetNamed(defName, false);

                returnCache = cachedExtremeDef;
                break;
        }

        return returnCache;
    }

    private static HediffDef GetDRHedDef(string msdrHed)
    {
        HediffDef def = null;
        if (msdrHed != null)
        {
            def = DefDatabase<HediffDef>.GetNamed(msdrHed, false);
        }

        return def;
    }

    private static ThingDef GetDRThgDef(string msdrThg)
    {
        ThingDef def = null;
        if (msdrThg != null)
        {
            def = DefDatabase<ThingDef>.GetNamed(msdrThg, false);
        }

        return def;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref DoIfPrisoner, "DoIfPrisoner");
        Scribe_Values.Look(ref DoDRIfPrisoner, "DoDRIfPrisoner");
        Scribe_Values.Look(ref UseNonMedical, "UseNonMedical");
        Scribe_Values.Look(ref ShowResearched, "ShowResearched", true);
        Scribe_Values.Look(ref BillsHighPain, "BillsHighPain");
        Scribe_Values.Look(ref DoIfImmune, "DoIfImmune", true);
        Scribe_Values.Look(ref UsePainManagement, "UsePainManagement");
        Scribe_Values.Look(ref PainReliefWaitPeriod, "PainReliefWaitPeriod", 24);
        Scribe_Values.Look(ref ShowReliefMsg, "ShowReliefMsg", true);
        Scribe_Values.Look(ref UseReliefBills, "UseReliefBills");
        Scribe_Values.Look(ref UseDrugResponse, "UseDrugResponse");
        Scribe_Values.Look(ref DRWaitPeriod, "DRWaitPeriod", 24);
        Scribe_Values.Look(ref DRWaitPeriod2, "DRWaitPeriod2", 24);
        Scribe_Values.Look(ref ShowResponseMsg, "ShowResponseMsg", true);
        Scribe_Values.Look(ref UseDRBills, "UseDRBills", true);
        Scribe_Values.Look(ref UseDRBills2, "UseDRBills2", true);
        Scribe_Values.Look(ref MSDrugMinor, "MSDrugMinor");
        Scribe_Values.Look(ref MSDrugSerious, "MSDrugSerious");
        Scribe_Values.Look(ref MSDrugIntense, "MSDrugIntense");
        Scribe_Values.Look(ref MSDrugExtreme, "MSDrugExtreme");
        Scribe_Values.Look(ref MSDRHed, "msdrHed");
        Scribe_Values.Look(ref MSDRThg, "MSDRThg");
        Scribe_Collections.Look(ref MSDRValues, "MSDRValues", LookMode.Value);
        Scribe_Values.Look(ref MSDRHed2, "MSDRHed2");
        Scribe_Values.Look(ref MSDRThg2, "MSDRThg2");
        Scribe_Collections.Look(ref MSDRValues2, "MSDRValues2", LookMode.Value);
        if (Scribe.mode == LoadSaveMode.Saving)
        {
            return;
        }

        MSDRValues ??= [];

        MSDRValues2 ??= [];
    }
}