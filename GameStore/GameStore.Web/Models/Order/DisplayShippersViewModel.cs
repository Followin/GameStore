using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Order
{
    public class ShipperViewModel
    {
        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String CompanyName { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Phone")]
        public String Phone { get; set; }
    }

    public class DisplayShippersViewModel
    {
        public IEnumerable<ShipperViewModel> Shippers { get; set; } 
    }
}