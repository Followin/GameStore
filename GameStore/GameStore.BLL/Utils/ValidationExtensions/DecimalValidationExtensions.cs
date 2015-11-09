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
        public static IValidation<Decimal> GreaterThan(this IValidation<Decimal> item, Decimal limit)
        {
            if (item.Value <= limit)
            {    
                throw new ArgumentOutOfRangeException(
                    item.ArgName,
                    String.Format("Argument {0} must be greater than {1}", item.ArgName, limit));
            }

            return item;
        }
    }
}
