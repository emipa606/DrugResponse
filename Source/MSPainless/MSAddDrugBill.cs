using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless
{
	// Token: 0x02000004 RID: 4
	public class MSAddDrugBill
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004A60 File Offset: 0x00002C60
		public static bool GenDrugResponse(bool isDR, Pawn pawn, HediffDef hdef, ThingDef drugDef = null, BodyPartRecord part = null, List<string> Master = null, int num = 1)
		{
			bool result = false;
			if (drugDef != null)
			{
				if (MSAddDrugBill.GenMSRecipe(pawn, hdef, drugDef, part))
				{
					result = true;
				}
			}
			else if (isDR)
			{
				string Hdefname = hdef?.defName;
				if (Hdefname != null)
				{
					string drugdefname = MSDRUtility.GetValueDRDrug(Hdefname, Master, num);
					if (drugdefname != null)
					{
						drugDef = DefDatabase<ThingDef>.GetNamed(drugdefname, false);
						if (drugDef != null && MSAddDrugBill.GenMSRecipe(pawn, hdef, drugDef, part))
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004AC0 File Offset: 0x00002CC0
		private static bool GenMSRecipe(Pawn pawn, HediffDef hdef, ThingDef drugdef, BodyPartRecord part = null)
		{
			bool result = false;
			if (MSDrugUtility.IsOKtoAdmin(pawn, hdef, drugdef))
			{
				RecipeDef recipeDef = DefDatabase<RecipeDef>.GetNamed("Administer_" + drugdef.defName, true);
				if (recipeDef != null)
				{
                    if (!MSAddDrugBill.IsViolation(pawn, recipeDef, out string reason))
                    {
                        if (!MSAddDrugBill.ChkDuplication(pawn, recipeDef) && MSAddDrugBill.GenAdminOption(pawn, recipeDef, part))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        Messages.Message(TranslatorFormattedStringExtensions.Translate("MSPainless.ViolationMsg", GenText.CapitalizeFirst(pawn.LabelShort), GenText.CapitalizeFirst(drugdef.label), reason), pawn, MessageTypeDefOf.NeutralEvent, false);
                    }
                }
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004B60 File Offset: 0x00002D60
		private static bool IsViolation(Pawn p, RecipeDef r, out string reason)
		{
			reason = "";
			if ((p?.Faction) != null && p.Faction == Faction.OfPlayer)
			{
				if (PawnUtility.IsTeetotaler(p) && r.ingredients[0].filter.AllowedThingDefs.First<ThingDef>().IsNonMedicalDrug)
				{
					reason = Translator.Translate("MSPainless.ViolationTeetotaler");
					return true;
				}
				return false;
			}
			else
			{
				if (r.ingredients[0].filter.AllowedThingDefs.First<ThingDef>().IsNonMedicalDrug)
				{
					reason = Translator.Translate("MSPainless.ViolationNonPlayer");
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004C04 File Offset: 0x00002E04
		private static bool ChkDuplication(Pawn pawn, RecipeDef recipedef)
		{
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				BillStack billStack = pawn.BillStack;
				flag = ((billStack?.Bills) != null);
			}
			if (flag && pawn.BillStack.Bills.Count > 0)
			{
				foreach (Bill bill in pawn.BillStack.Bills)
				{
					if ((bill?.recipe) != null && bill.recipe == recipedef)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004CA0 File Offset: 0x00002EA0
		private static bool GenAdminOption(Pawn patient, RecipeDef recipe, BodyPartRecord part = null)
		{
			bool result = false;
			if (patient != null)
			{
				Bill_Medical bill_Medical = new Bill_Medical(recipe);
				patient.BillStack.AddBill(bill_Medical);
				result = true;
				bill_Medical.Part = part;
				if (recipe.conceptLearned != null)
				{
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(recipe.conceptLearned, KnowledgeAmount.Total);
				}
				Pawn patient2 = patient;
				Map map = patient2?.Map;
				if (map != null)
				{
					if (!map.mapPawns.FreeColonists.Any((Pawn col) => recipe.PawnSatisfiesSkillRequirements(col)))
					{
						Bill.CreateNoPawnsWithSkillDialog(recipe);
					}
					if (!RestUtility.InBed(patient) && patient.RaceProps.IsFlesh)
					{
						if (patient.RaceProps.Humanlike)
						{
							if (!GenCollection.Any(map.listerBuildings.allBuildingsColonist, (Building x) => x is Building_Bed bed && RestUtility.CanUseBedEver(patient, x.def) && bed.Medical))
							{
								Messages.Message(Translator.Translate("MessageNoMedicalBeds"), patient, MessageTypeDefOf.CautionInput, false);
							}
						}
						else if (!GenCollection.Any<Building>(map.listerBuildings.allBuildingsColonist, (Building x) => x is Building_Bed && RestUtility.CanUseBedEver(patient, x.def)))
						{
							Messages.Message(Translator.Translate("MessageNoAnimalBeds"), patient, MessageTypeDefOf.CautionInput, false);
						}
					}
					if (patient.Faction != null && !patient.Faction.def.hidden && !FactionUtility.HostileTo(patient.Faction, Faction.OfPlayer) && recipe.Worker.IsViolationOnPawn(patient, part, Faction.OfPlayer))
					{
						Messages.Message(TranslatorFormattedStringExtensions.Translate("MessageMedicalOperationWillAngerFaction", patient.Faction), patient, MessageTypeDefOf.CautionInput, false);
					}
					ThingDef minRequiredMedicine = MSAddDrugBill.GetMinRequiredMedicine(recipe);
					if (minRequiredMedicine != null && patient.playerSettings != null && !MedicalCareUtility.AllowsMedicine(patient.playerSettings.medCare, minRequiredMedicine))
					{
						Messages.Message(TranslatorFormattedStringExtensions.Translate("MessageTooLowMedCare", minRequiredMedicine.label, patient.LabelShort, MedicalCareUtility.GetLabel(patient.playerSettings.medCare), NamedArgumentUtility.Named(patient, "PAWN")), patient, MessageTypeDefOf.CautionInput, false);
					}
				}
			}
			return result;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004F50 File Offset: 0x00003150
		private static ThingDef GetMinRequiredMedicine(RecipeDef recipe)
		{
			MSAddDrugBill.tmpMedicineBestToWorst.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].IsMedicine)
				{
					MSAddDrugBill.tmpMedicineBestToWorst.Add(allDefsListForReading[i]);
				}
			}
			GenCollection.SortByDescending<ThingDef, float>(MSAddDrugBill.tmpMedicineBestToWorst, (ThingDef x) => StatExtension.GetStatValueAbstract(x, StatDefOf.MedicalPotency, null));
			ThingDef thingDef = null;
			for (int j = 0; j < recipe.ingredients.Count; j++)
			{
				ThingDef thingDef2 = null;
				for (int k = 0; k < MSAddDrugBill.tmpMedicineBestToWorst.Count; k++)
				{
					if (recipe.ingredients[j].filter.Allows(MSAddDrugBill.tmpMedicineBestToWorst[k]))
					{
						thingDef2 = MSAddDrugBill.tmpMedicineBestToWorst[k];
					}
				}
				if (thingDef2 != null && (thingDef == null || StatExtension.GetStatValueAbstract(thingDef2, StatDefOf.MedicalPotency, null) > StatExtension.GetStatValueAbstract(thingDef, StatDefOf.MedicalPotency, null)))
				{
					thingDef = thingDef2;
				}
			}
			MSAddDrugBill.tmpMedicineBestToWorst.Clear();
			return thingDef;
		}

		// Token: 0x0400001F RID: 31
		private static readonly List<ThingDef> tmpMedicineBestToWorst = new List<ThingDef>();
	}
}
