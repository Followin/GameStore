using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Utils
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }
    }
}