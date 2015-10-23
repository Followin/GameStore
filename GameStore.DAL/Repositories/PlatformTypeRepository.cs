using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class PlatformTypeRepository : GenericRepository<PlatformType>, IPlatformTypeRepository
    {
        public PlatformTypeRepository(IContext context) : base(context)
        {
        }
    }
}