using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TowerFactory
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(YummiHarness.HarnessPlugin.PluginGuid)]
    public class TowerFactory : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.yummi.dsp.towerfactory";
        public const string PLUGIN_NAME = "塔厂 (Tower Factory)";
        public const string PLUGIN_VERSION = "1.3.0";

        private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);
        private static readonly YummiHarness.PatchOptions HarnessOptions = new YummiHarness.PatchOptions { ModName = "TowerFactory" };
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            YummiHarness.Harness.PatchAll(harmony, typeof(StationProductionPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(UIStationWindowPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(TechStationGiftPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(DysonSwarmCapPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(StationTowerRevalidatePatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(TechHashMainThreadPatch), HarnessOptions);

            Log.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} 已加载");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
