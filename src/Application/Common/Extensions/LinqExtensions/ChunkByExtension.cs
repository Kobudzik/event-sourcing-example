using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcingExample.Application.Common.Extensions.LinqExtensions
{
    internal static class ChunkByExtension
    {
        public static IEnumerable<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize = 50)
        {
            return source
                .ToList()
                .ChunkBy(chunkSize);
        }

        internal static IEnumerable<List<T>> ChunkBy<T>(this List<T> source, int chunkSize = 50)
        {
            for (int i = 0; i < source.Count; i += chunkSize)
            {
                yield return source.GetRange(
                    i,
                    Math.Min(chunkSize, source.Count - i)
                );
            }
        }

        public static List<Dictionary<TKey, TValue>> ChunkBy<TKey, TValue>(this Dictionary<TKey, TValue> source, int chunkSize)
        {
            if (chunkSize == 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), "ChunkSize cannot be zero");

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToDictionary(d => d.Key, d => d.Value))
                .ToList();
        }
    }
}