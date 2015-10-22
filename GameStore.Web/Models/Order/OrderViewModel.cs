using System;
using System.Collections.Generic;
using System.Linq;

namespace GameStore.Web.Models.Order
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderDetails = new List<OrderDetailsViewModel>();
        }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }

        public Decimal Price
        {
            get { return OrderDetails.Sum(x => (x.Price - (x.Price * (decimal) x.Discount)) * x.Quantity); }
        }
    }
}