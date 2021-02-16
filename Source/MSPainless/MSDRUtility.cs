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
            var list = new List<string>();
            list.AddDistinct("VN;Vanilla");
            list.AddDistinct("CA;Common Ailments");
            list.AddDistinct("CC;Common Ailments (Continued)");
            list.AddDistinct("DO;Diseases Overhauled");
            return list;
        }

        // Token: 0x06000053 RID: 83 RVA: 0x00005568 File Offset: 0x00003768
        public static List<string> Maladies()
        {
            var list = new List<string>();
            list.AddDistinct("VN;Asthma");
            list.AddDistinct("VN;Burn");
            list.AddDistinct("VN;Carcinoma");
            list.AddDistinct("VN;FibrousMechanites");
            list.AddDistinct("VN;Flu");
            list.AddDistinct("VN;FoodPoisoning");
            list.AddDistinct("VN;GutWorms");
            list.AddDistinct("VN;Malaria");
            list.AddDistinct("VN;MuscleParasites");
            list.AddDistinct("VN;Plague");
            list.AddDistinct("VN;SensoryMechanites");
            list.AddDistinct("VN;SleepingSickness");
            list.AddDistinct("VN;WoundInfection");
            list.AddDistinct("VN;Anesthetic");
            if (ModLister.HasActiveModWithName("Common Ailments") ||
                ModLister.HasActiveModWithName("Common Ailments (Continued)"))
            {
                list.AddDistinct("CA;CA_CommonCold");
                list.AddDistinct("CA;CA_Conjunctivitis");
                list.AddDistinct("CA;CA_Earache");
                list.AddDistinct("CA;CA_Indigestion");
                list.AddDistinct("CA;CA_Restless");
                list.AddDistinct("CA;CA_SoreThroat");
                list.AddDistinct("CA;CA_Hayfever");
                list.AddDistinct("CA;CA_SkinRash");
                list.AddDistinct("CA;CA_Sinusitis");
                list.AddDistinct("CA;CA_Minor_STD");
                list.AddDistinct("CA;CA_Fatigue");
                list.AddDistinct("CA;CA_Headache");
                list.AddDistinct("CA;CA_Migraine");
                list.AddDistinct("CA;CA_PhantomPain");
            }

            if (ModLister.HasActiveModWithName("Diseases Overhauled"))
            {
                list.AddDistinct("DO;HepatitisK");
                list.AddDistinct("DO;StomachUlcer");
                list.AddDistinct("DO;Stupor");
                list.AddDistinct("DO;Unease");
                list.AddDistinct("DO;PTSD");
                list.AddDistinct("DO;SuicidePreparation");
                list.AddDistinct("DO;Tuberculosis");
                list.AddDistinct("DO;KindredDickVirus");
                list.AddDistinct("DO;Sepsis");
                list.AddDistinct("DO;Toothache");
                list.AddDistinct("DO;VoightBernsteinDisease");
                list.AddDistinct("DO;HansenKampffDisease");
                list.AddDistinct("DO;Necrosis");
                list.AddDistinct("DO;LymphaticMechanites");
                list.AddDistinct("DO;Psoriasis");
                list.AddDistinct("DO;CommonCold");
                list.AddDistinct("DO;NewReschianFever");
                list.AddDistinct("DO;BloodCancer");
            }

            var Chemicals = DefDatabase<ChemicalDef>.AllDefsListForReading;
            if (Chemicals != null && Chemicals.Count > 0)
            {
                foreach (var Chemical in Chemicals)
                {
                    if (Chemical.defName == "MSMental")
                    {
                        continue;
                    }

                    var addiction = Chemical.addictionHediff;
                    if (addiction != null)
                    {
                        list.AddDistinct("VA;" + addiction.defName);
                    }
                }
            }

            list = (from s in list
                orderby s
                select s).ToList();
            return list;
        }

        // Token: 0x06000054 RID: 84 RVA: 0x00005824 File Offset: 0x00003A24
        public static bool MaladyUsed(string defname)
        {
            if (Maladies().Count <= 0)
            {
                return false;
            }

            foreach (var malstring in Maladies())
            {
                var divider = new[]
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

        // Token: 0x06000055 RID: 85 RVA: 0x000058A0 File Offset: 0x00003AA0
        public static List<ThingDef> MaladyDrugs()
        {
            var recipelist = DefDatabase<RecipeDef>.AllDefsListForReading;
            if (Current.ProgramState != ProgramState.Playing && DRSettings.ShowResearched)
            {
                Messages.Message("MSPainless.Warning".Translate(), LookTargets.Invalid, MessageTypeDefOf.CautionInput,
                    false);
            }

            var list = new List<ThingDef>();
            var chkList = DefDatabase<ThingDef>.AllDefsListForReading;
            if (chkList.Count <= 0)
            {
                return (from td in list
                    orderby td.label
                    select td).ToList();
            }

            foreach (var chkDef in chkList)
            {
                if (!chkDef.IsIngestible)
                {
                    continue;
                }

                var ingestible = chkDef.ingestible;
                var drugCategory = ingestible != null ? new DrugCategory?(ingestible.drugCategory) : null;
                var drugCategory2 = DrugCategory.Medical;
                if (!((drugCategory.GetValueOrDefault() == drugCategory2) & (drugCategory != null)))
                {
                    continue;
                }

                if (DRSettings.ShowResearched)
                {
                    if (MSPainDrug.IsResearchCompleted(chkDef, recipelist))
                    {
                        list.AddDistinct(chkDef);
                    }
                }
                else
                {
                    list.AddDistinct(chkDef);
                }
            }

            return (from td in list
                orderby td.label
                select td).ToList();
        }

        // Token: 0x06000056 RID: 86 RVA: 0x000059CC File Offset: 0x00003BCC
        public static void SetDRValues(string m, string d, int t, bool b, int numof, List<string> master,
            out List<string> newMaster)
        {
            newMaster = new List<string>();
            var newValue = ConvertToDRValue(t, m, d, b, numof);
            if (master == null)
            {
                return;
            }

            var beenSet = false;
            if (master.Count > 0)
            {
                foreach (var value in master)
                {
                    var a = HValuePart(value);
                    var num = NumValuePart(value);
                    if (a != m)
                    {
                        newMaster.AddDistinct(value);
                    }
                    else if (num != numof)
                    {
                        newMaster.AddDistinct(value);
                    }
                    else
                    {
                        newMaster.AddDistinct(newValue);
                        beenSet = true;
                    }
                }
            }

            if (!beenSet)
            {
                newMaster.AddDistinct(newValue);
            }
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00005A84 File Offset: 0x00003C84
        public static int GetValueDRnumof(string m, List<string> master)
        {
            if (m == null || master == null || master.Count <= 0)
            {
                return 1;
            }

            foreach (var value in master)
            {
                if (HValuePart(value) == m)
                {
                    return NumValuePart(value);
                }
            }

            return 1;
        }

        // Token: 0x06000058 RID: 88 RVA: 0x00005AF4 File Offset: 0x00003CF4
        public static int GetValueDRTime(string m, List<string> master, int num)
        {
            if (m == null || master == null || master.Count <= 0)
            {
                return 24;
            }

            foreach (var value in master)
            {
                var Hdef = HValuePart(value);
                var numof = NumValuePart(value);
                if (Hdef == m && numof == num)
                {
                    return TValuePart(value);
                }
            }

            return 24;
        }

        // Token: 0x06000059 RID: 89 RVA: 0x00005B74 File Offset: 0x00003D74
        public static string GetValueDRDrug(string m, List<string> master, int num)
        {
            if (m == null || master == null || master.Count <= 0)
            {
                return null;
            }

            foreach (var value in master)
            {
                var Hdef = HValuePart(value);
                var numof = NumValuePart(value);
                if (Hdef == m && numof == num)
                {
                    return DValuePart(value);
                }
            }

            return null;
        }

        // Token: 0x0600005A RID: 90 RVA: 0x00005BF4 File Offset: 0x00003DF4
        public static bool GetValueDRBills(string m, List<string> master)
        {
            if (m == null || master == null || master.Count <= 0)
            {
                return true;
            }

            foreach (var value in master)
            {
                if (HValuePart(value) == m)
                {
                    return BValuePart(value);
                }
            }

            return true;
        }

        // Token: 0x0600005B RID: 91 RVA: 0x00005C64 File Offset: 0x00003E64
        public static void ClearDRValues(string m, bool doAll, List<string> master, out List<string> newMaster)
        {
            newMaster = new List<string>();
            if (doAll || master == null || master.Count <= 0)
            {
                return;
            }

            foreach (var value in master)
            {
                if (HValuePart(value) != m)
                {
                    newMaster.AddDistinct(value);
                }
            }
        }

        // Token: 0x0600005C RID: 92 RVA: 0x00005CD8 File Offset: 0x00003ED8
        public static int NumValuePart(string value)
        {
            var divider = new[]
            {
                ';'
            };
            var segments = value.Split(divider);
            try
            {
                if (segments.Length > 4)
                {
                    return int.Parse(segments[4]);
                }

                return 1;
            }
            catch (FormatException)
            {
                Log.Message("Unable to parse Seg0: '" + segments[4] + "'");
            }

            return 1;
        }

        // Token: 0x0600005D RID: 93 RVA: 0x00005D40 File Offset: 0x00003F40
        public static int TValuePart(string value)
        {
            var divider = new[]
            {
                ';'
            };
            var segments = value.Split(divider);
            try
            {
                return int.Parse(segments[0]);
            }
            catch (FormatException)
            {
                Log.Message("Unable to parse Seg0: '" + segments[0] + "'");
            }

            return 24;
        }

        // Token: 0x0600005E RID: 94 RVA: 0x00005D9C File Offset: 0x00003F9C
        public static string HValuePart(string value)
        {
            var divider = new[]
            {
                ';'
            };
            return value.Split(divider)[1];
        }

        // Token: 0x0600005F RID: 95 RVA: 0x00005DC0 File Offset: 0x00003FC0
        public static string DValuePart(string value)
        {
            var divider = new[]
            {
                ';'
            };
            return value.Split(divider)[2];
        }

        // Token: 0x06000060 RID: 96 RVA: 0x00005DE4 File Offset: 0x00003FE4
        public static bool BValuePart(string value)
        {
            var divider = new[]
            {
                ';'
            };
            return value.Split(divider)[3] == "1";
        }

        // Token: 0x06000061 RID: 97 RVA: 0x00005E18 File Offset: 0x00004018
        public static string ConvertToDRValue(int t, string m, string d, bool b, int num)
        {
            var boolbit = "1";
            if (!b)
            {
                boolbit = "0";
            }

            return string.Concat(t.ToString(), ";", m, ";", d, ";", boolbit).Trim() + ";" + num;
        }
    }
}