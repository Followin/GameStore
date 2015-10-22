using System;
using System.Collections.Generic;
using GameStore.Web.Models.Genre;
using GameStore.Web.Models.PlatformType;

namespace GameStore.Web.Models.Game
{
    public class DisplayGameViewModel
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Key { get; set; }

        public String Description { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; }

        public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }
    }
}