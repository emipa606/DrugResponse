using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless;

public class MSDRUtility
{
    public static List<string> MaladyMods()
    {
        var list = new List<string>();
        list.AddDistinct("VN;Vanilla");
        list.AddDistinct("CA;Common Ailments");
        list.AddDistinct("CC;Common Ailments (Continued)");
        list.AddDistinct("DO;Diseases Overhauled");
        return list;
    }

    public static List<string> Maladies()
    {
        var list = new List<string>();
        list.AddDistinct("VN;Asthma");
        list.AddDistinct("VN;Burn");
        list.AddDistinct("VN;BloodLoss");
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
        if (Chemicals is { Count: > 0 })
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
                    list.AddDistinct($"VA;{addiction.defName}");
                }
            }
        }

        list = (from s in list
            orderby s
            select s).ToList();
        return list;
    }

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

    public static void SetDRValues(string m, string d, int t, bool b, int numof, List<string> master,
        out List<string> newMaster)
    {
        newMaster = [];
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

    public static int GetValueDRnumof(string m, List<string> master)
    {
        if (m == null || master is not { Count: > 0 })
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

    public static int GetValueDRTime(string m, List<string> master, int num)
    {
        if (m == null || master is not { Count: > 0 })
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

    public static string GetValueDRDrug(string m, List<string> master, int num)
    {
        if (m == null || master is not { Count: > 0 })
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

    public static bool GetValueDRBills(string m, List<string> master)
    {
        if (m == null || master is not { Count: > 0 })
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

    public static void ClearDRValues(string m, bool doAll, List<string> master, out List<string> newMaster)
    {
        newMaster = [];
        if (doAll || master is not { Count: > 0 })
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

    public static int NumValuePart(string value)
    {
        var divider = new[]
        {
            ';'
        };
        var segments = value.Split(divider);
        try
        {
            return segments.Length > 4 ? int.Parse(segments[4]) : 1;
        }
        catch (FormatException)
        {
            Log.Message($"Unable to parse Seg0: '{segments[4]}'");
        }

        return 1;
    }

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
            Log.Message($"Unable to parse Seg0: '{segments[0]}'");
        }

        return 24;
    }

    public static string HValuePart(string value)
    {
        var divider = new[]
        {
            ';'
        };
        return value.Split(divider)[1];
    }

    public static string DValuePart(string value)
    {
        var divider = new[]
        {
            ';'
        };
        return value.Split(divider)[2];
    }

    public static bool BValuePart(string value)
    {
        var divider = new[]
        {
            ';'
        };
        return value.Split(divider)[3] == "1";
    }

    public static string ConvertToDRValue(int t, string m, string d, bool b, int num)
    {
        var boolbit = "1";
        if (!b)
        {
            boolbit = "0";
        }

        return $"{$"{t};{m};{d};{boolbit}".Trim()};{num}";
    }
}