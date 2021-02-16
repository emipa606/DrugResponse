using Verse;

namespace MSPainless
{
    // Token: 0x0200000B RID: 11
    public static class MSPainUtility
    {
        // Token: 0x06000081 RID: 129 RVA: 0x0000720C File Offset: 0x0000540C
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

        // Token: 0x06000082 RID: 130 RVA: 0x00007281 File Offset: 0x00005481
        private static float GetPainAmount(Pawn pawn)
        {
            return pawn.health.hediffSet.PainTotal;
        }

        // Token: 0x06000083 RID: 131 RVA: 0x00007294 File Offset: 0x00005494
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

        // Token: 0x06000084 RID: 132 RVA: 0x000072D1 File Offset: 0x000054D1
        public static bool IsInPain(Pawn pawn)
        {
            return GetPainAmount(pawn) > 0.01f;
        }

        // Token: 0x06000085 RID: 133 RVA: 0x000072E3 File Offset: 0x000054E3
        private static bool IsInMinorPain(Pawn pawn)
        {
            return GetPainState(pawn) == 1;
        }

        // Token: 0x06000086 RID: 134 RVA: 0x000072EE File Offset: 0x000054EE
        private static bool IsInSeriousPain(Pawn pawn)
        {
            return GetPainState(pawn) == 2;
        }

        // Token: 0x06000087 RID: 135 RVA: 0x000072F9 File Offset: 0x000054F9
        private static bool IsInIntensePain(Pawn pawn)
        {
            return GetPainState(pawn) == 3;
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00007304 File Offset: 0x00005504
        private static bool IsInExtremePain(Pawn pawn)
        {
            return GetPainState(pawn) >= 4;
        }
    }
}