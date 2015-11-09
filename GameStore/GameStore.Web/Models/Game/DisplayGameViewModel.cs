using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models.Publisher;

namespace GameStore.Web.Models.Game
{
    public class DisplayGameModel
    {

        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Key")]
        public String Key { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public String DescriptionEn { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public String DescriptionRu { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public Decimal Price { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "UnitsInStock")]
        public Int16 UnitsInStock { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Discontinued")]
        public Boolean Discontinued { get; set; }

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
    }

    public class GameFiltersModel
    {
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public Int32[] GenreIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "PlatformTypes")]
        public Int32[] PlatformTypeIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_3_100")]
        public String Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "OrderBy")]
        public String OrderBy { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Publishers")]
        public Int32[] PublisherIds { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Page")]
        public Int32 Page { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "ItemsPerPage")]
        public Int32? ItemsPerPage { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MinPrice")]
        public Int32 MinPrice { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MaxPrice")]
        public Int32 MaxPrice { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "MinDate")]
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