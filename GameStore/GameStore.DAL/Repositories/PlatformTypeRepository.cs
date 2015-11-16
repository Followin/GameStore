using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.EF;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class PlatformTypeRepository : GenericRepository<PlatformType, Int32>, IPlatformTypeRepository
    {
        public PlatformTypeRepository(IEFContext context) : base(context)
        {
        }
    }
}