using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Web.Abstract;

namespace GameStore.Web.Models.Order
{
    public class OrderCheckoutViewModel
    {
        public OrderViewModel Order { get;set; }
        public IEnumerable<KeyValuePair<string, IPayment>> PaymentMethods { get;set; }
    }
}