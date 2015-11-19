using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Static;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Order
{
    public class CardPaymentViewModel
    {
        public PaymentMethod Method { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Number")]
        public String Number { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "ExpireDate")]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Cvv2")]
        public String Cvv2 { get; set; }

        
    }
}