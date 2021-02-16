using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace MSPainless
{
    // Token: 0x02000003 RID: 3
    public class DRSettings : WorldComponent
    {
        // Token: 0x04000001 RID: 1
        public static bool DoIfPrisoner;

        // Token: 0x04000002 RID: 2
        public static bool DoDRIfPrisoner;

        // Token: 0x04000003 RID: 3
        public static bool UseNonMedical;

        // Token: 0x04000004 RID: 4
        public static bool ShowResearched = true;

        // Token: 0x04000005 RID: 5
        public static bool BillsHighPain;

        // Token: 0x04000006 RID: 6
        public static bool DoIfImmune = true;

        // Token: 0x04000007 RID: 7
        public static bool UsePainManagement;

        // Token: 0x04000008 RID: 8
        public static int PainReliefWaitPeriod = 24;

        // Token: 0x04000009 RID: 9
        public static bool ShowReliefMsg = true;

        // Token: 0x0400000A RID: 10
        public static bool UseReliefBills;

        // Token: 0x0400000B RID: 11
        public static bool UseDrugResponse;

        // Token: 0x0400000C RID: 12
        public static int DRWaitPeriod = 24;

        // Token: 0x0400000D RID: 13
        public static int DRWaitPeriod2 = 24;

        // Token: 0x0400000E RID: 14
        public static bool ShowResponseMsg = true;

        // Token: 0x0400000F RID: 15
        public static bool UseDRBills = true;

        // Token: 0x04000010 RID: 16
        public static bool UseDRBills2 = true;

        // Token: 0x04000011 RID: 17
        private static ThingDef cachedMinorDef;

        // Token: 0x04000012 RID: 18
        private static ThingDef cachedSeriousDef;

        // Token: 0x04000013 RID: 19
        private static ThingDef cachedIntenseDef;

        // Token: 0x04000014 RID: 20
        private static ThingDef cachedExtremeDef;

        // Token: 0x04000015 RID: 21
        public static string MSDrugMinor;

        // Token: 0x04000016 RID: 22
        public static string MSDrugSerious;

        // Token: 0x04000017 RID: 23
        public static string MSDrugIntense;

        // Token: 0x04000018 RID: 24
        public static string MSDrugExtreme;

        // Token: 0x04000019 RID: 25
        public static string MSDRHed;

        // Token: 0x0400001A RID: 26
        public static string MSDRHed2;

        // Token: 0x0400001B RID: 27
        public static string MSDRThg;

        // Token: 0x0400001C RID: 28
        public static string MSDRThg2;

        // Token: 0x0400001D RID: 29
        public static List<string> MSDRValues = new List<string>();

        // Token: 0x0400001E RID: 30
        public static List<string> MSDRValues2 = new List<string>();

        // Token: 0x06000028 RID: 40 RVA: 0x000044AF File Offset: 0x000026AF
        public DRSettings(World world) : base(world)
        {
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000029 RID: 41 RVA: 0x000044B8 File Offset: 0x000026B8
        public static ThingDef MSDrugMinorDef => GetCachedValue(MSDrugMinor, 1);

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600002A RID: 42 RVA: 0x000044C5 File Offset: 0x000026C5
        public static ThingDef MSDrugSeriousDef => GetCachedValue(MSDrugSerious, 2);

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600002B RID: 43 RVA: 0x000044D2 File Offset: 0x000026D2
        public static ThingDef MSDrugIntenseDef => GetCachedValue(MSDrugIntense, 3);

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600002C RID: 44 RVA: 0x000044DF File Offset: 0x000026DF
        public static ThingDef MSDrugExtremeDef => GetCachedValue(MSDrugExtreme, 4);

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600002D RID: 45 RVA: 0x000044EC File Offset: 0x000026EC
        public static string MSDrugMinorLabel
        {
            get
            {
                if (MSDrugMinorDef == null)
                {
                    return "None";
                }

                var msdrugMinorDef = MSDrugMinorDef;
                return msdrugMinorDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600002E RID: 46 RVA: 0x00004510 File Offset: 0x00002710
        public static string MSDrugSeriousLabel
        {
            get
            {
                if (MSDrugSeriousDef == null)
                {
                    return "None";
                }

                var msdrugSeriousDef = MSDrugSeriousDef;
                return msdrugSeriousDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600002F RID: 47 RVA: 0x00004534 File Offset: 0x00002734
        public static string MSDrugIntenseLabel
        {
            get
            {
                if (MSDrugIntenseDef == null)
                {
                    return "None";
                }

                var msdrugIntenseDef = MSDrugIntenseDef;
                return msdrugIntenseDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000030 RID: 48 RVA: 0x00004558 File Offset: 0x00002758
        public static string MSDrugExtremeLabel
        {
            get
            {
                if (MSDrugExtremeDef == null)
                {
                    return "None";
                }

                var msdrugExtremeDef = MSDrugExtremeDef;
                return msdrugExtremeDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00004672 File Offset: 0x00002872
        public static string MSDRHedLabel
        {
            get
            {
                if (MSDRHedDef == null)
                {
                    return "None";
                }

                var msdrhedDef = MSDRHedDef;
                return msdrhedDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000034 RID: 52 RVA: 0x00004696 File Offset: 0x00002896
        public static string MSDRHedLabel2
        {
            get
            {
                if (MSDRHedDef2 == null)
                {
                    return "None";
                }

                var msdrhedDef = MSDRHedDef2;
                return msdrhedDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000035 RID: 53 RVA: 0x000046BA File Offset: 0x000028BA
        public static HediffDef MSDRHedDef
        {
            get
            {
                if (MSDRHed == null)
                {
                    return null;
                }

                return GetDRHedDef(MSDRHed);
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000036 RID: 54 RVA: 0x000046CF File Offset: 0x000028CF
        public static HediffDef MSDRHedDef2
        {
            get
            {
                if (MSDRHed2 == null)
                {
                    return null;
                }

                return GetDRHedDef(MSDRHed2);
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000038 RID: 56 RVA: 0x000046FF File Offset: 0x000028FF
        public static string MSDRThgLabel
        {
            get
            {
                if (MSDRThgDef == null)
                {
                    return "None";
                }

                var msdrthgDef = MSDRThgDef;
                return msdrthgDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000039 RID: 57 RVA: 0x00004723 File Offset: 0x00002923
        public static string MSDRThgLabel2
        {
            get
            {
                if (MSDRThgDef2 == null)
                {
                    return "None";
                }

                var msdrthgDef = MSDRThgDef2;
                return msdrthgDef?.label.CapitalizeFirst();
            }
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x0600003A RID: 58 RVA: 0x00004747 File Offset: 0x00002947
        public static ThingDef MSDRThgDef
        {
            get
            {
                if (MSDRThg == null)
                {
                    return null;
                }

                return GetDRThgDef(MSDRThg);
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x0600003B RID: 59 RVA: 0x0000475C File Offset: 0x0000295C
        public static ThingDef MSDRThgDef2
        {
            get
            {
                if (MSDRThg2 == null)
                {
                    return null;
                }

                return GetDRThgDef(MSDRThg2);
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x0000457C File Offset: 0x0000277C
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

        // Token: 0x06000032 RID: 50 RVA: 0x000045D8 File Offset: 0x000027D8
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
                    if (cachedMinorDef == null)
                    {
                        cachedMinorDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                    }

                    returnCache = cachedMinorDef;
                    break;
                case 2:
                    if (cachedSeriousDef == null)
                    {
                        cachedSeriousDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                    }

                    returnCache = cachedSeriousDef;
                    break;
                case 3:
                    if (cachedIntenseDef == null)
                    {
                        cachedIntenseDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                    }

                    returnCache = cachedIntenseDef;
                    break;
                case 4:
                    if (cachedExtremeDef == null)
                    {
                        cachedExtremeDef = DefDatabase<ThingDef>.GetNamed(defName, false);
                    }

                    returnCache = cachedExtremeDef;
                    break;
            }

            return returnCache;
        }

        // Token: 0x06000037 RID: 55 RVA: 0x000046E4 File Offset: 0x000028E4
        private static HediffDef GetDRHedDef(string msdrHed)
        {
            HediffDef def = null;
            if (msdrHed != null)
            {
                def = DefDatabase<HediffDef>.GetNamed(msdrHed, false);
            }

            return def;
        }

        // Token: 0x0600003C RID: 60 RVA: 0x00004774 File Offset: 0x00002974
        private static ThingDef GetDRThgDef(string msdrThg)
        {
            ThingDef def = null;
            if (msdrThg != null)
            {
                def = DefDatabase<ThingDef>.GetNamed(msdrThg, false);
            }

            return def;
        }

        // Token: 0x0600003D RID: 61 RVA: 0x00004790 File Offset: 0x00002990
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
            Scribe_Collections.Look(ref MSDRValues, "MSDRValues", LookMode.Value, Array.Empty<object>());
            Scribe_Values.Look(ref MSDRHed2, "MSDRHed2");
            Scribe_Values.Look(ref MSDRThg2, "MSDRThg2");
            Scribe_Collections.Look(ref MSDRValues2, "MSDRValues2", LookMode.Value, Array.Empty<object>());
            if (Scribe.mode == LoadSaveMode.Saving)
            {
                return;
            }

            if (MSDRValues == null)
            {
                MSDRValues = new List<string>();
            }

            if (MSDRValues2 == null)
            {
                MSDRValues2 = new List<string>();
            }
        }

        // Token: 0x02000018 RID: 24
        [StaticConstructorOnStartup]
        private static class CheckDrugDefinitions
        {
            // Token: 0x060000D0 RID: 208 RVA: 0x0000796C File Offset: 0x00005B6C
            static CheckDrugDefinitions()
            {
                CheckPainDrugDefinitions();
                CheckDRDrugDefinitions1();
                CheckDRDrugDefinitions2();
            }

            // Token: 0x060000D1 RID: 209 RVA: 0x00007980 File Offset: 0x00005B80
            private static void CheckPainDrugDefinitions()
            {
                if (MSDrugMinor != null && DefDatabase<ThingDef>.GetNamed(MSDrugMinor, false) == null)
                {
                    MSDrugMinor = null;
                }

                if (MSDrugSerious != null && DefDatabase<ThingDef>.GetNamed(MSDrugSerious, false) == null)
                {
                    MSDrugSerious = null;
                }

                if (MSDrugIntense != null && DefDatabase<ThingDef>.GetNamed(MSDrugIntense, false) == null)
                {
                    MSDrugIntense = null;
                }

                if (MSDrugExtreme != null && DefDatabase<ThingDef>.GetNamed(MSDrugExtreme, false) == null)
                {
                    MSDrugExtreme = null;
                }
            }

            // Token: 0x060000D2 RID: 210 RVA: 0x000079F8 File Offset: 0x00005BF8
            private static void CheckDRDrugDefinitions1()
            {
                var newMSDRValues = new List<string>();
                if (MSDRValues != null && MSDRValues.Count > 0)
                {
                    foreach (var value in MSDRValues)
                    {
                        var mal = MSDRUtility.HValuePart(value);
                        if (!MSDRUtility.MaladyUsed(mal))
                        {
                            continue;
                        }

                        var end = 1;
                        if (value.EndsWith("2"))
                        {
                            end = 2;
                        }

                        var drug = MSDRUtility.DValuePart(value);
                        if (DefDatabase<ThingDef>.GetNamed(drug, false) != null)
                        {
                            var t = MSDRUtility.TValuePart(value);
                            var b = MSDRUtility.BValuePart(value);
                            var oldValue = MSDRUtility.ConvertToDRValue(t, mal, drug, b, end);
                            newMSDRValues.AddDistinct(oldValue);
                        }
                        else
                        {
                            var newValue = MSDRUtility.ConvertToDRValue(24, mal, null, true, end);
                            newMSDRValues.AddDistinct(newValue);
                        }
                    }
                }

                MSDRValues = newMSDRValues;
            }

            // Token: 0x060000D3 RID: 211 RVA: 0x00007AE0 File Offset: 0x00005CE0
            private static void CheckDRDrugDefinitions2()
            {
                var newMSDRValues = new List<string>();
                if (MSDRValues2 != null && MSDRValues2.Count > 0)
                {
                    foreach (var value in MSDRValues2)
                    {
                        var mal = MSDRUtility.HValuePart(value);
                        if (!MSDRUtility.MaladyUsed(mal))
                        {
                            continue;
                        }

                        var end = 1;
                        if (value.EndsWith("2"))
                        {
                            end = 2;
                        }

                        var drug = MSDRUtility.DValuePart(value);
                        if (DefDatabase<ThingDef>.GetNamed(drug, false) != null)
                        {
                            var t = MSDRUtility.TValuePart(value);
                            var b = MSDRUtility.BValuePart(value);
                            var oldValue = MSDRUtility.ConvertToDRValue(t, mal, drug, b, end);
                            newMSDRValues.AddDistinct(oldValue);
                        }
                        else
                        {
                            var newValue = MSDRUtility.ConvertToDRValue(24, mal, null, true, end);
                            newMSDRValues.AddDistinct(newValue);
                        }
                    }
                }

                MSDRValues2 = newMSDRValues;
            }
        }
    }
}