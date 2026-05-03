using System;
using System.Collections.Generic;
using Common.UI.Tooltips;
using Common.UI.Tooltips.Creators;
using Common.UI.Unity;
using HarmonyLib;
using Strategy.UI.Elements.Soldiers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    internal static class ColumnHeaderClone
    {
        private static bool _wasActiveAtLastClick;

        public static void Apply(
            object element,
            Dictionary<SortField, GHIToggle>? columnToggles,
            string childName,
            string label
        )
        {
            if (
                columnToggles == null
                || !columnToggles.TryGetValue(SortField.Bravery, out var braveryToggle)
                || braveryToggle == null
            )
            {
                Log.Warn($"{LogPrefix} could not find Bravery column toggle to clone for header");
                return;
            }

            if (
                !columnToggles.TryGetValue(SortField.Reflexes, out var reflexesToggle)
                || reflexesToggle == null
            )
            {
                Log.Warn($"{LogPrefix} could not find Reflexes column toggle for spacing");
                return;
            }

            var parent = braveryToggle.transform.parent;
            if (parent == null)
                return;

            if (parent.Find(childName) != null)
                return;

            TotalSort.Active = false;

            var clone = UnityEngine.Object.Instantiate(braveryToggle.gameObject, parent);
            clone.name = childName;
            clone.transform.SetSiblingIndex(braveryToggle.transform.GetSiblingIndex() + 1);

            if (
                braveryToggle.transform is RectTransform anchorRT
                && reflexesToggle.transform is RectTransform prevRT
                && clone.transform is RectTransform cloneRT
            )
            {
                StatColumnUtils.OffsetAfter(cloneRT, anchorRT, prevRT);
            }

            foreach (var component in clone.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (component == null)
                    continue;
                var typeName = component.GetType().FullName ?? string.Empty;
                if (typeName.EndsWith("TooltipCreator", StringComparison.Ordinal))
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
            }

            foreach (var child in clone.GetComponentsInChildren<Transform>(true))
            {
                StatColumnUtils.StripLocalization(child.gameObject);
            }
            StatColumnUtils.StripLocalization(clone);

            foreach (var text in clone.GetComponentsInChildren<TMP_Text>(true))
            {
                text.text = label;
            }

            AttachTooltip(clone);

            WireSortHandlers(element, clone);

            Log.Info($"{LogPrefix} header clone applied: name={clone.name}");
        }

        private static void WireSortHandlers(object element, GameObject clone)
        {
            var ghiToggle = clone.GetComponent<GHIToggle>();
            if (ghiToggle == null)
            {
                Log.Warn($"{LogPrefix} cloned header has no GHIToggle; sort wiring skipped");
                return;
            }

            ghiToggle.SetIsOnWithoutNotify(false);
            ghiToggle.onValueChanged.RemoveAllListeners();
            ghiToggle.onLeftClick.RemoveAllListeners();

            ghiToggle.onLeftClick.AddListener(
                (UnityAction<UnityEngine.EventSystems.PointerEventData>)(
                    _ =>
                    {
                        _wasActiveAtLastClick = TotalSort.Active;
                    }
                )
            );

            ghiToggle.onValueChanged.AddListener(
                (UnityAction<bool>)(
                    isOn =>
                    {
                        if (isOn)
                        {
                            if (_wasActiveAtLastClick)
                                TotalSort.Order *= -1;
                            else
                                TotalSort.Order = -1;
                            TotalSort.Active = true;
                            InvokeSort(element);
                        }
                        else
                        {
                            TotalSort.Active = false;
                        }
                    }
                )
            );
        }

        private static void AttachTooltip(GameObject clone)
        {
            const string title = "TOTAL";
            string description =
                "Sum of "
                + Link("health", "Health")
                + " + "
                + Link("tu", "Time Units")
                + " + "
                + Link("accuracy", "Accuracy")
                + " + "
                + Link("strength", "Strength")
                + " + "
                + Link("reflexes", "Reflexes")
                + " + "
                + Link("bravery", "Bravery")
                + ".";

            var tooltipPrefab = FindStaticTextTooltipPrefab();
            if (tooltipPrefab != null)
            {
                var creator = clone.AddComponent<StaticTextTooltipCreator>();
                creator.Tooltip = tooltipPrefab;
                creator.Title = title;
                creator.Description = description;
                creator.AllowTooltip = true;
                return;
            }

            var fallback = clone.AddComponent<TotalTooltipCreator>();
            fallback.TooltipText = $"{title}\n{description}";
            Log.Info($"{LogPrefix} no styled tooltip prefab found; using label fallback");
        }

        private static string Link(string conceptFilename, string text)
        {
            return $"<link=\"asset://concept-:-{conceptFilename}\"><style=tooltip>{text}</style></link>";
        }

        private static StaticTextTooltip? _cachedTooltipPrefab;

        private static StaticTextTooltip? FindStaticTextTooltipPrefab()
        {
            if (_cachedTooltipPrefab != null)
                return _cachedTooltipPrefab;

            var creators = UnityEngine.Object.FindObjectsOfType<StaticTextTooltipCreator>(true);
            foreach (var c in creators)
            {
                if (c != null && c.Tooltip != null)
                {
                    _cachedTooltipPrefab = c.Tooltip;
                    return _cachedTooltipPrefab;
                }
            }
            return null;
        }

        private static void InvokeSort(object element)
        {
            try
            {
                var field = AccessTools.Field(element.GetType(), "_soldierControllers");
                if (field == null)
                {
                    Log.Warn($"{LogPrefix} _soldierControllers field not found on {element.GetType()}");
                    return;
                }
                var controllers = field.GetValue(element);
                if (controllers == null)
                    return;
                var sortMethod = AccessTools.Method(controllers.GetType(), "Sort");
                if (sortMethod == null)
                {
                    Log.Warn($"{LogPrefix} Sort method not found on {controllers.GetType()}");
                    return;
                }
                sortMethod.Invoke(controllers, null);
            }
            catch (Exception ex)
            {
                Log.Warn($"{LogPrefix} InvokeSort failed: {ex}");
            }
        }
    }
}
