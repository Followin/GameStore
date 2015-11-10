using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Genres
{
    public class GenreViewModel
    {
        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public IEnumerable<GenreViewModel> ChildGenres { get; set; } 
    }
}
