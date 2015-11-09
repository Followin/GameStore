using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults.Order
{
    public class OrdersQueryResult : IEnumerable<OrderQueryResult>, IQueryResult
    {
        private IEnumerable<OrderQueryResult> _orders;

        public OrdersQueryResult(IEnumerable<OrderQueryResult> orders)
        {
            _orders = orders;
        }

        public IEnumerator<OrderQueryResult> GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
