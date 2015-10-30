using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.Models.Publisher;

namespace GameStore.Web.Models.Game
{
    public class DisplayGameModel
    {
        public Int32 Id { get; set; }

        public String Name { get; set; }

        public String Key { get; set; }

        public String Description { get; set; }

        public Decimal Price { get; set; }

        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        public DisplayPublisherViewModel Publisher { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime IncomeDate { get; set; }

        public IEnumerable<GenreViewModel> Genres { get; set; }

        public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }
    }

    public class GameFiltersModel
    {
        public Int32[] GenreIds { get; set; }

        public Int32[] PlatformTypeIds { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public String Name { get; set; }

        public String OrderBy { get; set; }

        public Int32[] PublisherIds { get; set; }

        public Int32 Page { get; set; }

        public Int32? ItemsPerPage { get; set; }

        public Int32 MinPrice { get; set; }

        public Int32 MaxPrice { get; set; }

        public String MinDateShortcut { get; set; }
    }

    public class PagedDisplayGameModel
    {
        public IEnumerable<DisplayGameModel> DisplayModel { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }

    public class DisplayGameViewModel
    {
        public PagedDisplayGameModel Model { get; set; }
        public GameFiltersModel FilterModel { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; }
        public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }
        public IEnumerable<DisplayPublisherViewModel> Publishers { get; set; }
        public IEnumerable<SelectListItem> OrderByVariants { get; set; }
        public IEnumerable<SelectListItem> ItemsPerPageVariants { get; set; }
        public IEnumerable<SelectListItem> DatesShorcuts { get; set; } 
    }
}