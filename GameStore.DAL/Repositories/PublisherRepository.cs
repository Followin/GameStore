using System;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class PublisherRepository : GenericRepository<Publisher, Int32>, IPublisherRepository
    {
        public PublisherRepository(IContext context) : base(context)
        {
        }
    }
}
