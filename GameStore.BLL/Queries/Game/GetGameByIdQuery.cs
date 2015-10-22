﻿using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGameByIdQuery : IQuery
    {
        public Int32 Id { get; set; }
    }
}
