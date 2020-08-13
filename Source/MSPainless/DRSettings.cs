using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace MSPainless
{
	// Token: 0x02000003 RID: 3
	public class DRSettings : WorldComponent
	{
		// Token: 0x06000028 RID: 40 RVA: 0x000044AF File Offset: 0x000026AF
		public DRSettings(World world) : base(world)
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000044B8 File Offset: 0x000026B8
		public static ThingDef MSDrugMinorDef
		{
			get
			{
				return DRSettings.GetCachedValue(DRSettings.MSDrugMinor, 1);
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000044C5 File Offset: 0x000026C5
		public static ThingDef MSDrugSeriousDef
		{
			get
			{
				return DRSettings.GetCachedValue(DRSettings.MSDrugSerious, 2);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000044D2 File Offset: 0x000026D2
		public static ThingDef MSDrugIntenseDef
		{
			get
			{
				return DRSettings.GetCachedValue(DRSettings.MSDrugIntense, 3);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000044DF File Offset: 0x000026DF
		public static ThingDef MSDrugExtremeDef
		{
			get
			{
				return DRSettings.GetCachedValue(DRSettings.MSDrugExtreme, 4);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000044EC File Offset: 0x000026EC
		public static string MSDrugMinorLabel
		{
			get
			{
				if (DRSettings.MSDrugMinorDef == null)
				{
					return "None";
				}
				ThingDef msdrugMinorDef = DRSettings.MSDrugMinorDef;
				if (msdrugMinorDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrugMinorDef.label);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00004510 File Offset: 0x00002710
		public static string MSDrugSeriousLabel
		{
			get
			{
				if (DRSettings.MSDrugSeriousDef == null)
				{
					return "None";
				}
				ThingDef msdrugSeriousDef = DRSettings.MSDrugSeriousDef;
				if (msdrugSeriousDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrugSeriousDef.label);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00004534 File Offset: 0x00002734
		public static string MSDrugIntenseLabel
		{
			get
			{
				if (DRSettings.MSDrugIntenseDef == null)
				{
					return "None";
				}
				ThingDef msdrugIntenseDef = DRSettings.MSDrugIntenseDef;
				if (msdrugIntenseDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrugIntenseDef.label);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00004558 File Offset: 0x00002758
		public static string MSDrugExtremeLabel
		{
			get
			{
				if (DRSettings.MSDrugExtremeDef == null)
				{
					return "None";
				}
				ThingDef msdrugExtremeDef = DRSettings.MSDrugExtremeDef;
				if (msdrugExtremeDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrugExtremeDef.label);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000457C File Offset: 0x0000277C
		public static void SetCachedValue(string defName, int type)
		{
			switch (type)
			{
			case 1:
				DRSettings.cachedMinorDef = DefDatabase<ThingDef>.GetNamed(defName, false);
				return;
			case 2:
				DRSettings.cachedSeriousDef = DefDatabase<ThingDef>.GetNamed(defName, false);
				return;
			case 3:
				DRSettings.cachedIntenseDef = DefDatabase<ThingDef>.GetNamed(defName, false);
				return;
			case 4:
				DRSettings.cachedExtremeDef = DefDatabase<ThingDef>.GetNamed(defName, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000045D8 File Offset: 0x000027D8
		public static ThingDef GetCachedValue(string defName, int type)
		{
			ThingDef returnCache = null;
			if (defName != null)
			{
				switch (type)
				{
				case 1:
					if (DRSettings.cachedMinorDef == null)
					{
						DRSettings.cachedMinorDef = DefDatabase<ThingDef>.GetNamed(defName, false);
					}
					returnCache = DRSettings.cachedMinorDef;
					break;
				case 2:
					if (DRSettings.cachedSeriousDef == null)
					{
						DRSettings.cachedSeriousDef = DefDatabase<ThingDef>.GetNamed(defName, false);
					}
					returnCache = DRSettings.cachedSeriousDef;
					break;
				case 3:
					if (DRSettings.cachedIntenseDef == null)
					{
						DRSettings.cachedIntenseDef = DefDatabase<ThingDef>.GetNamed(defName, false);
					}
					returnCache = DRSettings.cachedIntenseDef;
					break;
				case 4:
					if (DRSettings.cachedExtremeDef == null)
					{
						DRSettings.cachedExtremeDef = DefDatabase<ThingDef>.GetNamed(defName, false);
					}
					returnCache = DRSettings.cachedExtremeDef;
					break;
				}
			}
			return returnCache;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00004672 File Offset: 0x00002872
		public static string MSDRHedLabel
		{
			get
			{
				if (DRSettings.MSDRHedDef == null)
				{
					return "None";
				}
				HediffDef msdrhedDef = DRSettings.MSDRHedDef;
				if (msdrhedDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrhedDef.label);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00004696 File Offset: 0x00002896
		public static string MSDRHedLabel2
		{
			get
			{
				if (DRSettings.MSDRHedDef2 == null)
				{
					return "None";
				}
				HediffDef msdrhedDef = DRSettings.MSDRHedDef2;
				if (msdrhedDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrhedDef.label);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000046BA File Offset: 0x000028BA
		public static HediffDef MSDRHedDef
		{
			get
			{
				if (DRSettings.MSDRHed == null)
				{
					return null;
				}
				return DRSettings.GetDRHedDef(DRSettings.MSDRHed);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000046CF File Offset: 0x000028CF
		public static HediffDef MSDRHedDef2
		{
			get
			{
				if (DRSettings.MSDRHed2 == null)
				{
					return null;
				}
				return DRSettings.GetDRHedDef(DRSettings.MSDRHed2);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000046E4 File Offset: 0x000028E4
		public static HediffDef GetDRHedDef(string MSDRHed)
		{
			HediffDef def = null;
			if (MSDRHed != null)
			{
				def = DefDatabase<HediffDef>.GetNamed(MSDRHed, false);
			}
			return def;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000046FF File Offset: 0x000028FF
		public static string MSDRThgLabel
		{
			get
			{
				if (DRSettings.MSDRThgDef == null)
				{
					return "None";
				}
				ThingDef msdrthgDef = DRSettings.MSDRThgDef;
				if (msdrthgDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrthgDef.label);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00004723 File Offset: 0x00002923
		public static string MSDRThgLabel2
		{
			get
			{
				if (DRSettings.MSDRThgDef2 == null)
				{
					return "None";
				}
				ThingDef msdrthgDef = DRSettings.MSDRThgDef2;
				if (msdrthgDef == null)
				{
					return null;
				}
				return GenText.CapitalizeFirst(msdrthgDef.label);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00004747 File Offset: 0x00002947
		public static ThingDef MSDRThgDef
		{
			get
			{
				if (DRSettings.MSDRThg == null)
				{
					return null;
				}
				return DRSettings.GetDRThgDef(DRSettings.MSDRThg);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0000475C File Offset: 0x0000295C
		public static ThingDef MSDRThgDef2
		{
			get
			{
				if (DRSettings.MSDRThg2 == null)
				{
					return null;
				}
				return DRSettings.GetDRThgDef(DRSettings.MSDRThg2);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004774 File Offset: 0x00002974
		public static ThingDef GetDRThgDef(string MSDRThg)
		{
			ThingDef def = null;
			if (MSDRThg != null)
			{
				def = DefDatabase<ThingDef>.GetNamed(MSDRThg, false);
			}
			return def;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004790 File Offset: 0x00002990
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref DRSettings.DoIfPrisoner, "DoIfPrisoner", false, false);
			Scribe_Values.Look<bool>(ref DRSettings.DoDRIfPrisoner, "DoDRIfPrisoner", false, false);
			Scribe_Values.Look<bool>(ref DRSettings.UseNonMedical, "UseNonMedical", false, false);
			Scribe_Values.Look<bool>(ref DRSettings.ShowResearched, "ShowResearched", true, false);
			Scribe_Values.Look<bool>(ref DRSettings.BillsHighPain, "BillsHighPain", false, false);
			Scribe_Values.Look<bool>(ref DRSettings.DoIfImmune, "DoIfImmune", true, false);
			Scribe_Values.Look<bool>(ref DRSettings.UsePainManagement, "UsePainManagement", false, false);
			Scribe_Values.Look<int>(ref DRSettings.PainReliefWaitPeriod, "PainReliefWaitPeriod", 24, false);
			Scribe_Values.Look<bool>(ref DRSettings.ShowReliefMsg, "ShowReliefMsg", true, false);
			Scribe_Values.Look<bool>(ref DRSettings.UseReliefBills, "UseReliefBills", false, false);
			Scribe_Values.Look<bool>(ref DRSettings.UseDrugResponse, "UseDrugResponse", false, false);
			Scribe_Values.Look<int>(ref DRSettings.DRWaitPeriod, "DRWaitPeriod", 24, false);
			Scribe_Values.Look<int>(ref DRSettings.DRWaitPeriod2, "DRWaitPeriod2", 24, false);
			Scribe_Values.Look<bool>(ref DRSettings.ShowResponseMsg, "ShowResponseMsg", true, false);
			Scribe_Values.Look<bool>(ref DRSettings.UseDRBills, "UseDRBills", true, false);
			Scribe_Values.Look<bool>(ref DRSettings.UseDRBills2, "UseDRBills2", true, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDrugMinor, "MSDrugMinor", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDrugSerious, "MSDrugSerious", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDrugIntense, "MSDrugIntense", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDrugExtreme, "MSDrugExtreme", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDRHed, "MSDRHed", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDRThg, "MSDRThg", null, false);
			Scribe_Collections.Look<string>(ref DRSettings.MSDRValues, "MSDRValues", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref DRSettings.MSDRHed2, "MSDRHed2", null, false);
			Scribe_Values.Look<string>(ref DRSettings.MSDRThg2, "MSDRThg2", null, false);
			Scribe_Collections.Look<string>(ref DRSettings.MSDRValues2, "MSDRValues2", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				if (DRSettings.MSDRValues == null)
				{
					DRSettings.MSDRValues = new List<string>();
				}
				if (DRSettings.MSDRValues2 == null)
				{
					DRSettings.MSDRValues2 = new List<string>();
				}
			}
		}

		// Token: 0x04000001 RID: 1
		public static bool DoIfPrisoner = false;

		// Token: 0x04000002 RID: 2
		public static bool DoDRIfPrisoner = false;

		// Token: 0x04000003 RID: 3
		public static bool UseNonMedical = false;

		// Token: 0x04000004 RID: 4
		public static bool ShowResearched = true;

		// Token: 0x04000005 RID: 5
		public static bool BillsHighPain = false;

		// Token: 0x04000006 RID: 6
		public static bool DoIfImmune = true;

		// Token: 0x04000007 RID: 7
		public static bool UsePainManagement = false;

		// Token: 0x04000008 RID: 8
		public static int PainReliefWaitPeriod = 24;

		// Token: 0x04000009 RID: 9
		public static bool ShowReliefMsg = true;

		// Token: 0x0400000A RID: 10
		public static bool UseReliefBills = false;

		// Token: 0x0400000B RID: 11
		public static bool UseDrugResponse = false;

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
		public static ThingDef cachedMinorDef = null;

		// Token: 0x04000012 RID: 18
		public static ThingDef cachedSeriousDef = null;

		// Token: 0x04000013 RID: 19
		public static ThingDef cachedIntenseDef = null;

		// Token: 0x04000014 RID: 20
		public static ThingDef cachedExtremeDef = null;

		// Token: 0x04000015 RID: 21
		public static string MSDrugMinor = null;

		// Token: 0x04000016 RID: 22
		public static string MSDrugSerious = null;

		// Token: 0x04000017 RID: 23
		public static string MSDrugIntense = null;

		// Token: 0x04000018 RID: 24
		public static string MSDrugExtreme = null;

		// Token: 0x04000019 RID: 25
		public static string MSDRHed = null;

		// Token: 0x0400001A RID: 26
		public static string MSDRHed2 = null;

		// Token: 0x0400001B RID: 27
		public static string MSDRThg = null;

		// Token: 0x0400001C RID: 28
		public static string MSDRThg2 = null;

		// Token: 0x0400001D RID: 29
		public static List<string> MSDRValues = new List<string>();

		// Token: 0x0400001E RID: 30
		public static List<string> MSDRValues2 = new List<string>();

		// Token: 0x02000018 RID: 24
		[StaticConstructorOnStartup]
		private static class CheckDrugDefinitions
		{
			// Token: 0x060000D0 RID: 208 RVA: 0x0000796C File Offset: 0x00005B6C
			static CheckDrugDefinitions()
			{
				DRSettings.CheckDrugDefinitions.CheckPainDrugDefinitions();
				DRSettings.CheckDrugDefinitions.CheckDRDrugDefinitions1();
				DRSettings.CheckDrugDefinitions.CheckDRDrugDefinitions2();
			}

			// Token: 0x060000D1 RID: 209 RVA: 0x00007980 File Offset: 0x00005B80
			private static void CheckPainDrugDefinitions()
			{
				if (DRSettings.MSDrugMinor != null && DefDatabase<ThingDef>.GetNamed(DRSettings.MSDrugMinor, false) == null)
				{
					DRSettings.MSDrugMinor = null;
				}
				if (DRSettings.MSDrugSerious != null && DefDatabase<ThingDef>.GetNamed(DRSettings.MSDrugSerious, false) == null)
				{
					DRSettings.MSDrugSerious = null;
				}
				if (DRSettings.MSDrugIntense != null && DefDatabase<ThingDef>.GetNamed(DRSettings.MSDrugIntense, false) == null)
				{
					DRSettings.MSDrugIntense = null;
				}
				if (DRSettings.MSDrugExtreme != null && DefDatabase<ThingDef>.GetNamed(DRSettings.MSDrugExtreme, false) == null)
				{
					DRSettings.MSDrugExtreme = null;
				}
			}

			// Token: 0x060000D2 RID: 210 RVA: 0x000079F8 File Offset: 0x00005BF8
			private static void CheckDRDrugDefinitions1()
			{
				List<string> newMSDRValues = new List<string>();
				if (DRSettings.MSDRValues != null && DRSettings.MSDRValues.Count > 0)
				{
					foreach (string value in DRSettings.MSDRValues)
					{
						string mal = MSDRUtility.HValuePart(value);
						if (MSDRUtility.MaladyUsed(mal))
						{
							int end = 1;
							if (value.EndsWith("2"))
							{
								end = 2;
							}
							string drug = MSDRUtility.DValuePart(value);
							if (DefDatabase<ThingDef>.GetNamed(drug, false) != null)
							{
								int t = MSDRUtility.TValuePart(value);
								bool b = MSDRUtility.BValuePart(value);
								string oldValue = MSDRUtility.ConvertToDRValue(t, mal, drug, b, end);
								GenCollection.AddDistinct<string>(newMSDRValues, oldValue);
							}
							else
							{
								string newValue = MSDRUtility.ConvertToDRValue(24, mal, null, true, end);
								GenCollection.AddDistinct<string>(newMSDRValues, newValue);
							}
						}
					}
				}
				DRSettings.MSDRValues = newMSDRValues;
			}

			// Token: 0x060000D3 RID: 211 RVA: 0x00007AE0 File Offset: 0x00005CE0
			private static void CheckDRDrugDefinitions2()
			{
				List<string> newMSDRValues = new List<string>();
				if (DRSettings.MSDRValues2 != null && DRSettings.MSDRValues2.Count > 0)
				{
					foreach (string value in DRSettings.MSDRValues2)
					{
						string mal = MSDRUtility.HValuePart(value);
						if (MSDRUtility.MaladyUsed(mal))
						{
							int end = 1;
							if (value.EndsWith("2"))
							{
								end = 2;
							}
							string drug = MSDRUtility.DValuePart(value);
							if (DefDatabase<ThingDef>.GetNamed(drug, false) != null)
							{
								int t = MSDRUtility.TValuePart(value);
								bool b = MSDRUtility.BValuePart(value);
								string oldValue = MSDRUtility.ConvertToDRValue(t, mal, drug, b, end);
								GenCollection.AddDistinct<string>(newMSDRValues, oldValue);
							}
							else
							{
								string newValue = MSDRUtility.ConvertToDRValue(24, mal, null, true, end);
								GenCollection.AddDistinct<string>(newMSDRValues, newValue);
							}
						}
					}
				}
				DRSettings.MSDRValues2 = newMSDRValues;
			}
		}
	}
}
