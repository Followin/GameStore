using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name="UserName")]
        public String Login { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "RememberMe")]
        public Boolean RememberMe { get; set; }

        public String ReturnUrl { get; set; }
    }
}