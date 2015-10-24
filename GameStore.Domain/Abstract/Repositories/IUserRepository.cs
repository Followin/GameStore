﻿using System;
using GameStore.Domain.Entities;

namespace GameStore.Domain.Abstract.Repositories
{
    public interface IUserRepository : IRepository<User, Int32>
    {
         
    }
}