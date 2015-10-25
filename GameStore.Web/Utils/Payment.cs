using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Web.Abstract;

namespace GameStore.Web.Utils
{
    public static class PaymentList
    {
        static PaymentList()
        {
            PaymentMethods = new Dictionary<String, IPayment>();
        }

        public static Dictionary<String, IPayment> PaymentMethods { get; set; }  
    }
}