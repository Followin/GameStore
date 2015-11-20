using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Genres
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public IEnumerable<GenreViewModel> ChildGenres { get; set; } 
    }
}
