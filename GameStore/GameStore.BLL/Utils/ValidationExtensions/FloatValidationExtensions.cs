using System;
using ArgumentValidation;

namespace GameStore.BLL.Utils.ValidationExtensions
{
    public static class FloatValidationExtensions
    {
        public static IValidation<float> GreaterThan(this IValidation<float> item, float limit)
        {
            if (item.Value <= limit)
            {
                throw new ArgumentOutOfRangeException(item.ArgName, string.Format("{0} must be greater than {1}", item.ArgName, limit));
            }

            return item;
        }
    }
}
