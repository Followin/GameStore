using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class GameDTO : EntityDTO<Int32>
    {
        public String Key { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public ICollection<GenreDTO> Genres { get; set; }
        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; } 
    }
}
