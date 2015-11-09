using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameStore.Web.Models.Game;

namespace GameStore.Web.Models.Order
{
    public class OrderDetailsViewModel
    {
        public Decimal Price { get; set; }

        public float Discount { get; set; }

        public Int16 Quantity { get; set; }

        public Int32 GameId { get; set; }

        public DisplayGameModel Game { get; set; }
    }
}