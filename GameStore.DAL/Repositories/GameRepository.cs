﻿using System;
using System.Linq;
using GameStore.DAL.Abstract;
using GameStore.Domain.Abstract.Repositories;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class GameRepository : GenericRepository<Game, Int32>, IGameRepository
    {
        public GameRepository(IContext context) : base(context)
        {
        }
    }
}
