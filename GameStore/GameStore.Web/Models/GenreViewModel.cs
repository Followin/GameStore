using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class GenreViewModel
    {
        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String NameRu { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String NameEn { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public IEnumerable<GenreViewModel> ChildGenres { get; set; } 
    }
}
