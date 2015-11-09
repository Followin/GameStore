using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Order
{
    public class VisaPaymentViewModel
    {
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Number")]
        public String Number { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "ExpireDate")]
        public DateTime ExpireDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Cvv2")]
        public String Cvv2 { get; set; }
    }
}