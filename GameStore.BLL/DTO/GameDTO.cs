using System;
using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class GameDTO : EntityDTO<Int32>
    {
        public String Key { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Decimal Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        public PublisherDTO Publisher { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime IncomeDate { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }

        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; } 
    }
}
