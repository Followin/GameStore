using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Publisher
{
    public class DisplayPublisherViewModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public string CompanyName { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "HomePage")]
        public string HomePage { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public string Description { get; set; }
    }
}