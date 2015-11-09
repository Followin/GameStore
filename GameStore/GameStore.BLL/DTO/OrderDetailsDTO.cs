using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class OrderDetailsDTO
    {
        public Decimal Price { get; set; }

        public float Discount { get; set; }

        public Int16 Quantity { get; set; }

        public Int32 GameId { get; set; }

        public GameDTO Game { get; set; }

        public Int32 OrderId { get; set; }
    }
}
