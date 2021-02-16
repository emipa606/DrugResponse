using System.Reflection;
using HarmonyLib;
using Multiplayer.API;
using Verse;

namespace MSPainless
{
    // Token: 0x0200000C RID: 12
    [StaticConstructorOnStartup]
    internal static class MultiplayerSupport
    {
        // Token: 0x04000023 RID: 35
        private static readonly Harmony harmony = new Harmony("rimworld.pelador.drugresponse.multiplayersupport");

        // Token: 0x04000024 RID: 36
        private static readonly ISyncField[] DRSettingsFields;

        // Token: 0x06000089 RID: 137 RVA: 0x00007314 File Offset: 0x00005514
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
            DRSettingsFields = new[]
            {
                MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod").SetBufferChanges(),
                MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod2").SetBufferChanges(),
                MP.RegisterSyncField(typeof(DRSettings), "PainReliefWaitPeriod").SetBufferChanges()
            };
            harmony.Patch(AccessTools.Method(typeof(MainTabWindow_DrugResponse), "DoWindowContents"),
                new HarmonyMethod(typeof(MultiplayerSupport), "WatchBegin"),
                new HarmonyMethod(typeof(MultiplayerSupport), "WatchEnd"));
        }

        // Token: 0x0600008A RID: 138 RVA: 0x00007634 File Offset: 0x00005834
        private static void SyncWriterForDRSettings(SyncWorker sync, ref DRSettings dRSettings)
        {
            if (!sync.isWriting)
            {
                dRSettings = Find.World.GetComponent<DRSettings>();
            }
        }

        // Token: 0x0600008B RID: 139 RVA: 0x0000764A File Offset: 0x0000584A
        private static void WatchBegin()
        {
            if (!MP.IsInMultiplayer)
            {
                return;
            }

            MP.WatchBegin();
            DRSettingsFields.Watch();
        }

        // Token: 0x0600008C RID: 140 RVA: 0x00007664 File Offset: 0x00005864
        private static void WatchEnd()
        {
            if (!MP.IsInMultiplayer)
            {
                return;
            }

            MP.WatchEnd();
        }

        // Token: 0x0600008D RID: 141 RVA: 0x00007674 File Offset: 0x00005874
        private static void Watch(this ISyncField[] fields, object target = null)
        {
            foreach (var field in fields)
            {
                field.Watch(target);
            }
        }

        // Token: 0x0600008E RID: 142 RVA: 0x0000769B File Offset: 0x0000589B
        private static void FixRNG(MethodInfo method)
        {
            harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre"),
                new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos"));
        }

        // Token: 0x0600008F RID: 143 RVA: 0x000076D5 File Offset: 0x000058D5
        private static void FixRNGPre()
        {
            Rand.PushState(Find.TickManager.TicksAbs);
        }

        // Token: 0x06000090 RID: 144 RVA: 0x000076E6 File Offset: 0x000058E6
        private static void FixRNGPos()
        {
            Rand.PopState();
        }
    }
}