using System;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Publisher
{
    public class DisplayPublisherViewModel
    {
        public Int32 Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String CompanyName { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "HomePage")]
        public String HomePage { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public String Description { get; set; }
    }
}