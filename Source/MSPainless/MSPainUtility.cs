using Verse;

namespace MSPainless;

public static class MSPainUtility
{
    public static ThingDef NeedsPainReliefNow(Pawn pawn, out bool highpain)
    {
        highpain = false;
        if (!IsInPain(pawn))
        {
            return null;
        }

        if (IsInExtremePain(pawn))
        {
            if (DRSettings.MSDrugExtremeDef == null)
            {
                return null;
            }

            highpain = true;
            return DRSettings.MSDrugExtremeDef;
        }

        if (IsInIntensePain(pawn))
        {
            if (DRSettings.MSDrugIntenseDef == null)
            {
                return null;
            }

            highpain = true;
            return DRSettings.MSDrugIntenseDef;
        }

        if (IsInSeriousPain(pawn))
        {
            if (DRSettings.MSDrugSeriousDef != null)
            {
                return DRSettings.MSDrugSeriousDef;
            }
        }
        else if (IsInMinorPain(pawn) && DRSettings.MSDrugMinorDef != null)
        {
            return DRSettings.MSDrugMinorDef;
        }

        return null;
    }

    private static float GetPainAmount(Pawn pawn)
    {
        return pawn.health.hediffSet.PainTotal;
    }

    private static int GetPainState(Pawn pawn)
    {
        var painTotal = GetPainAmount(pawn);
        if (painTotal < 0.01f)
        {
            return 0;
        }

        if (painTotal < 0.15f)
        {
            return 1;
        }

        if (painTotal < 0.4f)
        {
            return 2;
        }

        if (painTotal < 0.8f)
        {
            return 3;
        }

        return 4;
    }

    public static bool IsInPain(Pawn pawn)
    {
        return GetPainAmount(pawn) > 0.01f;
    }

    private static bool IsInMinorPain(Pawn pawn)
    {
        return GetPainState(pawn) == 1;
    }

    private static bool IsInSeriousPain(Pawn pawn)
    {
        return GetPainState(pawn) == 2;
    }

    private static bool IsInIntensePain(Pawn pawn)
    {
        return GetPainState(pawn) == 3;
    }

    private static bool IsInExtremePain(Pawn pawn)
    {
        return GetPainState(pawn) >= 4;
    }
}