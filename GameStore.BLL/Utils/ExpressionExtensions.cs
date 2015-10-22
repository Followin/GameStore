using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Utils
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<TModel, TValue>(this Expression<Func<TModel, TValue>> propertySelector, char delimiter = '.', char endTrim = ')')
        {
            var asString = propertySelector.ToString(); // gives you: "o => o.Whatever"
            var lastDelim = asString.LastIndexOf(delimiter); // make sure there is a beginning property indicator; the "." in "o.Whatever" -- this may not be necessary?

            return lastDelim < 0
                ? asString
                : asString.Substring(lastDelim + 1).TrimEnd(endTrim);
        }
    }
}
