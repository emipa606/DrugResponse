using System.Collections.Generic;
using Verse;

namespace MSPainless;

[StaticConstructorOnStartup]
public static class CheckDrugDefinitions
{
    static CheckDrugDefinitions()
    {
        checkPainDrugDefinitions();
        checkDrDrugDefinitions1();
        checkDrDrugDefinitions2();
    }

    private static void checkPainDrugDefinitions()
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

    private static void checkDrDrugDefinitions1()
    {
        var newMSDRValues = new List<string>();
        if (DRSettings.MSDRValues is { Count: > 0 })
        {
            foreach (var value in DRSettings.MSDRValues)
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

        DRSettings.MSDRValues = newMSDRValues;
    }

    private static void checkDrDrugDefinitions2()
    {
        var newMSDRValues = new List<string>();
        if (DRSettings.MSDRValues2 is { Count: > 0 })
        {
            foreach (var value in DRSettings.MSDRValues2)
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

        DRSettings.MSDRValues2 = newMSDRValues;
    }
}