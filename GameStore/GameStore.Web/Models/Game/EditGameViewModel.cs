using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Game
{
    public class EditGameViewModel
    {
        public Int32 Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        [StringLength(30, MinimumLength = 5)]
        public String Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Key")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public String Key { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        [StringLength(200, MinimumLength = 10,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public String DescriptionEn { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        [StringLength(200, MinimumLength = 10,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public String DescriptionRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public Double Price { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "UnitsInStock")]
        public Int16 UnitsInStock { get; set; }

        public Boolean Discontinued { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Publisher")]
        public Int32 PublisherId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "PublicationDate")]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public Int32[] GenreIds { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "PlatformTypes")]
        public Int32[] PlatformTypeIds { get; set; }
    }
}