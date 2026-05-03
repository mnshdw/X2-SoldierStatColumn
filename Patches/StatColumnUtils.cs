using System;
using TMPro;
using UnityEngine;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    internal static class StatColumnUtils
    {
        public static void StripLocalization(GameObject go)
        {
            if (go == null)
                return;

            foreach (var component in go.GetComponents<MonoBehaviour>())
            {
                if (component == null)
                    continue;

                var typeName = component.GetType().FullName ?? string.Empty;
                if (
                    typeName == "I2.Loc.Localize"
                    || typeName == "Common.Translation.Assets.TranslationScanKeyBehaviour"
                )
                {
                    UnityEngine.Object.Destroy(component);
                }
            }
        }

        public static void OffsetAfter(
            RectTransform clone,
            RectTransform anchor,
            RectTransform previous
        )
        {
            if (clone == null || anchor == null || previous == null)
                return;

            float dx = anchor.anchoredPosition.x - previous.anchoredPosition.x;
            var pos = anchor.anchoredPosition;
            pos.x += dx;
            clone.anchoredPosition = pos;
        }

        public static TMP_Text? CloneStatColumn(
            TMP_Text template,
            TMP_Text previousSibling,
            string childName,
            Color color
        )
        {
            try
            {
                if (template == null || previousSibling == null)
                    return null;

                var anchorColumn = WalkUpToCommonParent(
                    template.transform,
                    previousSibling.transform
                );
                if (anchorColumn == null)
                    return null;

                var previousColumn = WalkUpToSiblingOf(
                    previousSibling.transform,
                    anchorColumn.parent
                );
                if (previousColumn == null)
                    return null;

                var rowParent = anchorColumn.parent;
                if (rowParent == null)
                    return null;

                var existing = rowParent.Find(childName);
                if (existing != null)
                    return existing.GetComponentInChildren<TMP_Text>(true);

                var clone = UnityEngine.Object.Instantiate(anchorColumn.gameObject, rowParent);
                clone.name = childName;
                clone.transform.SetSiblingIndex(anchorColumn.GetSiblingIndex() + 1);

                StripLocalization(clone);
                foreach (var child in clone.GetComponentsInChildren<Transform>(true))
                {
                    StripLocalization(child.gameObject);
                }

                if (
                    anchorColumn is RectTransform anchorRT
                    && previousColumn is RectTransform prevRT
                    && clone.transform is RectTransform cloneRT
                )
                {
                    OffsetAfter(cloneRT, anchorRT, prevRT);
                }

                var cloneText = clone.GetComponentInChildren<TMP_Text>(true);
                if (cloneText != null)
                {
                    cloneText.color = color;
                }
                return cloneText;
            }
            catch (Exception ex)
            {
                Log.Warn($"{LogPrefix} CloneStatColumn failed: {ex}");
                return null;
            }
        }

        private static Transform? WalkUpToCommonParent(Transform a, Transform b)
        {
            if (a == null || b == null)
                return null;

            var parents = new System.Collections.Generic.HashSet<Transform>();
            for (var t = b.parent; t != null; t = t.parent)
                parents.Add(t);

            for (var t = a; t != null && t.parent != null; t = t.parent)
            {
                if (parents.Contains(t.parent))
                    return t;
            }
            return null;
        }

        private static Transform? WalkUpToSiblingOf(Transform descendant, Transform? parent)
        {
            if (descendant == null || parent == null)
                return null;

            for (var t = descendant; t != null; t = t.parent)
            {
                if (t.parent == parent)
                    return t;
            }
            return null;
        }
    }
}
