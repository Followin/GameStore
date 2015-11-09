using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Publisher
{
    public class CreatePublisherViewModel
    {
        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public String CompanyName { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "HomePage")]
        public String HomePage { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Description")]
        public String Description { get; set; }
    }
}