using System;
using HarmonyLib;
using Strategy.UI.Elements.Soldiers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    [HarmonyPatch(typeof(SoldiersRecruitElement), "OnCreate")]
    public static class SoldiersRecruitElementPatch
    {
        public const string HeaderChildName = "SoldierTotalColumn_Header";
        public const string HeaderLabel = "TOT";

        [HarmonyPostfix]
        public static void Postfix(SoldiersRecruitElement __instance)
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
                Log.Warn($"{LogPrefix} header postfix failed: {ex}");
            }
        }
    }
}
