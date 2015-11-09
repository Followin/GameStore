﻿using System;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class GameQueryResult :  IQueryResult
    {
        public Int32 Id { get; set; }

        public String Key { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Decimal Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        public PublisherDTO Publisher { get; set; }

        public ICollection<GenreDTO> Genres { get; set; }

        public ICollection<PlatformTypeDTO> PlatformTypes { get; set; } 
    }
}