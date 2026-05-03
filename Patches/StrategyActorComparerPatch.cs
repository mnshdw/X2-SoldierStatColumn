using Artitas;
using Common.Utilities.Comparers;
using HarmonyLib;
using Xenonauts.Common.Util.Comparers;

namespace SoldierTotalColumn.Patches
{
    [HarmonyPatch(typeof(StrategyActorComparer), "Compare")]
    public static class StrategyActorComparerPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Entity x, Entity y, ref int __result)
        {
            if (!TotalSort.Active)
                return true;

            if (object.Equals(x, y))
            {
                __result = 0;
                return false;
            }
            if ((object)x == null)
            {
                __result = 1;
                return false;
            }
            if ((object)y == null)
            {
                __result = -1;
                return false;
            }

            int cmp = TotalSort.Order * TotalSort.Compute(x).CompareTo(TotalSort.Compute(y));
            if (cmp == 0)
                cmp = TotalSort.Order * SortOrderAlphaNumericComparer<Entity>.Default.Compare(x, y);
            __result = cmp;
            return false;
        }
    }
}
