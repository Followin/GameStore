using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Order
{
    public class GetOrdersHistoryQuery : IQuery
    {
        public bool OnlyPaid { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get;set; }
    }
}
