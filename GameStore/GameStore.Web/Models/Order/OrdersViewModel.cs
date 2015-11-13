using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Order
{
    public class OrdersViewModel
    {
        public IEnumerable<OrderViewModel> Orders { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "From")]
        [DataType(DataType.Date)]
        public DateTime? MinDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name="To")]
        [DataType(DataType.Date)]
        public DateTime? MaxDate { get; set; }
    }
}