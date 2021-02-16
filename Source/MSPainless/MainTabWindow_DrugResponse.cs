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
        public override Vector2 RequestedTabSize => new Vector2(1100f, 764f);

        // Token: 0x06000002 RID: 2 RVA: 0x00002064 File Offset: 0x00000264
        public override void DoWindowContents(Rect canvas)
        {
            SetInitialSizeAndPosition();
            string Yes = "MSPainless.Yes".Translate();
            string No = "MSPainless.No".Translate();
            var lineHeight = 30f;
            var spacing = 6f;
            var sectionspacing = 35f;
            canvas.height += (lineHeight * 4f) + 25f;
            var width = (canvas.width / 2f) - 50f;
            var textwidth = 120f;
            var posX = 0f;
            var posY = 6f;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.UsePainManagement".Translate() + ": " + (DRSettings.UsePainManagement ? Yes : No), true,
                false))
            {
                var list = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetUsePR(true); }),
                    new FloatMenuOption(No, delegate { SetUsePR(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list));
            }

            posX += width + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.UseDrugResponse".Translate() + ": " + (DRSettings.UseDrugResponse ? Yes : No), true, false))
            {
                var list2 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetUseDR(true); }),
                    new FloatMenuOption(No, delegate { SetUseDR(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list2));
            }

            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DoForPrisoners".Translate() + ": " + (DRSettings.DoIfPrisoner ? Yes : No), true, false))
            {
                var list3 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetDoPrisoners(true); }),
                    new FloatMenuOption(No, delegate { SetDoPrisoners(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list3));
            }

            posX += width + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DoDRForPrisoners".Translate() + ": " + (DRSettings.DoDRIfPrisoner ? Yes : No), true, false))
            {
                var list4 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetDoDRPrisoners(true); }),
                    new FloatMenuOption(No, delegate { SetDoDRPrisoners(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list4));
            }

            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.UseNonMedical".Translate() + ": " + (DRSettings.UseNonMedical ? Yes : No), true, false))
            {
                var list5 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetUseNM(true); }),
                    new FloatMenuOption(No, delegate { SetUseNM(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list5));
            }

            posX += width + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.ShowResearched".Translate() + ": " + (DRSettings.ShowResearched ? Yes : No), true, false))
            {
                var list6 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetShowR(true); }),
                    new FloatMenuOption(No, delegate { SetShowR(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list6));
            }

            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.ShowReliefMsg".Translate() + ": " + (DRSettings.ShowReliefMsg ? Yes : No), true, false))
            {
                var list7 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetShowNPR(true); }),
                    new FloatMenuOption(No, delegate { SetShowNPR(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list7));
            }

            posX += width + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.ShowResponseMsg".Translate() + ": " + (DRSettings.ShowResponseMsg ? Yes : No), true, false))
            {
                var list8 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetShowNDR(true); }),
                    new FloatMenuOption(No, delegate { SetShowNDR(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list8));
            }

            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.BillsHighPain".Translate() + ": " + (DRSettings.BillsHighPain ? Yes : No), true, false))
            {
                var list9 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetBillsHPain(true); }),
                    new FloatMenuOption(No, delegate { SetBillsHPain(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list9));
            }

            posX += width + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DoIfImmune".Translate() + ": " + (DRSettings.DoIfImmune ? Yes : No), true, false))
            {
                var list10 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetDoIfImm(true); }),
                    new FloatMenuOption(No, delegate { SetDoIfImm(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list10));
            }

            string None = "MSPainless.None".Translate();
            var painspacefactor = 2;
            posX = 0f;
            posY += lineHeight + sectionspacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DrugMinor".Translate() + ": " +
                (DRSettings.MSDrugMinor != null ? DRSettings.MSDrugMinorLabel : None), true, false))
            {
                var list11 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, SetPainMinorNone)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
                {
                    using (var enumerator = MSPainDrug.PainDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var Drug = enumerator.Current;
                            list11.Add(new FloatMenuOption(Drug.label.CapitalizeFirst(),
                                delegate { SetPainMinor(Drug.defName); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list11));
            }

            posX += width + (spacing * painspacefactor);
            var DrugImage = GetDrugImage(DRSettings.MSDrugMinorDef);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + (spacing * painspacefactor);
            if (DRSettings.MSDrugMinorDef != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugMinorDef);
            }

            posX += lineHeight + (spacing * painspacefactor);
            Widgets.TextField(GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY),
                "MSPainless.DrugMinorLevels".Translate());
            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DrugSerious".Translate() + ": " +
                (DRSettings.MSDrugSerious != null ? DRSettings.MSDrugSeriousLabel : None), true, false))
            {
                var list12 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, SetPainSeriousNone)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
                {
                    using (var enumerator = MSPainDrug.PainDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var Drug = enumerator.Current;
                            list12.Add(new FloatMenuOption(Drug.label.CapitalizeFirst(),
                                delegate { SetPainSerious(Drug.defName); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list12));
            }

            posX += width + (spacing * painspacefactor);
            DrugImage = GetDrugImage(DRSettings.MSDrugSeriousDef);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + (spacing * painspacefactor);
            if (DRSettings.MSDrugSeriousDef != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugSeriousDef);
            }

            posX += lineHeight + (spacing * painspacefactor);
            Widgets.TextField(GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY),
                "MSPainless.DrugSeriousLevels".Translate());
            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DrugIntense".Translate() + ": " +
                (DRSettings.MSDrugIntense != null ? DRSettings.MSDrugIntenseLabel : None), true, false))
            {
                var list13 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, SetPainIntenseNone)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
                {
                    using (var enumerator = MSPainDrug.PainDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var Drug = enumerator.Current;
                            list13.Add(new FloatMenuOption(Drug.label.CapitalizeFirst(),
                                delegate { SetPainIntense(Drug.defName); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list13));
            }

            posX += width + (spacing * painspacefactor);
            DrugImage = GetDrugImage(DRSettings.MSDrugIntenseDef);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + (spacing * painspacefactor);
            if (DRSettings.MSDrugIntenseDef != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugIntenseDef);
            }

            posX += lineHeight + (spacing * painspacefactor);
            Widgets.TextField(GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY),
                "MSPainless.DrugIntenseLevels".Translate());
            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.DrugExtreme".Translate() + ": " +
                (DRSettings.MSDrugExtreme != null ? DRSettings.MSDrugExtremeLabel : None), true, false))
            {
                var list14 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, SetPainExtremeNone)
                };
                if (MSPainDrug.PainDrugs().Count > 0)
                {
                    using (var enumerator = MSPainDrug.PainDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var Drug = enumerator.Current;
                            list14.Add(new FloatMenuOption(Drug.label.CapitalizeFirst(),
                                delegate { SetPainExtreme(Drug.defName); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list14));
            }

            posX += width + (spacing * painspacefactor);
            DrugImage = GetDrugImage(DRSettings.MSDrugExtremeDef);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + (spacing * painspacefactor);
            if (DRSettings.MSDrugExtremeDef != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDrugExtremeDef);
            }

            posX += lineHeight + (spacing * painspacefactor);
            Widgets.TextField(GetMSPainlessDrugRect(lineHeight, textwidth, posX, posY),
                "MSPainless.DrugExtremeLevels".Translate());
            posX = 0f;
            posY += lineHeight + (spacing * 2f);
            var mspainlessDrugRect = GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
            var num = (float) DRSettings.PainReliefWaitPeriod;
            var num2 = 6f;
            var num3 = 48f;
            var flag = false;
            var taggedString = "MSPainless.PainReliefWaitPeriod".Translate() + " : ";
            var num4 = DRSettings.PainReliefWaitPeriod;
            SetPRTime(Widgets.HorizontalSlider(mspainlessDrugRect, num, num2, num3, flag,
                taggedString + num4.ToString() +
                "MSPainless.WaitDays".Translate(ConvertToDaysDesc(DRSettings.PainReliefWaitPeriod)), null, null, 0f));
            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.MethodRelief".Translate() + ": " + (DRSettings.UseReliefBills ? Yes : No), true, false))
            {
                var list15 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetUseRB(true); }),
                    new FloatMenuOption(No, delegate { SetUseRB(false); })
                };
                Find.WindowStack.Add(new FloatMenu(list15));
            }

            var shift = 5;
            posX = 0f;
            posY += lineHeight + sectionspacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width - (spacing * (shift * 1.5f)), posX, posY),
                "MSPainless.Malady".Translate() + ": " + (DRSettings.MSDRHed != null ? DRSettings.MSDRHedLabel : None),
                true, false))
            {
                var list16 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate { SetMaladyNone(null, null, 24, true, 1); })
                };
                if (MSDRUtility.Maladies().Count > 0)
                {
                    var hedlist = DefDatabase<HediffDef>.AllDefsListForReading;
                    var divider = new[]
                    {
                        ';'
                    };
                    var descList = new List<string>();
                    foreach (var text in MSDRUtility.Maladies())
                    {
                        var segments = text.Split(divider);
                        var mod = segments[0];
                        var malady = segments[1];
                        var maladylabel = hedlist.Find(hd => hd.defName == malady).label.CapitalizeFirst();
                        descList.AddDistinct(string.Concat(maladylabel, " (", mod, ");", malady));
                    }

                    if (descList.Count > 0)
                    {
                        descList = (from s in descList
                            orderby s
                            select s).ToList();
                        foreach (var text2 in descList)
                        {
                            var descSegments = text2.Split(divider);
                            var descLabel = descSegments[0];
                            var descMalady = descSegments[1];
                            list16.Add(new FloatMenuOption(descLabel, delegate
                            {
                                SetMalady(descMalady, 1);
                                DRSettings.MSDRHed = descMalady;
                            }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list16));
            }

            posX += width + spacing - (spacing * (shift * 1.5f));
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width - (spacing * (shift * 1.5f)), posX, posY),
                "MSPainless.MaladyDrug".Translate() + ": " +
                (DRSettings.MSDRThg != null ? DRSettings.MSDRThgLabel : None), true, false))
            {
                var list17 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate { SetMaladyDrugNone(1); })
                };
                if (MSDRUtility.MaladyDrugs().Count > 0)
                {
                    using (var enumerator = MSDRUtility.MaladyDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var drug = enumerator.Current;
                            list17.Add(new FloatMenuOption(drug.label.CapitalizeFirst(),
                                delegate { SetMaladyDrug(drug.defName, 1); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list17));
            }

            posX += width + spacing - (spacing * (shift * 1.5f));
            DrugImage = GetDrugImage(DRSettings.MSDRThgDef);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + spacing;
            if (DRSettings.MSDRThgDef != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDRThgDef);
            }

            posX += lineHeight + spacing;
            if (DRSettings.MSDRHedDef != null && Widgets.ButtonText(
                GetMSPainlessDrugRect(lineHeight, lineHeight + 2f, posX, posY), "MSPainless.Set".Translate(), true,
                false))
            {
                var list18 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null),
                    new FloatMenuOption("MSPainless.Set".Translate(), delegate
                    {
                        if (DRSettings.MSDRHed != null)
                        {
                            SetMaladySet(1);
                        }
                    })
                };
                Find.WindowStack.Add(new FloatMenu(list18));
            }

            posX += lineHeight + (spacing * 2f);
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, lineHeight + 20f, posX, posY),
                "MSPainless.Clear".Translate(), true, false))
            {
                var list19 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null),
                    new FloatMenuOption("MSPainless.Clear".Translate(), delegate { ClearMaladyOne(1); }),
                    new FloatMenuOption("MSPainless.ClearAll".Translate(), delegate { ClearMaladyAll(1); })
                };
                Find.WindowStack.Add(new FloatMenu(list19));
            }

            posX = 0f;
            posY += lineHeight + (spacing * 2f);
            var mspainlessDrugRect2 = GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
            var num5 = (float) DRSettings.DRWaitPeriod;
            var num6 = 6f;
            var num7 = 120f;
            var flag2 = false;
            var taggedString2 = "MSPainless.DRWaitPeriod".Translate() + " : ";
            num4 = DRSettings.DRWaitPeriod;
            SetDRTime(Widgets.HorizontalSlider(mspainlessDrugRect2, num5, num6, num7, flag2,
                taggedString2 + num4.ToString() +
                "MSPainless.WaitDays".Translate(ConvertToDaysDesc(DRSettings.DRWaitPeriod)), null, null, 0f));
            posX = 0f;
            posY += lineHeight + spacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.MethodResponse".Translate() + ": " + (DRSettings.UseDRBills ? Yes : No), true, false))
            {
                var list20 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(Yes, delegate { SetUseDB(true, 1); }),
                    new FloatMenuOption(No, delegate { SetUseDB(false, 1); })
                };
                Find.WindowStack.Add(new FloatMenu(list20));
            }

            posX = 0f;
            posY += lineHeight + sectionspacing;
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width - (spacing * (shift * 1.5f)), posX, posY),
                "MSPainless.Malady".Translate() + ": " +
                (DRSettings.MSDRHed2 != null ? DRSettings.MSDRHedLabel2 : None), true, false))
            {
                var list21 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate { SetMaladyNone(null, null, 24, true, 2); })
                };
                if (MSDRUtility.Maladies().Count > 0)
                {
                    var hedlist2 = DefDatabase<HediffDef>.AllDefsListForReading;
                    var divider2 = new[]
                    {
                        ';'
                    };
                    var descList2 = new List<string>();
                    foreach (var text3 in MSDRUtility.Maladies())
                    {
                        var segments2 = text3.Split(divider2);
                        var mod2 = segments2[0];
                        var malady = segments2[1];
                        var maladylabel2 = hedlist2.Find(hd => hd.defName == malady).label.CapitalizeFirst();
                        descList2.AddDistinct(string.Concat(maladylabel2, " (", mod2, ");", malady));
                    }

                    if (descList2.Count > 0)
                    {
                        descList2 = (from s in descList2
                            orderby s
                            select s).ToList();
                        foreach (var text4 in descList2)
                        {
                            var descSegments2 = text4.Split(divider2);
                            var descLabel2 = descSegments2[0];
                            var descMalady = descSegments2[1];
                            list21.Add(new FloatMenuOption(descLabel2, delegate
                            {
                                SetMalady(descMalady, 2);
                                DRSettings.MSDRHed2 = descMalady;
                            }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list21));
            }

            posX += width + spacing - (spacing * (shift * 1.5f));
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width - (spacing * (shift * 1.5f)), posX, posY),
                "MSPainless.MaladyDrug".Translate() + ": " +
                (DRSettings.MSDRThg2 != null ? DRSettings.MSDRThgLabel2 : None), true, false))
            {
                var list22 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, delegate { SetMaladyDrugNone(2); })
                };
                if (MSDRUtility.MaladyDrugs().Count > 0)
                {
                    using (var enumerator = MSDRUtility.MaladyDrugs().GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            var drug = enumerator.Current;
                            list22.Add(new FloatMenuOption(drug.label.CapitalizeFirst(),
                                delegate { SetMaladyDrug(drug.defName, 2); }));
                        }
                    }
                }

                Find.WindowStack.Add(new FloatMenu(list22));
            }

            posX += width + spacing - (spacing * (shift * 1.5f));
            DrugImage = GetDrugImage(DRSettings.MSDRThgDef2);
            Widgets.ButtonImageFitted(GetMSPainlessDrugRect(lineHeight, lineHeight, posX, posY), DrugImage);
            posX += lineHeight + spacing;
            if (DRSettings.MSDRThgDef2 != null)
            {
                Widgets.InfoCardButton(posX, posY, DRSettings.MSDRThgDef2);
            }

            posX += lineHeight + spacing;
            if (DRSettings.MSDRHedDef2 != null && Widgets.ButtonText(
                GetMSPainlessDrugRect(lineHeight, lineHeight + 2f, posX, posY), "MSPainless.Set".Translate(), true,
                false))
            {
                var list23 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null),
                    new FloatMenuOption("MSPainless.Set".Translate(), delegate
                    {
                        if (DRSettings.MSDRHed2 != null)
                        {
                            SetMaladySet(2);
                        }
                    })
                };
                Find.WindowStack.Add(new FloatMenu(list23));
            }

            posX += lineHeight + (spacing * 2f);
            if (Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, lineHeight + 20f, posX, posY),
                "MSPainless.Clear".Translate(), true, false))
            {
                var list24 = new List<FloatMenuOption>
                {
                    new FloatMenuOption(None, null),
                    new FloatMenuOption("MSPainless.Clear".Translate(), delegate { ClearMaladyOne(2); }),
                    new FloatMenuOption("MSPainless.ClearAll".Translate(), delegate { ClearMaladyAll(2); })
                };
                Find.WindowStack.Add(new FloatMenu(list24));
            }

            posX = 0f;
            posY += lineHeight + (spacing * 2f);
            var mspainlessDrugRect3 = GetMSPainlessDrugRect(lineHeight, width * 2f, posX, posY);
            var num8 = (float) DRSettings.DRWaitPeriod2;
            var num9 = 6f;
            var num10 = 120f;
            var flag3 = false;
            var taggedString3 = "MSPainless.DRWaitPeriod".Translate() + " : ";
            num4 = DRSettings.DRWaitPeriod2;
            SetDRTime2(Widgets.HorizontalSlider(mspainlessDrugRect3, num8, num9, num10, flag3,
                taggedString3 + num4.ToString() +
                "MSPainless.WaitDays".Translate(ConvertToDaysDesc(DRSettings.DRWaitPeriod2)), null, null, 0f));
            posX = 0f;
            posY += lineHeight + spacing;
            if (!Widgets.ButtonText(GetMSPainlessDrugRect(lineHeight, width, posX, posY),
                "MSPainless.MethodResponse".Translate() + ": " + (DRSettings.UseDRBills2 ? Yes : No), true, false))
            {
                return;
            }

            var list25 = new List<FloatMenuOption>
            {
                new FloatMenuOption(Yes, delegate { SetUseDB(true, 2); })
            };
            Find.WindowStack.Add(new FloatMenu(list25));
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00003ED4 File Offset: 0x000020D4
        private static string ConvertToDaysDesc(int hours)
        {
            return (hours / 24f).ToString("F2");
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00003EF6 File Offset: 0x000020F6
        private static void SetDoIfImm(bool b)
        {
            DRSettings.DoIfImmune = b;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00003EFE File Offset: 0x000020FE
        private static void SetBillsHPain(bool b)
        {
            DRSettings.BillsHighPain = b;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00003F06 File Offset: 0x00002106
        private static void SetDRTime(float t)
        {
            DRSettings.DRWaitPeriod = (int) t;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00003F0F File Offset: 0x0000210F
        private static void SetDRTime2(float t)
        {
            DRSettings.DRWaitPeriod2 = (int) t;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00003F18 File Offset: 0x00002118
        private static void SetPRTime(float t)
        {
            DRSettings.PainReliefWaitPeriod = (int) t;
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00003F21 File Offset: 0x00002121
        private static void SetUseDB(bool b, int num)
        {
            DRSettings.UseDRBills = b;
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00003F29 File Offset: 0x00002129
        private static void SetUseRB(bool b)
        {
            DRSettings.UseReliefBills = b;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00003F31 File Offset: 0x00002131
        private static void SetShowNDR(bool b)
        {
            DRSettings.ShowResponseMsg = b;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00003F39 File Offset: 0x00002139
        private static void SetShowNPR(bool b)
        {
            DRSettings.ShowReliefMsg = b;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00003F41 File Offset: 0x00002141
        private static void SetShowR(bool b)
        {
            DRSettings.ShowResearched = b;
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00003F49 File Offset: 0x00002149
        private static void SetUseNM(bool b)
        {
            DRSettings.UseNonMedical = b;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00003F51 File Offset: 0x00002151
        private static void SetDoPrisoners(bool b)
        {
            DRSettings.DoIfPrisoner = b;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00003F59 File Offset: 0x00002159
        private static void SetDoDRPrisoners(bool b)
        {
            DRSettings.DoDRIfPrisoner = b;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00003F61 File Offset: 0x00002161
        private static void SetUseDR(bool b)
        {
            DRSettings.UseDrugResponse = b;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00003F69 File Offset: 0x00002169
        private static void SetUsePR(bool b)
        {
            DRSettings.UsePainManagement = b;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00003F74 File Offset: 0x00002174
        private static void ClearMaladyOne(int num)
        {
            if (num == 2)
            {
                MSDRUtility.ClearDRValues(DRSettings.MSDRHed2, false, DRSettings.MSDRValues2, out var newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
                DRSettings.MSDRThg2 = null;
                DRSettings.DRWaitPeriod2 = 24;
                DRSettings.UseDRBills2 = true;
                return;
            }

            MSDRUtility.ClearDRValues(DRSettings.MSDRHed, false, DRSettings.MSDRValues, out var newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
            DRSettings.MSDRThg = null;
            DRSettings.DRWaitPeriod = 24;
            DRSettings.UseDRBills = true;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00003FDC File Offset: 0x000021DC
        private static void ClearMaladyAll(int num)
        {
            if (num == 2)
            {
                MSDRUtility.ClearDRValues(DRSettings.MSDRHed2, true, DRSettings.MSDRValues2, out var newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
                DRSettings.MSDRHed2 = null;
                DRSettings.MSDRThg2 = null;
                DRSettings.DRWaitPeriod2 = 24;
                DRSettings.UseDRBills2 = true;
                return;
            }

            MSDRUtility.ClearDRValues(DRSettings.MSDRHed, true, DRSettings.MSDRValues, out var newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
            DRSettings.MSDRHed = null;
            DRSettings.MSDRThg = null;
            DRSettings.DRWaitPeriod = 24;
            DRSettings.UseDRBills = true;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00004050 File Offset: 0x00002250
        private static void SetMaladySet(int num)
        {
            if (num == 2)
            {
                MSDRUtility.SetDRValues(DRSettings.MSDRHed2, DRSettings.MSDRThg2, DRSettings.DRWaitPeriod2,
                    DRSettings.UseDRBills2, num, DRSettings.MSDRValues2, out var newMSDRValues);
                DRSettings.MSDRValues2 = newMSDRValues;
                return;
            }

            MSDRUtility.SetDRValues(DRSettings.MSDRHed, DRSettings.MSDRThg, DRSettings.DRWaitPeriod,
                DRSettings.UseDRBills, num, DRSettings.MSDRValues, out var newMSDRValues2);
            DRSettings.MSDRValues = newMSDRValues2;
        }

        // Token: 0x06000016 RID: 22 RVA: 0x000040B0 File Offset: 0x000022B0
        private static void SetMaladyDrug(string d, int num)
        {
            if (num == 2)
            {
                DRSettings.MSDRThg2 = d;
                return;
            }

            DRSettings.MSDRThg = d;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000040C4 File Offset: 0x000022C4
        private static void SetMaladyDrugNone(int num)
        {
            if (num == 2)
            {
                DRSettings.MSDRThg2 = null;
                DRSettings.DRWaitPeriod2 = 24;
                SetValueDRBills(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
                return;
            }

            DRSettings.MSDRThg = null;
            DRSettings.DRWaitPeriod = 24;
            SetValueDRBills(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00004110 File Offset: 0x00002310
        private static void SetMalady(string m, int num)
        {
            if (num <= 0)
            {
                return;
            }

            DRSettings.MSDRHed2 = m;
            SetValueDRDrug(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
            SetValueDRTime(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
            SetValueDRBills(DRSettings.MSDRHed2, DRSettings.MSDRValues2, 2);
            DRSettings.MSDRHed = m;
            SetValueDRDrug(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
            SetValueDRTime(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
            SetValueDRBills(DRSettings.MSDRHed, DRSettings.MSDRValues, 1);
        }

        // Token: 0x06000019 RID: 25 RVA: 0x0000418D File Offset: 0x0000238D
        private static void SetMaladyNone(string h, string d, int t, bool b, int num)
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
        private static void SetPainMinorNone()
        {
            DRSettings.MSDrugMinor = null;
        }

        // Token: 0x0600001B RID: 27 RVA: 0x000041CD File Offset: 0x000023CD
        private static void SetPainSeriousNone()
        {
            DRSettings.MSDrugSerious = null;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x000041D5 File Offset: 0x000023D5
        private static void SetPainIntenseNone()
        {
            DRSettings.MSDrugIntense = null;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x000041DD File Offset: 0x000023DD
        private static void SetPainExtremeNone()
        {
            DRSettings.MSDrugExtreme = null;
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000041E5 File Offset: 0x000023E5
        private static void SetPainMinor(string defname)
        {
            DRSettings.MSDrugMinor = defname;
            DRSettings.SetCachedValue(defname, 1);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000041F4 File Offset: 0x000023F4
        private static void SetPainSerious(string defname)
        {
            DRSettings.MSDrugSerious = defname;
            DRSettings.SetCachedValue(defname, 2);
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00004203 File Offset: 0x00002403
        private static void SetPainIntense(string defname)
        {
            DRSettings.MSDrugIntense = defname;
            DRSettings.SetCachedValue(defname, 3);
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00004212 File Offset: 0x00002412
        private static void SetPainExtreme(string defname)
        {
            DRSettings.MSDrugExtreme = defname;
            DRSettings.SetCachedValue(defname, 4);
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00004224 File Offset: 0x00002424
        private static void SetValueDRDrug(string hed, List<string> master, int num)
        {
            string setto = null;
            if (hed != null && master != null && master.Count > 0)
            {
                foreach (var value in master)
                {
                    var hdef = MSDRUtility.HValuePart(value);
                    var numof = MSDRUtility.NumValuePart(value);
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
        private static void SetValueDRTime(string hed, List<string> master, int num)
        {
            var setto = 24;
            if (hed != null && master != null && master.Count > 0)
            {
                foreach (var value in master)
                {
                    var hdef = MSDRUtility.HValuePart(value);
                    var numof = MSDRUtility.NumValuePart(value);
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
        private static void SetValueDRBills(string hed, List<string> master, int num)
        {
            var setto = true;
            if (hed != null && master != null && master.Count > 0)
            {
                foreach (var value in master)
                {
                    var hdef = MSDRUtility.HValuePart(value);
                    var numof = MSDRUtility.NumValuePart(value);
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
        private static Texture2D GetDrugImage(ThingDef def)
        {
            var text = "UI/NullDrug";
            var NullIconPath = "UI/NullIcon";
            var NullDrug = ContentFinder<Texture2D>.Get(text, false);
            var NullIcon = ContentFinder<Texture2D>.Get(NullIconPath, false);
            if (def == null)
            {
                return NullDrug;
            }

            var icon = def.uiIcon;
            if (icon != null)
            {
                return icon;
            }

            var graphicData = def.graphicData;
            if (graphicData?.texPath == null)
            {
                return NullIcon;
            }

            var graphicData2 = def.graphicData;
            var text2 = graphicData2?.texPath;
            var texturePath = text2;
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

        // Token: 0x06000026 RID: 38 RVA: 0x0000449C File Offset: 0x0000269C
        private static Rect GetMSPainlessDrugRect(float height, float width, float posX, float posY)
        {
            return new Rect(posX, posY, width, height);
        }
    }
}