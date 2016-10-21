using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRSalad.Infrastructure
{
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            var keys = new HashSet<TKey>();
            return source.Where(element => keys.Add(keySelector(element)));
        }

        public static bool AllEqual<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctBy(keySelector).Count() == 1;
        }
    }
}