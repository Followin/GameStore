using System;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Game
{
    public class GameQueryResult :  IQueryResult
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEn { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public PublisherDTO Publisher { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime IncomeDate { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }

        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; }

        public EntryState EntryState { get; set; }
    }
}
