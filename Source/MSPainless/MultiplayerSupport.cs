using System;
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
		// Token: 0x06000089 RID: 137 RVA: 0x00007314 File Offset: 0x00005514
		static MultiplayerSupport()
		{
			if (!MP.enabled)
			{
				return;
			}
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainMinor", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainSerious", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainIntense", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainExtreme", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainMinorNone", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainSeriousNone", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainIntenseNone", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetPainExtremeNone", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMalady", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladyDrug", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladyDrugNone", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetMaladySet", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "ClearMaladyOne", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "ClearMaladyAll", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUsePR", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseDR", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoPrisoners", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoDRPrisoners", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowNPR", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowNDR", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseNM", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetShowR", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetBillsHPain", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetDoIfImm", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseRB", null);
			MP.RegisterSyncMethod(typeof(MainTabWindow_DrugResponse), "SetUseDB", null);
			MP.RegisterSyncWorker<DRSettings>(new SyncWorkerDelegate<DRSettings>(MultiplayerSupport.SyncWriterForDRSettings), null, false, false);
			MultiplayerSupport.DRSettingsFields = new ISyncField[]
			{
				MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod").SetBufferChanges(),
				MP.RegisterSyncField(typeof(DRSettings), "DRWaitPeriod2").SetBufferChanges(),
				MP.RegisterSyncField(typeof(DRSettings), "PainReliefWaitPeriod").SetBufferChanges()
			};
			MultiplayerSupport.harmony.Patch(AccessTools.Method(typeof(MainTabWindow_DrugResponse), "DoWindowContents", null, null), new HarmonyMethod(typeof(MultiplayerSupport), "WatchBegin", null), new HarmonyMethod(typeof(MultiplayerSupport), "WatchEnd", null), null, null);
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
			MultiplayerSupport.DRSettingsFields.Watch(null);
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
			for (int i = 0; i < fields.Length; i++)
			{
				fields[i].Watch(target, null);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000769B File Offset: 0x0000589B
		private static void FixRNG(MethodInfo method)
		{
			MultiplayerSupport.harmony.Patch(method, new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPre", null), new HarmonyMethod(typeof(MultiplayerSupport), "FixRNGPos", null), null, null);
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

		// Token: 0x04000023 RID: 35
		private static Harmony harmony = new Harmony("rimworld.pelador.drugresponse.multiplayersupport");

		// Token: 0x04000024 RID: 36
		private static readonly ISyncField[] DRSettingsFields;
	}
}
