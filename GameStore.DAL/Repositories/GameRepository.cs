using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameRepository : GenericRepository<Game, Int32>, IGameRepository
    {
        private IDbSet<Game> _set;  
        public GameRepository(IContext context) : base(context)
        {
            _set = context.Set<Game>();
        }

        public IEnumerable<Game> Get(
            Expression<Func<Game, bool>> predicate,
            String comparer = null,
            int? skip = null,
            int? number = null)
        {
            
            IQueryable<Game> fullyResult = _set;
            if (comparer != null)
            {
                switch (comparer)
                {
                    case "views":
                        fullyResult = fullyResult.OrderBy(x => x.UsersViewed.Count);
                        break;
                    case "comments":
                        fullyResult = fullyResult.OrderBy(x => x.Comments.Count);
                        break;
                    case "price":
                        fullyResult = fullyResult.OrderBy(x => x.Price);
                        break;
                    case "incomeDate":
                        fullyResult = fullyResult.OrderBy(x => x.IncomeDate);
                        break;
                }
            }

            fullyResult = fullyResult.Where(predicate);

            if (skip.HasValue)
            {
                fullyResult = fullyResult.Skip(skip.Value);
            }

            if (number.HasValue)
            {
                fullyResult = fullyResult.Take(number.Value);
            }

            return fullyResult.ToList();
        }
    }
}
