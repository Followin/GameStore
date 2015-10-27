using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GameStore.Web.Models.Game
{
    public class CreateGameModel
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
        public Double Price { get; set; }

        [Required]
        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        [Required]
        public Int32 PublisherId { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public Int32[] GenreIds { get; set; }

        [Required]
        public Int32[] PlatformTypeIds { get; set; }
    }

    public class CreateGameViewModel
    {
        [Required]
        public CreateGameModel CreateModel { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; }

        public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }

        public SelectList Publishers { get; set; } 
    }
}