using System.Collections.Generic;
using System.Linq;

namespace HRSystem.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> With<T>(this IEnumerable<T> enumerable, T item)
        {
            return enumerable.Concat(new[] {item});
        }
    }
}