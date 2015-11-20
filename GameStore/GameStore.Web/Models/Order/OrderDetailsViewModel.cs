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
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Discount")]
        public float Discount { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Quantity")]
        public short Quantity { get; set; }

        public int OrderId { get; set; }

        public int GameId { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Game")]
        public DisplayGameModel Game { get; set; }
    }
}