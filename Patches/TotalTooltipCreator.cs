using Artitas;
using Artitas.Utils;
using Common.DataStructures;
using Common.Systems;
using Common.UI.Tooltips;
using UnityEngine;

namespace SoldierTotalColumn.Patches
{
    public class TotalTooltipCreator : MonoBehaviour, ITooltipCreator
    {
        public string TooltipText = "Total\nSum of HP + TU + ACC + STR + RFL + BRV.";

        Result ITooltipCreator.CanShowTooltip(World world)
        {
            return Result.Success;
        }

        GameObject ITooltipCreator.AcquireTooltip(World world)
        {
            return CommonTooltips.CreateLabelTooltip(world, TooltipText);
        }

        void ITooltipCreator.GetWorldCorners(Vector3[] fourCornersArray)
        {
            ((RectTransform)transform).GetWorldCorners(fourCornersArray);
        }

        RectAnchorAlignment[] ITooltipCreator.GetPreferredTooltipAnchorAlignments()
        {
            return TooltipSystem.TOOLTIP_ANCHOR_LOOKUP["default"];
        }

        bool ITooltipCreator.IsActive()
        {
            return gameObject.activeInHierarchy;
        }

        public Optional<float> GetTooltipDelayOverride()
        {
            return Optional<float>.None;
        }
    }
}
