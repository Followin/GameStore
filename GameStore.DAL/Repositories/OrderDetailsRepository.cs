using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(IContext context) : base(context)
        {
        }
    }
}
