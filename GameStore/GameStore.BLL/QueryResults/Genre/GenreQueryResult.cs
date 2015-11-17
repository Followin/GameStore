using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Genre
{
    public class GenreQueryResult : IQueryResult
    {
        public Int32 Id { get; set; }

        public String NameRu { get; set; }

        public String NameEn { get; set; }

        public ICollection<GenreDTO> ChildGenres { get; set; } 
    }
}
