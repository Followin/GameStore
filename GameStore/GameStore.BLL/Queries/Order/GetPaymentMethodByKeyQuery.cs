using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Order
{
    public class GetPaymentMethodByKeyQuery : IQuery
    {
        public String Key { get; set; }
    }
}
