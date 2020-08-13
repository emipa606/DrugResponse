using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace MSPainless
{
	// Token: 0x02000002 RID: 2
	public class MainTabWindow_DrugResponse : MainTabWindow
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1100f, 764f);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
		public override void DoWindowContents(Rect canvas)
		{
			this.SetInitialSizeAndPosition();
			string Yes = Translator.Translate("MSPainless.Yes");
			string No = Translator.Translate("MSPainless.No");
			float lineHeight = 30f;
			float spacing = 6f;
			float sectionspacing = 35f;
			canvas.height += lineHeight * 4f + 25f;
			float width = canvas.width / 2f - 50f;
			float textwidth = 120f;
			float posX = 0f;
			float posY = 6f;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.UsePainManagement") + ": " + (DRSettings.UsePainManagement ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUsePR(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUsePR(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list));
			}
			posX += width + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.UseDrugResponse") + ": " + (DRSettings.UseDrugResponse ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list2 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseDR(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseDR(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list2));
			}
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DoForPrisoners") + ": " + (DRSettings.DoIfPrisoner ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list3 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoPrisoners(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoPrisoners(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list3));
			}
			posX += width + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DoDRForPrisoners") + ": " + (DRSettings.DoDRIfPrisoner ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list4 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoDRPrisoners(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoDRPrisoners(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list4));
			}
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.UseNonMedical") + ": " + (DRSettings.UseNonMedical ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list5 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseNM(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseNM(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list5));
			}
			posX += width + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.ShowResearched") + ": " + (DRSettings.ShowResearched ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list6 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowR(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowR(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list6));
			}
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.ShowReliefMsg") + ": " + (DRSettings.ShowReliefMsg ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list7 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowNPR(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowNPR(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list7));
			}
			posX += width + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.ShowResponseMsg") + ": " + (DRSettings.ShowResponseMsg ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list8 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowNDR(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetShowNDR(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list8));
			}
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.BillsHighPain") + ": " + (DRSettings.BillsHighPain ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list9 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetBillsHPain(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetBillsHPain(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list9));
			}
			posX += width + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DoIfImmune") + ": " + (DRSettings.DoIfImmune ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list10 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoIfImm(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetDoIfImm(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list10));
			}
			string None = Translator.Translate("MSPainless.None");
			int painspacefactor = 2;
			posX = 0f;
			posY += lineHeight + sectionspacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DrugMinor") + ": " + ((DRSettings.MSDrugMinor != null) ? DRSettings.MSDrugMinorLabel : None), true, false, true))
			{
                List<FloatMenuOption> list11 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetPainMinorNone();
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSPainDrug.PainDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef Drug = enumerator.Current;
							list11.Add(new FloatMenuOption(GenText.CapitalizeFirst(Drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetPainMinor(Drug.defName);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list11));
			}
			posX += width + spacing * (float)painspacefactor;
			Texture2D DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDrugMinorDef);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing * (float)painspacefactor;
			if (DRSettings.MSDrugMinorDef != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugMinorDef);
			}
			posX += lineHeight + spacing * (float)painspacefactor;
			Widgets.TextField(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY), Translator.Translate("MSPainless.DrugMinorLevels"));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DrugSerious") + ": " + ((DRSettings.MSDrugSerious != null) ? DRSettings.MSDrugSeriousLabel : None), true, false, true))
			{
                List<FloatMenuOption> list12 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetPainSeriousNone();
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSPainDrug.PainDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef Drug = enumerator.Current;
							list12.Add(new FloatMenuOption(GenText.CapitalizeFirst(Drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetPainSerious(Drug.defName);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list12));
			}
			posX += width + spacing * (float)painspacefactor;
			DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDrugSeriousDef);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing * (float)painspacefactor;
			if (DRSettings.MSDrugSeriousDef != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugSeriousDef);
			}
			posX += lineHeight + spacing * (float)painspacefactor;
			Widgets.TextField(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY), Translator.Translate("MSPainless.DrugSeriousLevels"));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DrugIntense") + ": " + ((DRSettings.MSDrugIntense != null) ? DRSettings.MSDrugIntenseLabel : None), true, false, true))
			{
                List<FloatMenuOption> list13 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetPainIntenseNone();
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSPainDrug.PainDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef Drug = enumerator.Current;
							list13.Add(new FloatMenuOption(GenText.CapitalizeFirst(Drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetPainIntense(Drug.defName);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list13));
			}
			posX += width + spacing * (float)painspacefactor;
			DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDrugIntenseDef);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing * (float)painspacefactor;
			if (DRSettings.MSDrugIntenseDef != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugIntenseDef);
			}
			posX += lineHeight + spacing * (float)painspacefactor;
			Widgets.TextField(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY), Translator.Translate("MSPainless.DrugIntenseLevels"));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.DrugExtreme") + ": " + ((DRSettings.MSDrugExtreme != null) ? DRSettings.MSDrugExtremeLabel : None), true, false, true))
			{
                List<FloatMenuOption> list14 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetPainExtremeNone();
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSPainDrug.PainDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef Drug = enumerator.Current;
							list14.Add(new FloatMenuOption(GenText.CapitalizeFirst(Drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetPainExtreme(Drug.defName);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list14));
			}
			posX += width + spacing * (float)painspacefactor;
			DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDrugExtremeDef);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing * (float)painspacefactor;
			if (DRSettings.MSDrugExtremeDef != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugExtremeDef);
			}
			posX += lineHeight + spacing * (float)painspacefactor;
			Widgets.TextField(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY), Translator.Translate("MSPainless.DrugExtremeLevels"));
			posX = 0f;
			posY += lineHeight + spacing * 2f;
			Rect mspainlessDrugRect = MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
			float num = (float)DRSettings.PainReliefWaitPeriod;
			float num2 = 6f;
			float num3 = 48f;
			bool flag = false;
			TaggedString taggedString = Translator.Translate("MSPainless.PainReliefWaitPeriod") + " : ";
			int num4 = DRSettings.PainReliefWaitPeriod;
			MainTabWindow_DrugResponse.SetPRTime(Widgets.HorizontalSlider(mspainlessDrugRect, num, num2, num3, flag, taggedString + num4.ToString() + TranslatorFormattedStringExtensions.Translate("MSPainless.WaitDays", MainTabWindow_DrugResponse.ConvertToDaysDesc(DRSettings.PainReliefWaitPeriod)), null, null, 0f));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.MethodRelief") + ": " + (DRSettings.UseReliefBills ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list15 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseRB(true);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseRB(false);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list15));
			}
			int shift = 5;
			posX = 0f;
			posY += lineHeight + sectionspacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width - spacing * ((float)shift * 1.5f), posX, posY), Translator.Translate("MSPainless.Malady") + ": " + ((DRSettings.MSDRHed != null) ? DRSettings.MSDRHedLabel : None), true, false, true))
			{
                List<FloatMenuOption> list16 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetMaladyNone(null, null, 24, true, 1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSDRUtility.Maladies().Count > 0)
				{
					List<HediffDef> hedlist = DefDatabase<HediffDef>.AllDefsListForReading;
					char[] divider = new char[]
					{
						';'
					};
					List<string> descList = new List<string>();
					foreach (string text in MSDRUtility.Maladies())
					{
						string[] segments = text.Split(divider);
						string mod = segments[0];
						string malady = segments[1];
						string maladylabel = GenText.CapitalizeFirst(hedlist.Find((HediffDef hd) => hd.defName == malady).label);
						GenCollection.AddDistinct<string>(descList, string.Concat(new string[]
						{
							maladylabel,
							" (",
							mod,
							");",
							malady
						}));
					}
					if (descList.Count > 0)
					{
						descList = (from s in descList
						orderby s
						select s).ToList<string>();
						foreach (string text2 in descList)
						{
							string[] descSegments = text2.Split(divider);
							string descLabel = descSegments[0];
							string descMalady = descSegments[1];
							list16.Add(new FloatMenuOption(descLabel, delegate()
							{
								MainTabWindow_DrugResponse.SetMalady(descMalady, 1);
								DRSettings.MSDRHed = descMalady;
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list16));
			}
			posX += width + spacing - spacing * ((float)shift * 1.5f);
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width - spacing * ((float)shift * 1.5f), posX, posY), Translator.Translate("MSPainless.MaladyDrug") + ": " + ((DRSettings.MSDRThg != null) ? DRSettings.MSDRThgLabel : None), true, false, true))
			{
                List<FloatMenuOption> list17 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetMaladyDrugNone(1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSDRUtility.MaladyDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSDRUtility.MaladyDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef drug = enumerator.Current;
							list17.Add(new FloatMenuOption(GenText.CapitalizeFirst(drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetMaladyDrug(drug.defName, 1);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list17));
			}
			posX += width + spacing - spacing * ((float)shift * 1.5f);
			DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDRThgDef);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing;
			if (DRSettings.MSDRThgDef != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDRThgDef);
			}
			posX += lineHeight + spacing;
			if (DRSettings.MSDRHedDef != null && Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight + 2f, posX, posY), Translator.Translate("MSPainless.Set"), true, false, true))
			{
                List<FloatMenuOption> list18 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.Set"), delegate ()
                    {
                        if (DRSettings.MSDRHed != null)
                        {
                            MainTabWindow_DrugResponse.SetMaladySet(1);
                        }
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list18));
			}
			posX += lineHeight + spacing * 2f;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight + 20f, posX, posY), Translator.Translate("MSPainless.Clear"), true, false, true))
			{
                List<FloatMenuOption> list19 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.Clear"), delegate ()
                    {
                        MainTabWindow_DrugResponse.ClearMaladyOne(1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.ClearAll"), delegate ()
                    {
                        MainTabWindow_DrugResponse.ClearMaladyAll(1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list19));
			}
			posX = 0f;
			posY += lineHeight + spacing * 2f;
			Rect mspainlessDrugRect2 = MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
			float num5 = (float)DRSettings.DRWaitPeriod;
			float num6 = 6f;
			float num7 = 120f;
			bool flag2 = false;
			TaggedString taggedString2 = Translator.Translate("MSPainless.DRWaitPeriod") + " : ";
			num4 = DRSettings.DRWaitPeriod;
			MainTabWindow_DrugResponse.SetDRTime(Widgets.HorizontalSlider(mspainlessDrugRect2, num5, num6, num7, flag2, taggedString2 + num4.ToString() + TranslatorFormattedStringExtensions.Translate("MSPainless.WaitDays", MainTabWindow_DrugResponse.ConvertToDaysDesc(DRSettings.DRWaitPeriod)), null, null, 0f));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.MethodResponse") + ": " + (DRSettings.UseDRBills ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list20 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseDB(true, 1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(No, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseDB(false, 1);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list20));
			}
			posX = 0f;
			posY += lineHeight + sectionspacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width - spacing * ((float)shift * 1.5f), posX, posY), Translator.Translate("MSPainless.Malady") + ": " + ((DRSettings.MSDRHed2 != null) ? DRSettings.MSDRHedLabel2 : None), true, false, true))
			{
                List<FloatMenuOption> list21 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetMaladyNone(null, null, 24, true, 2);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSDRUtility.Maladies().Count > 0)
				{
					List<HediffDef> hedlist2 = DefDatabase<HediffDef>.AllDefsListForReading;
					char[] divider2 = new char[]
					{
						';'
					};
					List<string> descList2 = new List<string>();
					foreach (string text3 in MSDRUtility.Maladies())
					{
						string[] segments2 = text3.Split(divider2);
						string mod2 = segments2[0];
						string malady = segments2[1];
						string maladylabel2 = GenText.CapitalizeFirst(hedlist2.Find((HediffDef hd) => hd.defName == malady).label);
						GenCollection.AddDistinct<string>(descList2, string.Concat(new string[]
						{
							maladylabel2,
							" (",
							mod2,
							");",
							malady
						}));
					}
					if (descList2.Count > 0)
					{
						descList2 = (from s in descList2
						orderby s
						select s).ToList<string>();
						foreach (string text4 in descList2)
						{
							string[] descSegments2 = text4.Split(divider2);
							string descLabel2 = descSegments2[0];
							string descMalady = descSegments2[1];
							list21.Add(new FloatMenuOption(descLabel2, delegate()
							{
								MainTabWindow_DrugResponse.SetMalady(descMalady, 2);
								DRSettings.MSDRHed2 = descMalady;
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list21));
			}
			posX += width + spacing - spacing * ((float)shift * 1.5f);
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width - spacing * ((float)shift * 1.5f), posX, posY), Translator.Translate("MSPainless.MaladyDrug") + ": " + ((DRSettings.MSDRThg2 != null) ? DRSettings.MSDRThgLabel2 : None), true, false, true))
			{
                List<FloatMenuOption> list22 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetMaladyDrugNone(2);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                if (MSDRUtility.MaladyDrugs().Count > 0)
				{
					using (List<ThingDef>.Enumerator enumerator = MSDRUtility.MaladyDrugs().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ThingDef drug = enumerator.Current;
							list22.Add(new FloatMenuOption(GenText.CapitalizeFirst(drug.label), delegate()
							{
								MainTabWindow_DrugResponse.SetMaladyDrug(drug.defName, 2);
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list22));
			}
			posX += width + spacing - spacing * ((float)shift * 1.5f);
			DrugImage = MainTabWindow_DrugResponse.GetDrugImage(DRSettings.MSDRThgDef2);
			Widgets.ButtonImageFitted(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
			posX += lineHeight + spacing;
			if (DRSettings.MSDRThgDef2 != null)
			{
				Widgets.InfoCardButton(posX, posY, DRSettings.MSDRThgDef2);
			}
			posX += lineHeight + spacing;
			if (DRSettings.MSDRHedDef2 != null && Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight + 2f, posX, posY), Translator.Translate("MSPainless.Set"), true, false, true))
			{
                List<FloatMenuOption> list23 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.Set"), delegate ()
                    {
                        if (DRSettings.MSDRHed2 != null)
                        {
                            MainTabWindow_DrugResponse.SetMaladySet(2);
                        }
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list23));
			}
			posX += lineHeight + spacing * 2f;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, lineHeight + 20f, posX, posY), Translator.Translate("MSPainless.Clear"), true, false, true))
			{
                List<FloatMenuOption> list24 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.Clear"), delegate ()
                    {
                        MainTabWindow_DrugResponse.ClearMaladyOne(2);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null),
                    new FloatMenuOption(Translator.Translate("MSPainless.ClearAll"), delegate ()
                    {
                        MainTabWindow_DrugResponse.ClearMaladyAll(2);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list24));
			}
			posX = 0f;
			posY += lineHeight + spacing * 2f;
			Rect mspainlessDrugRect3 = MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
			float num8 = (float)DRSettings.DRWaitPeriod2;
			float num9 = 6f;
			float num10 = 120f;
			bool flag3 = false;
			TaggedString taggedString3 = Translator.Translate("MSPainless.DRWaitPeriod") + " : ";
			num4 = DRSettings.DRWaitPeriod2;
			MainTabWindow_DrugResponse.SetDRTime2(Widgets.HorizontalSlider(mspainlessDrugRect3, num8, num9, num10, flag3, taggedString3 + num4.ToString() + TranslatorFormattedStringExtensions.Translate("MSPainless.WaitDays", MainTabWindow_DrugResponse.ConvertToDaysDesc(DRSettings.DRWaitPeriod2)), null, null, 0f));
			posX = 0f;
			posY += lineHeight + spacing;
			if (Widgets.ButtonText(MainTabWindow_DrugResponse.GetMSPainlessDrugRect(lineHeight, width, posX, posY), Translator.Translate("MSPainless.MethodResponse") + ": " + (DRSettings.UseDRBills2 ? Yes : No), true, false, true))
			{
                List<FloatMenuOption> list25 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate ()
                    {
                        MainTabWindow_DrugResponse.SetUseDB(true, 2);
                    }, MenuOptionPriority.Default, null, null, 0f, null, null)
                };
                Find.WindowStack.Add(new FloatMenu(list25));
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00003ED4 File Offset: 0x000020D4
		public static string ConvertToDaysDesc(int hours)
		{
			return ((float)hours / 24f).ToString("F2");
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00003EF6 File Offset: 0x000020F6
		public static void SetDoIfImm(bool b)
		{
			DRSettings.DoIfImmune = b;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00003EFE File Offset: 0x000020FE
		public static void SetBillsHPain(bool b)
		{
			DRSettings.BillsHighPain = b;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00003F06 File Offset: 0x00002106
		public static void SetDRTime(float t)
		{
			DRSettings.DRWaitPeriod = (int)t;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00003F0F File Offset: 0x0000210F
		public static void SetDRTime2(float t)
		{
			DRSettings.DRWaitPeriod2 = (int)t;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003F18 File Offset: 0x00002118
		public static void SetPRTime(float t)
		{
			DRSettings.PainReliefWaitPeriod = (int)t;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00003F21 File Offset: 0x00002121
		public static void SetUseDB(bool b, int num)
		{
			DRSettings.UseDRBills = b;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00003F29 File Offset: 0x00002129
		public static void SetUseRB(bool b)
		{
			DRSettings.UseReliefBills = b;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00003F31 File Offset: 0x00002131
		public static void SetShowNDR(bool b)
		{
			DRSettings.ShowResponseMsg = b;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00003F39 File Offset: 0x00002139
		public static void SetShowNPR(bool b)
		{
			DRSettings.ShowReliefMsg = b;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00003F41 File Offset: 0x00002141
		public static void SetShowR(bool b)
		{
			DRSettings.ShowResearched = b;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003F49 File Offset: 0x00002149
		public static void SetUseNM(bool b)
		{
			DRSettings.UseNonMedical = b;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00003F51 File Offset: 0x00002151
		public static void SetDoPrisoners(bool b)
		{
			DRSettings.DoIfPrisoner = b;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003F59 File Offset: 0x00002159
		public static void SetDoDRPrisoners(bool b)
		{
			DRSettings.DoDRIfPrisoner = b;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003F61 File Offset: 0x00002161
		public static void SetUseDR(bool b)
		{
			DRSettings.UseDrugResponse = b;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003F69 File Offset: 0x00002169
		public static void SetUsePR(bool b)
		{
			DRSettings.UsePainManagement = b;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003F74 File Offset: 0x00002174
		public static void ClearMaladyOne(int num)
		{
			if (num == 2)
			{
                MSDRUtility.ClearDRValues(DRSettings.MSDRHed2, false, DRSettings.MSDRValues2, out List<string> newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
				DRSettings.MSDRThg2 = null;
				DRSettings.DRWaitPeriod2 = 24;
				DRSettings.UseDRBills2 = true;
				return;
			}
            MSDRUtility.ClearDRValues(DRSettings.MSDRHed, false, DRSettings.MSDRValues, out List<string> newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
			DRSettings.MSDRThg = null;
			DRSettings.DRWaitPeriod = 24;
			DRSettings.UseDRBills = true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00003FDC File Offset: 0x000021DC
		public static void ClearMaladyAll(int num)
		{
			if (num == 2)
			{
                MSDRUtility.ClearDRValues(DRSettings.MSDRHed2, true, DRSettings.MSDRValues2, out List<string> newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
				DRSettings.MSDRHed2 = null;
				DRSettings.MSDRThg2 = null;
				DRSettings.DRWaitPeriod2 = 24;
				DRSettings.UseDRBills2 = true;
				return;
			}
            MSDRUtility.ClearDRValues(DRSettings.MSDRHed, true, DRSettings.MSDRValues, out List<string> newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
			DRSettings.MSDRHed = null;
			DRSettings.MSDRThg = null;
			DRSettings.DRWaitPeriod = 24;
			DRSettings.UseDRBills = true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00004050 File Offset: 0x00002250
		public static void SetMaladySet(int num)
		{
			if (num == 2)
			{
                MSDRUtility.SetDRValues(DRSettings.MSDRHed2, DRSettings.MSDRThg2, DRSettings.DRWaitPeriod2, DRSettings.UseDRBills2, num, DRSettings.MSDRValues2, out List<string> newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
				return;
			}
            MSDRUtility.SetDRValues(DRSettings.MSDRHed, DRSettings.MSDRThg, DRSettings.DRWaitPeriod, DRSettings.UseDRBills, num, DRSettings.MSDRValues, out List<string> newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000040B0 File Offset: 0x000022B0
		public static void SetMaladyDrug(string d, int num)
		{
			if (num == 2)
			{
				DRSettings.MSDRThg2 = d;
				return;
			}
			DRSettings.MSDRThg = d;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000040C4 File Offset: 0x000022C4
		public static void SetMaladyDrugNone(int num)
		{
			if (num == 2)
			{
				DRSettings.MSDRThg2 = null;
				DRSettings.DRWaitPeriod2 = 24;
				MainTabWindow_DrugResponse.SetValueDRBills(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
				return;
			}
			DRSettings.MSDRThg = null;
			DRSettings.DRWaitPeriod = 24;
			MainTabWindow_DrugResponse.SetValueDRBills(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00004110 File Offset: 0x00002310
		public static void SetMalady(string m, int num)
		{
			if (num > 0)
			{
				DRSettings.MSDRHed2 = m;
				MainTabWindow_DrugResponse.SetValueDRDrug(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
				MainTabWindow_DrugResponse.SetValueDRTime(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
				MainTabWindow_DrugResponse.SetValueDRBills(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
				DRSettings.MSDRHed = m;
				MainTabWindow_DrugResponse.SetValueDRDrug(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
				MainTabWindow_DrugResponse.SetValueDRTime(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
				MainTabWindow_DrugResponse.SetValueDRBills(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000418D File Offset: 0x0000238D
		public static void SetMaladyNone(string h, string d, int t, bool b, int num)
		{
			if (num == 2)
			{
				DRSettings.MSDRHed2 = h;
				DRSettings.MSDRThg2 = d;
				DRSettings.DRWaitPeriod2 = t;
				DRSettings.UseDRBills2 = b;
				return;
			}
			DRSettings.MSDRHed = h;
			DRSettings.MSDRThg = d;
			DRSettings.DRWaitPeriod = t;
			DRSettings.UseDRBills = b;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000041C5 File Offset: 0x000023C5
		public static void SetPainMinorNone()
		{
			DRSettings.MSDrugMinor = null;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000041CD File Offset: 0x000023CD
		public static void SetPainSeriousNone()
		{
			DRSettings.MSDrugSerious = null;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000041D5 File Offset: 0x000023D5
		public static void SetPainIntenseNone()
		{
			DRSettings.MSDrugIntense = null;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000041DD File Offset: 0x000023DD
		public static void SetPainExtremeNone()
		{
			DRSettings.MSDrugExtreme = null;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000041E5 File Offset: 0x000023E5
		public static void SetPainMinor(string defname)
		{
			DRSettings.MSDrugMinor = defname;
			DRSettings.SetCachedValue(defname, 1);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000041F4 File Offset: 0x000023F4
		public static void SetPainSerious(string defname)
		{
			DRSettings.MSDrugSerious = defname;
			DRSettings.SetCachedValue(defname, 2);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00004203 File Offset: 0x00002403
		public static void SetPainIntense(string defname)
		{
			DRSettings.MSDrugIntense = defname;
			DRSettings.SetCachedValue(defname, 3);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00004212 File Offset: 0x00002412
		public static void SetPainExtreme(string defname)
		{
			DRSettings.MSDrugExtreme = defname;
			DRSettings.SetCachedValue(defname, 4);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00004224 File Offset: 0x00002424
		public static void SetValueDRDrug(string hed, List<string> master, int num)
		{
			string setto = null;
			if (hed != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					string hdef = MSDRUtility.HValuePart(value);
					int numof = MSDRUtility.NumValuePart(value);
					if (hdef == hed && numof == num)
					{
						setto = MSDRUtility.DValuePart(value);
					}
				}
			}
			if (num == 2)
			{
				DRSettings.MSDRThg2 = setto;
				return;
			}
			DRSettings.MSDRThg = setto;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000042B4 File Offset: 0x000024B4
		public static void SetValueDRTime(string hed, List<string> master, int num)
		{
			int setto = 24;
			if (hed != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					string hdef = MSDRUtility.HValuePart(value);
					int numof = MSDRUtility.NumValuePart(value);
					if (hdef == hed && numof == num)
					{
						setto = MSDRUtility.TValuePart(value);
					}
				}
			}
			if (num == 2)
			{
				DRSettings.DRWaitPeriod2 = setto;
				return;
			}
			DRSettings.DRWaitPeriod = setto;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00004344 File Offset: 0x00002544
		public static void SetValueDRBills(string hed, List<string> master, int num)
		{
			bool setto = true;
			if (hed != null && master != null && master.Count > 0)
			{
				foreach (string value in master)
				{
					string hdef = MSDRUtility.HValuePart(value);
					int numof = MSDRUtility.NumValuePart(value);
					if (hdef == hed && numof == num)
					{
						setto = MSDRUtility.BValuePart(value);
					}
				}
			}
			if (num == 2)
			{
				DRSettings.UseDRBills2 = setto;
				return;
			}
			DRSettings.UseDRBills = setto;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000043D4 File Offset: 0x000025D4
		public static Texture2D GetDrugImage(ThingDef def)
		{
			string text = "UI/NullDrug";
			string NullIconPath = "UI/NullIcon";
			Texture2D NullDrug = ContentFinder<Texture2D>.Get(text, false);
			Texture2D NullIcon = ContentFinder<Texture2D>.Get(NullIconPath, false);
			if (def == null)
			{
				return NullDrug;
			}
			Texture2D icon = def?.uiIcon;
			if (icon != null)
			{
				return icon;
			}
			if (def != null)
			{
				GraphicData graphicData = def.graphicData;
				if ((graphicData?.texPath) != null)
				{
					string text2;
					if (def == null)
					{
						text2 = null;
					}
					else
					{
						GraphicData graphicData2 = def.graphicData;
						text2 = (graphicData2?.texPath);
					}
					string texturePath = text2;
					if (def.graphicData.graphicClass.Name == "Graphic_StackCount")
					{
						texturePath = texturePath + "/" + def.defName + "_a";
					}
					if (ContentFinder<Texture2D>.Get(texturePath, false))
					{
						return ContentFinder<Texture2D>.Get(texturePath, false);
					}
					return NullIcon;
				}
			}
			return NullIcon;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000449C File Offset: 0x0000269C
		public static Rect GetMSPainlessDrugRect(float height, float width, float posX, float posY)
		{
			return new Rect(posX, posY, width, height);
		}
	}
}
