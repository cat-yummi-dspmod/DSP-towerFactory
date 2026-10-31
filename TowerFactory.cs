using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TowerFactory
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(CatAPI.HarnessPlugin.PluginGuid)]
    public class TowerFactory : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.yummi.dsp.towerfactory";
        public const string PLUGIN_NAME = "塔厂 (Tower Factory)";
        public const string PLUGIN_VERSION = "1.4.2";

        private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);
        private static readonly CatAPI.PatchOptions HarnessOptions = new CatAPI.PatchOptions { ModName = "TowerFactory" };
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            CatAPI.Harness.PatchAll(harmony, typeof(StationProductionPatch), HarnessOptions);
            CatAPI.Harness.PatchAll(harmony, typeof(UIStationWindowPatch), HarnessOptions);
            CatAPI.Harness.PatchAll(harmony, typeof(TechStationGiftPatch), HarnessOptions);
            CatAPI.Harness.PatchAll(harmony, typeof(DysonSwarmCapPatch), HarnessOptions);
            CatAPI.Harness.PatchAll(harmony, typeof(StationTowerRevalidatePatch), HarnessOptions);
            CatAPI.Harness.PatchAll(harmony, typeof(TechHashMainThreadPatch), HarnessOptions);

            Log.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} 已加载");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
