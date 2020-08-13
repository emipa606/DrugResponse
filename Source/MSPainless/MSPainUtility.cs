using System;
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
			ThingDef drugdef = null;
			if (MSPainUtility.IsInPain(pawn))
			{
				if (MSPainUtility.IsInExtremePain(pawn))
				{
					if (DRSettings.MSDrugExtremeDef != null)
					{
						highpain = true;
						return DRSettings.MSDrugExtremeDef;
					}
				}
				else if (MSPainUtility.IsInIntensePain(pawn))
				{
					if (DRSettings.MSDrugIntenseDef != null)
					{
						highpain = true;
						return DRSettings.MSDrugIntenseDef;
					}
				}
				else if (MSPainUtility.IsInSeriousPain(pawn))
				{
					if (DRSettings.MSDrugSeriousDef != null)
					{
						return DRSettings.MSDrugSeriousDef;
					}
				}
				else if (MSPainUtility.IsInMinorPain(pawn) && DRSettings.MSDrugMinorDef != null)
				{
					return DRSettings.MSDrugMinorDef;
				}
			}
			return drugdef;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00007281 File Offset: 0x00005481
		public static float GetPainAmount(Pawn pawn)
		{
			return pawn.health.hediffSet.PainTotal;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00007294 File Offset: 0x00005494
		public static int GetPainState(Pawn pawn)
		{
			float painTotal = MSPainUtility.GetPainAmount(pawn);
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
			return MSPainUtility.GetPainAmount(pawn) > 0.01f;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000072E3 File Offset: 0x000054E3
		public static bool IsInMinorPain(Pawn pawn)
		{
			return MSPainUtility.GetPainState(pawn) == 1;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000072EE File Offset: 0x000054EE
		public static bool IsInSeriousPain(Pawn pawn)
		{
			return MSPainUtility.GetPainState(pawn) == 2;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000072F9 File Offset: 0x000054F9
		public static bool IsInIntensePain(Pawn pawn)
		{
			return MSPainUtility.GetPainState(pawn) == 3;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007304 File Offset: 0x00005504
		public static bool IsInExtremePain(Pawn pawn)
		{
			return MSPainUtility.GetPainState(pawn) >= 4;
		}
	}
}
