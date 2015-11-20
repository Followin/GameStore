﻿using System;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.Domain.Entities;

namespace GameStore.DAL.Repositories
{
    public class PlatformTypeRepository : GenericRepository<PlatformType, int>, IPlatformTypeRepository
    {
        public PlatformTypeRepository(IEFContext context) : base(context)
        {
        }
    }
}