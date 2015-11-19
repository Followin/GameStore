using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.Static;

namespace GameStore.BLL.QueryResults.Order
{
    public class PaymentMethodQueryResult : IQueryResult
    {
        public PaymentMethodQueryResult(PaymentMethod method)
        {
            Method = method;
        }

        public PaymentMethod Method { get; set; }
    }
}
