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

        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public DateTime? Time { get; set; }

        public Boolean Payed { get; set; }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }

        public Decimal Price
        {
            get { return OrderDetails.Sum(x => (x.Price - (x.Price * (decimal) x.Discount)) * x.Quantity); }
        }
    }
}