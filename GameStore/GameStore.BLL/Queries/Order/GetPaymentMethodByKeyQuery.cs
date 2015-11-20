using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Order
{
    public class GetPaymentMethodByKeyQuery : IQuery
    {
        public string Key { get; set; }
    }
}
