using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models.Publisher
{
    public class CreatePublisherViewModel
    {
        [Required]
        public String CompanyName { get; set; }

        [Required]
        public String HomePage { get; set; }

        [Required]
        public String Description { get; set; }
    }
}