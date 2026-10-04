using HarmonyLib;

namespace TowerFactory
{
    /// <summary>
    /// 科技解锁时赠送行星内物流运输塔 (2103)，不改动科技树的 UnlockRecipes / UnlockFunctions。
    /// 五项新手相关科技各送 3 座；其余科技首次解锁各送 1 座。每科技每存档一次（featureKey）。
    /// Tech IDs: dsp-wiki.com/Modding:Tech_IDs
    /// </summary>
    [HarmonyPatch(typeof(GameHistoryData))]
    public static class TechStationGiftPatch
    {
        private const int PlanetaryLogisticsStationItemId = 2103;
        private const int DefaultStationGiftCount = 1;
        private const int BonusStationGiftCount = 3;
        private const int FeatureKeyBase = 99001000;

        private static readonly int[] BonusGiftTechIds =
        {
            1001, // Electromagnetics / 电磁学
            1201, // Basic assembling process / 基础制造
            1002, // Electromagnetic matrix / 电磁矩阵
            1401, // Automatic metallurgy / 自动化冶金
            1601, // Basic logistics system / 基础物流系统
        };

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameHistoryData.NotifyTechUnlock))]
        public static void NotifyTechUnlock_Postfix(GameHistoryData __instance, int _techId, int _level, bool _unlockedDirect)
        {
            if (__instance == null)
            {
                return;
            }
            if (!__instance.techStates.TryGetValue(_techId, out TechState state) || !state.unlocked)
            {
                return;
            }
            int featureKey = FeatureKeyBase + _techId;
            if (__instance.HasFeatureKey(featureKey))
            {
                return;
            }
            int count = IsBonusGiftTech(_techId) ? BonusStationGiftCount : DefaultStationGiftCount;
            __instance.RegFeatureKey(featureKey);
            __instance.GainTechAwards(PlanetaryLogisticsStationItemId, count);
            string techLabel = LDB.techs.Select(_techId)?.name ?? _techId.ToString();
            TowerFactory.Log.LogInfo($"科技 {techLabel} 已解锁：赠送 {count} 个行星内物流运输塔（未解锁物流科技配方）");
        }

        private static bool IsBonusGiftTech(int techId)
        {
            foreach (int id in BonusGiftTechIds)
            {
                if (id == techId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
