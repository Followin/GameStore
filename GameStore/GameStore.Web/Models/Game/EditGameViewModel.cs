using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Game
{
    public class EditGameViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        [StringLength(30, MinimumLength = 5)]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Key")]
        [StringLength(30, MinimumLength = 5,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public string Key { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "DescriptionEn")]
        [StringLength(200, MinimumLength = 10,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public string DescriptionEn { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "DescriptionRu")]
        [StringLength(200, MinimumLength = 10,
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public string DescriptionRu { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Price")]
        public double Price { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "UnitsInStock")]
        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Publisher")]
        public int? PublisherId { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "PublicationDate")]
        public DateTime PublicationDate { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Genres")]
        public int[] GenreIds { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "FieldIsRequired")]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "PlatformTypes")]
        public int[] PlatformTypeIds { get; set; }
    }
}