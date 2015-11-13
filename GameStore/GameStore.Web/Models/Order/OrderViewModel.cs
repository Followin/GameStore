using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Order
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderDetails = new List<OrderDetailsViewModel>();
        }

        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "User")]
        public Int32 UserId { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Time")]
        public DateTime? Time { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "ShippedDate")]
        public DateTime? ShippedDate { get; set; }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public Decimal Price
        {
            get { return OrderDetails.Sum(x => (x.Price - (x.Price * (decimal) x.Discount)) * x.Quantity); }
        }

        public String Status
        {
            get
            {
                return ShippedDate != null ? GlobalRes.Shipped : OrderDate != null ? GlobalRes.Paid : GlobalRes.NotPaid;
            }
        }
    }
}