using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class OrderDetails
    {
        public Decimal Price { get; set; }

        public float Discount { get; set; }
 
        public Int16 Quantity { get; set; }

        public Int32 GameId { get; set; }

        [NotMapped]
        public virtual Game Game { get; set; }

        public Int32 OrderId { get; set; }

        public Order Order { get; set; }
    }
}
