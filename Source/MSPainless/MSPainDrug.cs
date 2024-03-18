using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace MSPainless;

internal class MSPainDrug : DefModExtension
{
    public bool ManagesPain;

    private static bool GetManagesPain(ThingDef def)
    {
        var ApplyPain = false;
        if (def.HasModExtension<MSPainDrug>())
        {
            ApplyPain = def.GetModExtension<MSPainDrug>().ManagesPain;
        }

        return ApplyPain;
    }

    internal static List<ThingDef> PainDrugs()
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
            if (!GetManagesPain(chkDef) || !DrugOk(chkDef) || !chkDef.IsIngestible)
            {
                continue;
            }

            var ingestible = chkDef.ingestible;
            if (ingestible?.outcomeDoers == null)
            {
                continue;
            }

            var ingestible2 = chkDef.ingestible;
            var list2 = ingestible2?.outcomeDoers;
            if (list2 == null)
            {
                continue;
            }

            foreach (var ODoer in list2)
            {
                if (ODoer is not IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoerGiveHediff)
                {
                    continue;
                }

                var ODHediffDef = ingestionOutcomeDoerGiveHediff.hediffDef;

                var ODHedStages = ODHediffDef?.stages;
                if (ODHedStages == null)
                {
                    continue;
                }

                foreach (var ODHedStage in ODHedStages)
                {
                    if (!(ODHedStage.painFactor <= 0.8f) && !(ODHedStage.painOffset <= -0.2f))
                    {
                        continue;
                    }

                    if (!DRSettings.ShowResearched)
                    {
                        list.AddDistinct(chkDef);
                        break;
                    }

                    if (IsResearchCompleted(chkDef, recipelist))
                    {
                        list.AddDistinct(chkDef);
                    }

                    break;
                }
            }
        }

        return (from td in list
            orderby td.label
            select td).ToList();
    }

    private static bool DrugOk(ThingDef def)
    {
        if (def?.ingestible == null)
        {
            return false;
        }

        if (DRSettings.UseNonMedical)
        {
            var ingestible = def.ingestible;
            if (ingestible != null)
            {
                return true;
            }
        }
        else
        {
            var ingestible2 = def.ingestible;
            var drugCategory2 = ingestible2 != null ? new DrugCategory?(ingestible2.drugCategory) : null;
            var drugCategory3 = DrugCategory.Medical;
            if ((drugCategory2.GetValueOrDefault() == drugCategory3) & (drugCategory2 != null))
            {
                return true;
            }
        }

        return false;
    }

    internal static bool IsResearchCompleted(ThingDef def, List<RecipeDef> recipes)
    {
        if (Current.ProgramState != ProgramState.Playing)
        {
            return true;
        }

        bool b;
        if (def == null)
        {
            b = false;
        }
        else
        {
            var recipeMaker = def.recipeMaker;
            b = recipeMaker?.researchPrerequisite != null;
        }

        if (b && def.recipeMaker.researchPrerequisite.IsFinished)
        {
            return true;
        }

        if (recipes.Count <= 0)
        {
            return false;
        }

        using var enumerator = recipes.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var recipe = enumerator.Current;
            if (recipe?.products == null)
            {
                continue;
            }

            foreach (var thingDefCountClass in recipe.products)
            {
                if (thingDefCountClass?.thingDef != def)
                {
                    continue;
                }

                if (recipe.researchPrerequisite == null)
                {
                    return true;
                }

                if (recipe.researchPrerequisite.IsFinished)
                {
                    return true;
                }
            }
        }

        return false;
    }
}