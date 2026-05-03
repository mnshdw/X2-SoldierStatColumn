using System;
using Artitas;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Xenonauts.Common.Util;
using Xenonauts.Strategy.UI;
using Xenonauts.Strategy.Utils;
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

                int total =
                    (int)actor.UnmodifiedHitPoints().ToStrategyBoundedRange().Value
                    + (int)actor.TimeUnits().ToStrategyBoundedRange().Value
                    + (int)actor.Accuracy().ToStrategyBoundedRange().Value
                    + (int)actor.Strength().ToStrategyBoundedRange().Value
                    + (int)actor.Reflexes().ToStrategyBoundedRange().Value
                    + (int)actor.Bravery().ToStrategyBoundedRange().Value;

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
