using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Static;
using GameStore.Web.Abstract;
using GameStore.Web.Concrete;

namespace GameStore.Web.Utils
{
    public static class PaymentList
    {
        private static Dictionary<PaymentMethod, IPayment> _paymentMethods;

        static PaymentList()
        {
            _paymentMethods = new Dictionary<PaymentMethod, IPayment>
            {
                {
                    PaymentMethod.Visa,
                    new CardPayment(
                        @"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Visa_Inc._logo.svg/200px-Visa_Inc._logo.svg.png",
                        "VISA",
                        "Payment by VISA card",
                        PaymentMethod.Visa)
                },
                {
                    PaymentMethod.Mastercard,
                    new CardPayment(
                        @"https://upload.wikimedia.org/wikipedia/commons/thumb/0/0c/MasterCard_logo.png/320px-MasterCard_logo.png",
                        "Mastercard",
                        "Payment by mastercard",
                        PaymentMethod.Mastercard)
                },
                {
                    PaymentMethod.Bank,
                    new BankPayment(
                        @"http://goodlogo.com/images/logos/state_bank_of_india_logo_3898.png",
                        "Bank",
                        "Invoice payment using bank")
                },
                {
                    PaymentMethod.Ibox,
                    new IboxPayment(
                        @"http://www.uic.in.ua/wp-content/uploads/2014/06/ibox.png",
                        "IBOX",
                        "Payment by IBOX terminal")
                }
            };
        }

        public static IPayment GetPayment(PaymentMethod method)
        {
            return _paymentMethods[method];
        }

        public static IEnumerable<IPayment> GetPayments()
        {
            return _paymentMethods.Values;
        }

        
    }
}