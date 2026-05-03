using System.Reflection;
using Artitas;
using HarmonyLib;
using Strategy.UI.Elements.Soldiers;
using Xenonauts.Common.Util.Comparers;

namespace SoldierTotalColumn.Patches
{
    [HarmonyPatch]
    public static class FieldComparerSetSelectedSortColumnPatch
    {
        public static MethodBase TargetMethod()
        {
            return typeof(FieldComparer<SortField, Entity>).GetMethod("SetSelectedSortColumn");
        }

        [HarmonyPrefix]
        public static void Prefix()
        {
            TotalSort.Active = false;
        }
    }
}
