using Artitas;
using Xenonauts.Common.Util;
using Xenonauts.Strategy.Utils;

namespace SoldierTotalColumn.Patches
{
    internal static class TotalSort
    {
        public static bool Active;
        public static int Order = -1;

        public static int Compute(Entity actor)
        {
            return (int)actor.GetWoundUnmodifiedHitPoints().ToStrategyBoundedRange().Value
                + (int)actor.TimeUnits().ToStrategyBoundedRange().Value
                + (int)actor.Accuracy().ToStrategyBoundedRange().Value
                + (int)actor.Strength().ToStrategyBoundedRange().Value
                + (int)actor.Reflexes().ToStrategyBoundedRange().Value
                + (int)actor.Bravery().ToStrategyBoundedRange().Value;
        }
    }
}
