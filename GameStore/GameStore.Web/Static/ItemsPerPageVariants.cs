using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Static
{
    public static class ItemsPerPageVariants
    {
        public static IEnumerable<int> Variants;

        static ItemsPerPageVariants()
        {
            Variants = new[] {1, 10, 20, 50, 100};
        }
    }
}