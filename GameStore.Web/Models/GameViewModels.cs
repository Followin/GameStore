using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.BLL.Commands;

namespace GameStore.Web.Models
{
    public class CreateGameViewModel
    {
        [Required]
        [MinLength(3)]
        public String Name { get; set; }

        [Required]
        [MinLength(3)]
        public String Key { get; set; }

        [Required]
        [MinLength(10)]
        public String Description { get; set; }

        [Required]
        public Int32[] GenreIds { get; set; }

        [Required]
        public Int32[] PlatformTypeIds { get; set; }
    }

    public class EditGameViewModel
    {
        [Required]
        public Int32 Id { get; set; }

        [Required]
        [MinLength(3)]
        public String Name { get; set; }

        [Required]
        [MinLength(3)]
        public String Key { get; set; }

        [Required]
        [MinLength(10)]
        public String Description { get; set; }

        [Required]
        public Int32[] GenreIds { get; set; }

        [Required]
        public Int32[] PlatformTypeIds { get; set; }
    }

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