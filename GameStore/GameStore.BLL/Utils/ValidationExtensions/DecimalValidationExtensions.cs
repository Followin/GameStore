using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;

namespace GameStore.BLL.Utils.ValidationExtensions
{
    public static class DecimalValidationExtensions
    {
        public static IValidation<decimal> GreaterThan(this IValidation<decimal> item, decimal limit)
        {
            if (item.Value <= limit)
            {    
                throw new ArgumentOutOfRangeException(
                    item.ArgName,
                    string.Format("Argument {0} must be greater than {1}", item.ArgName, limit));
            }

            return item;
        }
    }
}
