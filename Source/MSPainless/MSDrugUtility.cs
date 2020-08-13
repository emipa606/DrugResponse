using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace MSPainless
{
	// Token: 0x02000006 RID: 6
	public class MSDrugUtility
	{
		// Token: 0x0600004A RID: 74 RVA: 0x000050B8 File Offset: 0x000032B8
		public static bool IsValidPawnMod(Pawn pawn, bool painmode)
		{
			if (pawn.IsPrisoner)
			{
				if (painmode)
				{
					if (!DRSettings.DoIfPrisoner)
					{
						return false;
					}
				}
				else if (!DRSettings.DoDRIfPrisoner)
				{
					return false;
				}
			}
			else if (!pawn.IsColonist)
			{
				return false;
			}
			return (pawn.Spawned || CaravanUtility.IsCaravanMember(pawn)) && !pawn.Dead && !pawn.InMentalState;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00005114 File Offset: 0x00003314
		public static bool IsViolation(Pawn p, ThingDef t)
		{
			if (!t.IsDrug)
			{
				return true;
			}
			if ((p?.Faction) != null && p.Faction == Faction.OfPlayer)
			{
				return PawnUtility.IsTeetotaler(p) && t.IsNonMedicalDrug;
			}
			return t.IsNonMedicalDrug;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005168 File Offset: 0x00003368
		public static bool IsOKtoAdmin(Pawn pawn, HediffDef hdef, ThingDef def)
		{
			DrugPolicy drugPolicy;
			if (pawn == null)
			{
				drugPolicy = null;
			}
			else
			{
				Pawn_DrugPolicyTracker drugs = pawn.drugs;
				drugPolicy = (drugs?.CurrentPolicy);
			}
			DrugPolicy policy = drugPolicy;
			if (policy != null)
			{
				if (!MSDrugUtility.DPExists(policy, def))
				{
					Messages.Message(TranslatorFormattedStringExtensions.Translate("MSPainless.ErrDrugPolicy", pawn?.LabelShort, def?.label), pawn, MessageTypeDefOf.NeutralEvent, false);
					return false;
				}
				if (policy[def] != null)
				{
					DrugPolicyEntry entry = policy[def];
					if (entry != null && entry.allowScheduled && entry != null && entry.daysFrequency > 0f)
					{
						return false;
					}
				}
			}
			if (!DRSettings.DoIfImmune && MSDrugUtility.ImmuneNow(pawn, hdef))
			{
				return false;
			}
			if (hdef != null && hdef.defName == "Anesthetic")
			{
				HediffSet hediffSet;
				if (pawn == null)
				{
					hediffSet = null;
				}
				else
				{
					Pawn_HealthTracker health = pawn.health;
					hediffSet = (health?.hediffSet);
				}
				HediffSet set = hediffSet;
				if (set != null)
				{
					Hediff Anesthetic = set.GetFirstHediffOfDef(hdef, false);
					if (Anesthetic != null && Anesthetic.Severity >= 0.8f)
					{
						return false;
					}
				}
			}
			if (def.IsIngestible)
			{
				List<IngestionOutcomeDoer> ODs = def.ingestible.outcomeDoers;
				if (ODs.Count > 0)
				{
					bool toohighsev = false;
					HediffSet hediffSet2;
					if (pawn == null)
					{
						hediffSet2 = null;
					}
					else
					{
						Pawn_HealthTracker health2 = pawn.health;
						hediffSet2 = (health2?.hediffSet);
					}
					HediffSet hediffset = hediffSet2;
					if (hediffset != null)
					{
						foreach (IngestionOutcomeDoer OD in ODs)
						{
							if (OD is IngestionOutcomeDoer_GiveHediff)
							{
								IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = OD as IngestionOutcomeDoer_GiveHediff;
								HediffDef ODhediffdef = ingestionOutcomeDoer_GiveHediff?.hediffDef;
								if (ODhediffdef != null)
								{
									float ODSev = (OD as IngestionOutcomeDoer_GiveHediff).severity;
									if (ODSev > 0f)
									{
										Hediff ODhediff = hediffset.GetFirstHediffOfDef(ODhediffdef, false);
										if (ODhediff != null && ODhediff.Severity / ODSev > 0.75f)
										{
											toohighsev = true;
										}
									}
								}
							}
						}
						if (toohighsev)
						{
							return false;
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00005358 File Offset: 0x00003558
		public static bool DPExists(DrugPolicy policy, ThingDef drugDef)
		{
			if (policy.Count > 0)
			{
				for (int i = 0; i < policy.Count; i++)
				{
					DrugPolicyEntry drugPolicyEntry = policy[i];
					if ((drugPolicyEntry?.drug) == drugDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005398 File Offset: 0x00003598
		public static Thing FindDrugFor(Pawn pawn, ThingDef drugDef)
		{
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == drugDef && MSDrugUtility.DrugValidator(pawn, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			if (!pawn.IsPrisoner)
			{
				return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef), PathEndMode.ClosestTouch, TraverseParms.For(pawn,Danger.Deadly, 0, false), 9999f, (Thing x) => MSDrugUtility.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(drugDef),  PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.PassDoors,Danger.Deadly, false), 9999f, (Thing x) => MSDrugUtility.DrugValidator(pawn, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00005498 File Offset: 0x00003698
		private static bool DrugValidator(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (!pawn.IsPrisoner && drug.Spawned)
			{
				if (ForbidUtility.IsForbidden(drug, pawn))
				{
					return false;
				}
				if (!ReservationUtility.CanReserve(pawn, drug, 1, -1, null, false))
				{
					return false;
				}
				if (!SocialProperness.IsSociallyProper(drug, pawn))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000054F0 File Offset: 0x000036F0
		private static bool ImmuneNow(Pawn pawn, HediffDef chkhdef)
		{
			if (pawn != null && chkhdef != null)
			{
				HediffSet hediffSet;
				if (pawn == null)
				{
					hediffSet = null;
				}
				else
				{
					Pawn_HealthTracker health = pawn.health;
					hediffSet = (health?.hediffSet);
				}
				HediffSet set = hediffSet;
				if (set != null)
				{
					Hediff chk = set.GetFirstHediffOfDef(chkhdef, false);
					if (chk != null && HediffUtility.FullyImmune(chk))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
