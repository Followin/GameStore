using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class OrderDetails
    {
        public decimal Price { get; set; }

        public float Discount { get; set; }
 
        public short Quantity { get; set; }

        public int GameId { get; set; }

        [NotMapped]
        public virtual Game Game { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
