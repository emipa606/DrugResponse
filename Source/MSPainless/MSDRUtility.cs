using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless
{
	// Token: 0x02000007 RID: 7
	public class MSDRUtility
	{
		// Token: 0x06000052 RID: 82 RVA: 0x0000553E File Offset: 0x0000373E
		public static List<string> MaladyMods()
		{
			List<string> list = new List<string>();
			GenCollection.AddDistinct<string>(list, "VN;Vanilla");
			GenCollection.AddDistinct<string>(list, "CA;Common Ailments");
			GenCollection.AddDistinct<string>(list, "DO;Diseases Overhauled");
			return list;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005568 File Offset: 0x00003768
		public static List<string> Maladies()
		{
			List<string> list = new List<string>();
			GenCollection.AddDistinct<string>(list, "VN;Asthma");
			GenCollection.AddDistinct<string>(list, "VN;Burn");
			GenCollection.AddDistinct<string>(list, "VN;Carcinoma");
			GenCollection.AddDistinct<string>(list, "VN;FibrousMechanites");
			GenCollection.AddDistinct<string>(list, "VN;Flu");
			GenCollection.AddDistinct<string>(list, "VN;FoodPoisoning");
			GenCollection.AddDistinct<string>(list, "VN;GutWorms");
			GenCollection.AddDistinct<string>(list, "VN;Malaria");
			GenCollection.AddDistinct<string>(list, "VN;MuscleParasites");
			GenCollection.AddDistinct<string>(list, "VN;Plague");
			GenCollection.AddDistinct<string>(list, "VN;SensoryMechanites");
			GenCollection.AddDistinct<string>(list, "VN;SleepingSickness");
			GenCollection.AddDistinct<string>(list, "VN;WoundInfection");
			GenCollection.AddDistinct<string>(list, "VN;Anesthetic");
			if (ModLister.HasActiveModWithName("Common Ailments"))
			{
				GenCollection.AddDistinct<string>(list, "CA;CA_CommonCold");
				GenCollection.AddDistinct<string>(list, "CA;CA_Conjunctivitis");
				GenCollection.AddDistinct<string>(list, "CA;CA_Earache");
				GenCollection.AddDistinct<string>(list, "CA;CA_Indigestion");
				GenCollection.AddDistinct<string>(list, "CA;CA_Restless");
				GenCollection.AddDistinct<string>(list, "CA;CA_SoreThroat");
				GenCollection.AddDistinct<string>(list, "CA;CA_Hayfever");
				GenCollection.AddDistinct<string>(list, "CA;CA_SkinRash");
				GenCollection.AddDistinct<string>(list, "CA;CA_Sinusitis");
				GenCollection.AddDistinct<string>(list, "CA;CA_Minor_STD");
				GenCollection.AddDistinct<string>(list, "CA;CA_Fatigue");
			}
			if (ModLister.HasActiveModWithName("Diseases Overhauled"))
			{
				GenCollection.AddDistinct<string>(list, "DO;HepatitisK");
				GenCollection.AddDistinct<string>(list, "DO;StomachUlcer");
				GenCollection.AddDistinct<string>(list, "DO;Stupor");
				GenCollection.AddDistinct<string>(list, "DO;Unease");
				GenCollection.AddDistinct<string>(list, "DO;PTSD");
				GenCollection.AddDistinct<string>(list, "DO;SuicidePreparation");
				GenCollection.AddDistinct<string>(list, "DO;Tuberculosis");
				GenCollection.AddDistinct<string>(list, "DO;KindredDickVirus");
				GenCollection.AddDistinct<string>(list, "DO;Sepsis");
				GenCollection.AddDistinct<string>(list, "DO;Toothache");
				GenCollection.AddDistinct<string>(list, "DO;VoightBernsteinDisease");
				GenCollection.AddDistinct<string>(list, "DO;HansenKampffDisease");
				GenCollection.AddDistinct<string>(list, "DO;Necrosis");
				GenCollection.AddDistinct<string>(list, "DO;LymphaticMechanites");
				GenCollection.AddDistinct<string>(list, "DO;Psoriasis");
				GenCollection.AddDistinct<string>(list, "DO;CommonCold");
				GenCollection.AddDistinct<string>(list, "DO;NewReschianFever");
				GenCollection.AddDistinct<string>(list, "DO;BloodCancer");
			}
			List<ChemicalDef> Chemicals = DefDatabase<ChemicalDef>.AllDefsListForReading;
			if (Chemicals != null && Chemicals.Count > 0)
			{
				foreach (ChemicalDef Chemical in Chemicals)
				{
					if (!(Chemical.defName == "MSMental"))
					{
						HediffDef addiction = Chemical?.addictionHediff;
						if (addiction != null)
						{
							GenCollection.AddDistinct<string>(list, "VA;" + addiction.defName);
						}
					}
				}
			}
			list = (from s in list
			orderby s
			select s).ToList<string>();
			return list;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005824 File Offset: 0x00003A24
		public static bool MaladyUsed(string defname)
		{
			if (MSDRUtility.Maladies().Count > 0)
			{
				foreach (string malstring in MSDRUtility.Maladies())
				{
					char[] divider = new char[]
					{
						';'
					};
					if (malstring.Split(divider)[1] == defname)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000058A0 File Offset: 0x00003AA0
		public static List<ThingDef> MaladyDrugs()
		{
			List<RecipeDef> recipelist = DefDatabase<RecipeDef>.AllDefsListForReading;
			if (Current.ProgramState != ProgramState.Playing && DRSettings.ShowResearched)
			{
				Messages.Message(Translator.Translate("MSPainless.Warning"), LookTargets.Invalid, MessageTypeDefOf.CautionInput, false);
			}
			List<ThingDef> list = new List<ThingDef>();
			List<ThingDef> chkList = DefDatabase<ThingDef>.AllDefsListForReading;
			if (chkList.Count > 0)
			{
				foreach (ThingDef chkDef in chkList)
				{
					if (chkDef.IsIngestible && chkDef != null)
					{
						IngestibleProperties ingestible = chkDef.ingestible;
						DrugCategory? drugCategory = (ingestible != null) ? new DrugCategory?(ingestible.drugCategory) : null;
						DrugCategory drugCategory2 = DrugCategory.Medical;
						if (drugCategory.GetValueOrDefault() == drugCategory2 & drugCategory != null)
						{
							if (DRSettings.ShowResearched)
							{
								if (MSPainDrug.IsResearchCompleted(chkDef, recipelist))
								{
									GenCollection.AddDistinct<ThingDef>(list, chkDef);
								}
							}
							else
							{
								GenCollection.AddDistinct<ThingDef>(list, chkDef);
							}
						}
					}
				}
			}
			return (from td in list
			orderby td.label
			select td).ToList<ThingDef>();
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000059CC File Offset: 0x00003BCC
		public static void SetDRValues(string m, string d, int t, bool b, int numof, List<string> master, out List<string> newMaster)
		{
			newMaster = new List<string>();
			string newValue = MSDRUtility.ConvertToDRValue(t, m, d, b, numof);
			if (master != null)
			{
				bool beenSet = false;
				if (master.Count > 0)
				{
					foreach (string value in master)
					{
						string a = MSDRUtility.HValuePart(value);
						int num = MSDRUtility.NumValuePart(value);
						if (a != m)
						{
							GenCollection.AddDistinct<string>(newMaster, value);
						}
						else if (num != numof)
						{
							GenCollection.AddDistinct<string>(newMaster, value);
						}
						else
						{
							GenCollection.AddDistinct<string>(newMaster, newValue);
							beenSet = true;
						}
					}
				}
				if (!beenSet)
				{
					GenCollection.AddDistinct<string>(newMaster, newValue);
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00005A84 File Offset: 0x00003C84
		public static int GetValueDRnumof(string m, List<string> master)
		{
			if (m != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					if (MSDRUtility.HValuePart(value) == m)
					{
						return MSDRUtility.NumValuePart(value);
					}
				}
				return 1;
			}
			return 1;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public static int GetValueDRTime(string m, List<string> master, int num)
		{
			if (m != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					string Hdef = MSDRUtility.HValuePart(value);
					int numof = MSDRUtility.NumValuePart(value);
					if (Hdef == m && numof == num)
					{
						return MSDRUtility.TValuePart(value);
					}
				}
				return 24;
			}
			return 24;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005B74 File Offset: 0x00003D74
		public static string GetValueDRDrug(string m, List<string> master, int num)
		{
			if (m != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					string Hdef = MSDRUtility.HValuePart(value);
					int numof = MSDRUtility.NumValuePart(value);
					if (Hdef == m && numof == num)
					{
						return MSDRUtility.DValuePart(value);
					}
				}
			}
			return null;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005BF4 File Offset: 0x00003DF4
		public static bool GetValueDRBills(string m, List<string> master)
		{
			if (m != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					if (MSDRUtility.HValuePart(value) == m)
					{
						return MSDRUtility.BValuePart(value);
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005C64 File Offset: 0x00003E64
		public static void ClearDRValues(string m, bool doAll, List<string> master, out List<string> newMaster)
		{
			newMaster = new List<string>();
			if (!doAll && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					if (MSDRUtility.HValuePart(value) != m)
					{
						GenCollection.AddDistinct<string>(newMaster, value);
					}
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00005CD8 File Offset: 0x00003ED8
		public static int NumValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			string[] segments = value.Split(divider);
			try
			{
				if (segments.Count<string>() > 4)
				{
					return int.Parse(segments[4]);
				}
				return 1;
			}
			catch (FormatException)
			{
				Log.Message("Unable to parse Seg0: '" + segments[4] + "'", false);
			}
			return 1;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005D40 File Offset: 0x00003F40
		public static int TValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			string[] segments = value.Split(divider);
			try
			{
				return int.Parse(segments[0]);
			}
			catch (FormatException)
			{
				Log.Message("Unable to parse Seg0: '" + segments[0] + "'", false);
			}
			return 24;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005D9C File Offset: 0x00003F9C
		public static string HValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			return value.Split(divider)[1];
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public static string DValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			return value.Split(divider)[2];
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005DE4 File Offset: 0x00003FE4
		public static bool BValuePart(string value)
		{
			char[] divider = new char[]
			{
				';'
			};
			return value.Split(divider)[3] == "1";
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005E18 File Offset: 0x00004018
		public static string ConvertToDRValue(int t, string m, string d, bool b, int num)
		{
			string boolbit = "1";
			if (!b)
			{
				boolbit = "0";
			}
			return string.Concat(new string[]
			{
				t.ToString(),
				";",
				m,
				";",
				d,
				";",
				boolbit
			}).Trim() + ";" + num.ToString();
		}
	}
}
