using System.Collections.Concurrent;
using HarmonyLib;

namespace TowerFactory
{
    /// <summary>
    /// 矩阵研究须在主线程执行：AddTechHash 会触发科技解锁 UI，且 while 连升依赖同步更新 currentTech / 等级。
    /// 塔厂在并行 PlanetTransport.GameTick 里只入队，本帧 GameMain.Update 执行。
    /// </summary>
    public static class TechHashMainThreadPatch
    {
        private readonly struct PendingTechCraft
        {
            public readonly PlanetFactory factory;
            public readonly StationComponent station;
            public readonly int buildingCount;

            public PendingTechCraft(PlanetFactory factory, StationComponent station, int buildingCount)
            {
                this.factory = factory;
                this.station = station;
                this.buildingCount = buildingCount;
            }
        }

        private static readonly ConcurrentQueue<PendingTechCraft> pendingCrafts = new ConcurrentQueue<PendingTechCraft>();

        internal static void EnqueueTechCraft(PlanetFactory factory, StationComponent station, int buildingCount)
        {
            if (factory == null || station == null || buildingCount <= 0)
            {
                return;
            }
            pendingCrafts.Enqueue(new PendingTechCraft(factory, station, buildingCount));
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameMain), "Update")]
        [CatAPI.Guard("381b98d1af328905")]
        public static void GameMain_Update_Postfix()
        {
            while (pendingCrafts.TryDequeue(out PendingTechCraft craft))
            {
                StationProductionPatch.RunTechResearchOnMainThread(craft.factory, craft.station, craft.buildingCount);
            }
        }
    }
}
