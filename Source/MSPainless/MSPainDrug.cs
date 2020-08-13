using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless
{
	// Token: 0x02000008 RID: 8
	internal class MSPainDrug : DefModExtension
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00005E8C File Offset: 0x0000408C
		internal static bool GetManagesPain(ThingDef def)
		{
			bool ApplyPain = false;
			if (def.HasModExtension<MSPainDrug>())
			{
				ApplyPain = def.GetModExtension<MSPainDrug>().ManagesPain;
			}
			return ApplyPain;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005EB0 File Offset: 0x000040B0
		internal static List<ThingDef> PainDrugs()
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
					if (MSPainDrug.GetManagesPain(chkDef) && MSPainDrug.DrugOk(chkDef) && chkDef.IsIngestible)
					{
						bool flag;
						if (chkDef == null)
						{
							flag = (null != null);
						}
						else
						{
							IngestibleProperties ingestible = chkDef.ingestible;
							flag = ((ingestible?.outcomeDoers) != null);
						}
						if (flag)
						{
							List<IngestionOutcomeDoer> list2;
							if (chkDef == null)
							{
								list2 = null;
							}
							else
							{
								IngestibleProperties ingestible2 = chkDef.ingestible;
								list2 = (ingestible2?.outcomeDoers);
							}
							foreach (IngestionOutcomeDoer ODoer in list2)
							{
								if (ODoer is IngestionOutcomeDoer_GiveHediff)
								{
									IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff = ODoer as IngestionOutcomeDoer_GiveHediff;
									HediffDef ODHediffDef = ingestionOutcomeDoer_GiveHediff?.hediffDef;
									if (ODHediffDef != null)
									{
										List<HediffStage> ODHedStages = ODHediffDef.stages;
										if (ODHedStages != null)
										{
											foreach (HediffStage ODHedStage in ODHedStages)
											{
												if (ODHedStage.painFactor <= 0.8f || ODHedStage.painOffset <= -0.2f)
												{
													if (!DRSettings.ShowResearched)
													{
														GenCollection.AddDistinct<ThingDef>(list, chkDef);
														break;
													}
													if (MSPainDrug.IsResearchCompleted(chkDef, recipelist))
													{
														GenCollection.AddDistinct<ThingDef>(list, chkDef);
														break;
													}
													break;
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return (from td in list
			orderby td.label
			select td).ToList<ThingDef>();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000060E0 File Offset: 0x000042E0
		internal static bool DrugOk(ThingDef def)
		{
			if ((def?.ingestible) != null)
			{
				if (DRSettings.UseNonMedical)
				{
					if (def != null)
					{
						IngestibleProperties ingestible = def.ingestible;
						bool flag;
						if (ingestible == null)
						{
							flag = false;
						}
						else
						{
							DrugCategory drugCategory = ingestible.drugCategory;
							flag = true;
						}
						if (flag)
						{
							return true;
						}
					}
				}
				else if (def != null)
				{
					IngestibleProperties ingestible2 = def.ingestible;
					DrugCategory? drugCategory2 = (ingestible2 != null) ? new DrugCategory?(ingestible2.drugCategory) : null;
					DrugCategory drugCategory3 = DrugCategory.Medical;
					if (drugCategory2.GetValueOrDefault() == drugCategory3 & drugCategory2 != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000615C File Offset: 0x0000435C
		internal static bool IsResearchCompleted(ThingDef def, List<RecipeDef> recipes)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				bool flag;
				if (def == null)
				{
					flag = (null != null);
				}
				else
				{
					RecipeMakerProperties recipeMaker = def.recipeMaker;
					flag = ((recipeMaker?.researchPrerequisite) != null);
				}
				if (flag && def.recipeMaker.researchPrerequisite.IsFinished)
				{
					return true;
				}
				if (recipes.Count > 0)
				{
					using (List<RecipeDef>.Enumerator enumerator = recipes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RecipeDef recipe = enumerator.Current;
							if ((recipe?.products) != null)
							{
								foreach (ThingDefCountClass thingDefCountClass in recipe.products)
								{
									if ((thingDefCountClass?.thingDef) == def)
									{
										if ((recipe?.researchPrerequisite) == null)
										{
											return true;
										}
										if (recipe.researchPrerequisite.IsFinished)
										{
											return true;
										}
									}
								}
							}
						}
						return false;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		// Token: 0x04000020 RID: 32
		public bool ManagesPain;
	}
}
