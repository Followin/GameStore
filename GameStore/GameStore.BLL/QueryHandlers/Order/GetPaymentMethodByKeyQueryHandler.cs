using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;

namespace GameStore.BLL.QueryHandlers.Order
{
    public class GetPaymentMethodByKeyQueryHandler : 
        IQueryHandler<GetPaymentMethodByKeyQuery, PaymentMethodQueryResult>
    {
        public PaymentMethodQueryResult Retrieve(GetPaymentMethodByKeyQuery query)
        {
            return new PaymentMethodQueryResult(PaymentMethodsDictionary.GetMethod(query.Key));
        }
    }
}
