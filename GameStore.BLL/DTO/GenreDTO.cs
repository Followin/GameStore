using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Entities;

namespace GameStore.BLL.DTO
{
    public class GenreDTO : EntityDTO<Int32>
    {
        public String NameRu { get; set; }

        public String NameEn { get; set; }

        public ICollection<GenreDTO> ChildGenres { get; set; } 
    }
}
