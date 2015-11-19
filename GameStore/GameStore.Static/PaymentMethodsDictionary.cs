using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Static
{
    public static class PaymentMethodsDictionary
    {
        private static Dictionary<String, PaymentMethod> _methods;

        static PaymentMethodsDictionary()
        {
            _methods = new Dictionary<string, PaymentMethod>
            {
                {"ibox", PaymentMethod.Ibox},
                {"bank", PaymentMethod.Bank},
                {"visa", PaymentMethod.Visa},
                {"mastercard", PaymentMethod.Mastercard}
            };
        }

        public static PaymentMethod GetMethod(String key)
        {
            return _methods[key];
        }

        public static Dictionary<String, PaymentMethod> GetMethods()
        {
            return _methods;
        } 
    }
}
