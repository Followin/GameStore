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
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Number")]
        public string Number { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Cvv2")]
        public string Cvv2 { get; set; }

        
    }
}