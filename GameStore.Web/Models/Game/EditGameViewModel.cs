using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.Game
{
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
}