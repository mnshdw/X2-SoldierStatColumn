using System;
using Artitas;
using HarmonyLib;
using UnityEngine;
using Xenonauts.Strategy.UI;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    [HarmonyPatch(typeof(SoldierInfoRowController), "OnSetTarget")]
    public static class SoldierInfoRowPatch
    {
        public const string TotalChildName = "SoldierTotalColumn_Total";

        [HarmonyPostfix]
        public static void Postfix(SoldierInfoRowController __instance)
        {
            try
            {
                var totalText = StatColumnUtils.CloneStatColumn(
                    __instance.braveryText,
                    __instance.reflexesText,
                    TotalChildName,
                    Color.white
                );
                if (totalText == null)
                    return;

                var target = __instance.Target;
                Entity? actor = target?.X;
                if (actor == null)
                {
                    totalText.text = string.Empty;
                    return;
                }

                int total = TotalSort.Compute(actor);

                totalText.text = total.ToString("N0");
                TotalColorRegistry.Update("manage", totalText, total);
            }
            catch (Exception ex)
            {
                Log.Warn($"{LogPrefix} manage row postfix failed: {ex}");
            }
        }
    }
}
