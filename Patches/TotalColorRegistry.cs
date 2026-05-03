using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SoldierTotalColumn.ModConstants;

namespace SoldierTotalColumn.Patches
{
    internal static class TotalColorRegistry
    {
        private static readonly HashSet<string> _loggedKeys = new();

        private static readonly Color LowColor = new Color(0.85f, 0.20f, 0.20f);
        private static readonly Color MidColor = new Color(0.95f, 0.85f, 0.30f);
        private static readonly Color HighColor = new Color(0.45f, 0.85f, 0.30f);

        private sealed class Entry
        {
            public TMP_Text Text = null!;
            public int Value;
        }

        private static readonly Dictionary<string, List<Entry>> _byKey = new();

        public static void Update(string key, TMP_Text text, int value)
        {
            if (text == null)
                return;

            if (!_byKey.TryGetValue(key, out var entries))
            {
                entries = new List<Entry>();
                _byKey[key] = entries;
            }

            entries.RemoveAll(e => e.Text == null);

            var existing = entries.Find(e => e.Text == text);
            if (existing != null)
            {
                existing.Value = value;
            }
            else
            {
                entries.Add(new Entry { Text = text, Value = value });
            }

            if (entries.Count == 0)
                return;

            int min = int.MaxValue;
            int max = int.MinValue;
            foreach (var e in entries)
            {
                if (e.Value < min)
                    min = e.Value;
                if (e.Value > max)
                    max = e.Value;
            }

            float range = max - min;
            foreach (var e in entries)
            {
                float t = range > 0f ? (e.Value - min) / range : 0.5f;
                e.Text.color = Interpolate(t);
            }

            if (_loggedKeys.Add(key))
            {
                Log.Info(
                    $"{LogPrefix} color registry: key={key} entries={entries.Count} min={min} max={max}"
                );
            }
        }

        private static Color Interpolate(float t)
        {
            if (t < 0.5f)
                return Color.Lerp(LowColor, MidColor, t * 2f);
            return Color.Lerp(MidColor, HighColor, (t - 0.5f) * 2f);
        }
    }
}
