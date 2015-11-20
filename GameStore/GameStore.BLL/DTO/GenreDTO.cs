using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.BLL.DTO
{
    public class GenreDTO : EntityDTO<int>
    {
        public string NameRu { get; set; }

        public string NameEn { get; set; }

        public ICollection<GenreDTO> ChildGenres { get; set; } 
    }
}
