using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace MSPainless;

[StaticConstructorOnStartup]
internal static class MultiplayerSupport
{
    private static readonly Harmony harmony = new Harmony("rimworld.pelador.drugresponse.multiplayersupport");

    private static readonly ISyncField[] DRSettingsFields;

    static MultiplayerSupport()
    {
        if (!MP.enabled)
        {
            return;
        }

        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainMinor");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainSerious");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainIntense");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainExtreme");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainMinorNone");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainSeriousNone");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainIntenseNone");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainExtremeNone");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMalady");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladyDrug");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladyDrugNone");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladySet");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "ClearMaladyOne");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "ClearMaladyAll");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUsePR");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseDR");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoPrisoners");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoDRPrisoners");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowNPR");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowNDR");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseNM");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowR");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetBillsHPain");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoIfImm");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseRB");
        MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseDB");
        MP.RegisterSyncWorker(new SyncWorkerDelegate<DRSettings>(SyncWriterForDRSettings));
        DRSettingsFields =
        [
            MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod").SetBufferChanges(),
            MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod2").SetBufferChanges(),
            MP.RegisterSyncField(typeof(DRSettings), "PainReliefWaitPeriod").SetBufferChanges()
        ];
        harmony.Patch(AccessTools.Method(typeof(MainTabWindow_DrugResponse), "DoWindowContents"),
            new HarmonyMethod(typeof(MultiplayerSupport), "WatchBegin"),
            new HarmonyMethod(typeof(MultiplayerSupport), "WatchEnd"));
    }

    private static void SyncWriterForDRSettings(SyncWorker sync, ref DRSettings dRSettings)
    {
        if (!sync.isWriting)
        {
            dRSettings = Find.World.GetComponent<DRSettings>();
        }
    }

    private static void WatchBegin()
    {
        if (!MP.IsInMultiplayer)
        {
            return;
        }

        MP.WatchBegin();
        DRSettingsFields.Watch();
    }

    private static void WatchEnd()
    {
        if (!MP.IsInMultiplayer)
        {
            return;
        }

        MP.WatchEnd();
    }

    private static void Watch(this ISyncField[] fields, object target = null)
    {
        foreach (var field in fields)
        {
            field.Watch(target);
        }
    }

    private static void FixRNG(MethodInfo method)
    {
        harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
            new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
    }

    private static void FixRNGPre()
    {
        Rand.PushState(Find.TickManager.TicksAbs);
    }

    private static void FixRNGPos()
    {
        Rand.PopState();
    }
}