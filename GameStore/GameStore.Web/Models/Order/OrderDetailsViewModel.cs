using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models.Game;

namespace GameStore.Web.Models.Order
{
    public class OrderDetailsViewModel
    {
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public Decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Discount")]
        public float Discount { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Quantity")]
        public Int16 Quantity { get; set; }

        public Int32 OrderId { get; set; }

        public Int32 GameId { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Game")]
        public DisplayGameModel Game { get; set; }
    }
}