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
    [HarmonyPatch(typeof(SoldierRecruitmentRowController), "OnSetTarget")]
    public static class SoldierRecruitmentRowPatch
    {
        public const string TotalChildName = "SoldierTotalColumn_Total";

        [HarmonyPostfix]
        public static void Postfix(SoldierRecruitmentRowController __instance)
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

                Entity? actor = __instance.Target;
                if (actor == null)
                {
                    totalText.text = string.Empty;
                    return;
                }

                int total =
                    (int)actor.GetWoundUnmodifiedHitPoints().ToStrategyBoundedRange().Value
                    + (int)actor.TimeUnits().ToStrategyBoundedRange().Value
                    + (int)actor.Accuracy().ToStrategyBoundedRange().Value
                    + (int)actor.Strength().ToStrategyBoundedRange().Value
                    + (int)actor.Reflexes().ToStrategyBoundedRange().Value
                    + (int)actor.Bravery().ToStrategyBoundedRange().Value;

                totalText.text = total.ToString("N0");
                TotalColorRegistry.Update("recruit", totalText, total);
            }
            catch (Exception ex)
            {
                Log.Warn($"{LogPrefix} row postfix failed: {ex}");
            }
        }
    }
}
