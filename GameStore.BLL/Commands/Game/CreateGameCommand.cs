using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Game
{
    public class CreateGameCommand : ICommand
    {
        public String Name { get; set; }

        public String Key { get; set; }

        public String Description { get; set; }

        public Double Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        public Int32 PublisherId { get; set; }

        public Int32[] GenreIds { get; set; }

        public Int32[] PlatformTypeIds { get; set; }
    }
}
