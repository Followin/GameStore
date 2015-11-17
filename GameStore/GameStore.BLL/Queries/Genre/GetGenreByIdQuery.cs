using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Genre
{
    public class GetGenreByIdQuery : IQuery
    {
        public Int32 Id { get; set; }
    }
}
