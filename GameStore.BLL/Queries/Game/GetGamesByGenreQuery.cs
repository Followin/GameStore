using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesByGenreQuery : IQuery
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }
    }
}
