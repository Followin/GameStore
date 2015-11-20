using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class OrderDetailsDTO
    {
        public decimal Price { get; set; }

        public float Discount { get; set; }

        public short Quantity { get; set; }

        public int GameId { get; set; }

        public GameDTO Game { get; set; }

        public int OrderId { get; set; }
    }
}
