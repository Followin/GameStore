using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Static;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models.Genres;
using GameStore.Web.Models.Publisher;
using GameStore.Web.Static;

namespace GameStore.Web.Models.Game
{
    public class DisplayGameModel
    {

        public int Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Key")]
        public string Key { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Discontinued")]
        public bool Discontinued { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Publisher")]
        public DisplayPublisherViewModel Publisher { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "PublicationDate")]
        public DateTime PublicationDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "IncomeDate")]
        public DateTime IncomeDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public IEnumerable<GenreViewModel> Genres { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "PlatformTypes")]
        public IEnumerable<PlatformTypeViewModel> PlatformTypes { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class GameFiltersModel
    {
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public int[] GenreIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "PlatformTypes")]
        public int[] PlatformTypeIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_3_100")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "OrderBy")]
        public GamesOrderType OrderBy { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Publishers")]
        public int[] PublisherIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Page")]
        public int Page { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "ItemsPerPage")]
        public int? ItemsPerPage { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MinPrice")]
        public int MinPrice { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MaxPrice")]
        public int MaxPrice { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MinDate")]
        public DaysShortcut MinDateShortcut { get; set; }
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
        public IEnumerable<SelectListItem> ItemsPerPageVariants { get; set; }
    }
}