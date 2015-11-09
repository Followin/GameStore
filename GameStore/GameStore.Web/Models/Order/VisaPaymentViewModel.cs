using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models.Order
{
    public class VisaPaymentViewModel
    {
        public String Name { get; set; }
        public String Number { get; set; }
        public DateTime ExpireDate { get; set; }
        public String Cvv2 { get; set; }
    }
}