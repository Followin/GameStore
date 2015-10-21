using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands
{
    public class EditGameCommand : ICommand
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Key { get; set; }

        public String Description { get; set; }

        public Double Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discounted { get; set; }

        public Int32 PublisherId { get; set; }

        public Int32[] GenreIds { get; set; }

        public Int32[] PlatformTypeIds { get; set; }
    }
}
