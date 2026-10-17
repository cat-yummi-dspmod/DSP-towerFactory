using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace TowerFactory
{
    public enum SlotPreset
    {
        BigTower,
        Off
    }

    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInDependency(YummiHarness.HarnessPlugin.PluginGuid)]
    [BepInDependency(StationAssistant.Plugin.PluginGuid)]
    // 软依赖只为保证加载顺序：若已装 BigTower / SafeBigTower，以它们为准，塔厂不再重复设置。
    [BepInDependency(BigTowerGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(SafeBigTowerGuid, BepInDependency.DependencyFlags.SoftDependency)]
    public class TowerFactory : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.yummi.dsp.towerfactory";
        public const string PLUGIN_NAME = "塔厂 (Tower Factory)";
        public const string PLUGIN_VERSION = "1.4.0";
        internal const string BigTowerGuid = "com.yummi.dsp.bigtower";
        internal const string SafeBigTowerGuid = "com.yummi.dsp.safebigtower";

        private static readonly Harmony harmony = new Harmony(PLUGIN_GUID);
        private static readonly YummiHarness.PatchOptions HarnessOptions = new YummiHarness.PatchOptions { ModName = "TowerFactory" };
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            ConfigEntry<SlotPreset> preset = Config.Bind("Slots", "SlotPreset", SlotPreset.BigTower,
                "BigTower = apply the same Station Assistant preset as the BigTower mod (interstellar 9 / planetary 8, expand on build/open). Off = leave Station Assistant settings alone.\n" +
                "BigTower = 自动套用与 BigTower 相同的 Station Assistant 设置（星际 9 / 行星 8 格，建造/打开时扩容）。Off = 不改任何设置。");
            ApplySlotPreset(preset.Value);

            YummiHarness.Harness.PatchAll(harmony, typeof(StationProductionPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(UIStationWindowPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(TechStationGiftPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(DysonSwarmCapPatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(StationTowerRevalidatePatch), HarnessOptions);
            YummiHarness.Harness.PatchAll(harmony, typeof(TechHashMainThreadPatch), HarnessOptions);

            Log.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} 已加载");
        }

        private void ApplySlotPreset(SlotPreset preset)
        {
            if (preset == SlotPreset.Off)
            {
                return;
            }
            if (YummiHarness.Harness.IsPluginLoaded(BigTowerGuid) || YummiHarness.Harness.IsPluginLoaded(SafeBigTowerGuid))
            {
                Log.LogInfo("已安装 BigTower / SafeBigTower，槽位预设以它们为准，塔厂跳过 SlotPreset。");
                return;
            }
            StationAssistant.Treat.AutoStellarSlots(9);
            StationAssistant.Treat.AutoPlanetarySlots(8);
            StationAssistant.Treat.ExpandOnBuild(true);
            StationAssistant.Treat.ExpandOnOpen(true);
            StationAssistant.Treat.ManualSlotButton(false);
            StationAssistant.Treat.StationPanelScroll(false);
            StationAssistant.Treat.ControlPanelScroll(true);
            StationAssistant.Treat.PageSize(6);
            Log.LogInfo("SlotPreset = BigTower：已要求 Station Assistant 按 9/8 格、建造/打开时扩容。");
        }

        private void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}
