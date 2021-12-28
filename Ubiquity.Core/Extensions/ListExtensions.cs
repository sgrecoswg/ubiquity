using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Ubiquity.Core.Extensions
{
    public static class ListExtensions
    {
        public static bool IsNullOrEmpty<T>(this List<T> l)
        {
            return l == null || l.Count() <= 0;
        }

        public static T Random<T>(this IEnumerable<T> list)
        {
            Random rnd = new Random();
            return list.ElementAt(rnd.Next(0, list.Count()));
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
              .Range(0, 1 << (data.Length))
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToArray());
        }
    }
}
