using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Game
{
    public class EditGameCommand : ICommand
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string DescriptionEn { get; set; }

        public string DescriptionRu { get; set; }

        public double Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int PublisherId { get; set; }

        public DateTime PublishDate { get; set; }

        public int[] GenreIds { get; set; }

        public int[] PlatformTypeIds { get; set; }
    }
}
