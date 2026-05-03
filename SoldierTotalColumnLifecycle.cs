using System;
using System.Collections.Generic;
using Artitas;
using Common.Content;
using Common.Modding;
using HarmonyLib;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn
{
    public class SoldierTotalColumnLifecycle : IModLifecycle
    {
        public void Create(Mod mod, Harmony patcher)
        {
            Log.Info($"{LogPrefix} Create - mod loaded");
        }

        public void Destroy()
        {
            Log.Info($"{LogPrefix} Destroy - mod unloaded");
        }

        public void OnWorldCreate(IModLifecycle.Section section, WeakReference<World> world) { }

        public void OnWorldDispose(IModLifecycle.Section section, WeakReference<World> world) { }

        public IEnumerable<Descriptor> GetRequiredAssets(IModLifecycle.Section section)
        {
            return [];
        }
    }
}
