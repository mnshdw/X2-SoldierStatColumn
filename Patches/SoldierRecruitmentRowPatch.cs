using System;
using Artitas;
using HarmonyLib;
using UnityEngine;
using Xenonauts.Strategy.UI;
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

                int total = TotalSort.Compute(actor);

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
