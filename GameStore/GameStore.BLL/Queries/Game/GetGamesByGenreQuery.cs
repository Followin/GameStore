using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesByGenreQuery : IQuery
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
