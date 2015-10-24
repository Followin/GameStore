using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class PlatformTypeRepository : GenericRepository<PlatformType, Int32>, IPlatformTypeRepository
    {
        public PlatformTypeRepository(IContext context) : base(context)
        {
        }
    }
}