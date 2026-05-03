using System;
using HarmonyLib;
using Strategy.UI.Elements.Soldiers;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    [HarmonyPatch(typeof(SoldiersManageElement), "OnCreate")]
    public static class SoldiersManageElementPatch
    {
        public const string HeaderChildName = "SoldierTotalColumn_Header";
        public const string HeaderLabel = "TOT";

        [HarmonyPostfix]
        public static void Postfix(SoldiersManageElement __instance)
        {
            try
            {
                ColumnHeaderClone.Apply(
                    __instance,
                    __instance.columnToggles,
                    HeaderChildName,
                    HeaderLabel
                );
            }
            catch (Exception ex)
            {
                Log.Warn($"{LogPrefix} manage header postfix failed: {ex}");
            }
        }
    }
}
