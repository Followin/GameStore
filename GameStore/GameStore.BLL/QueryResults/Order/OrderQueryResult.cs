using System;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Order
{
    public class OrderQueryResult : IQueryResult
    {
        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public IEnumerable<OrderDetailsDTO> OrderDetails { get; set; } 
    }
}
