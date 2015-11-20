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
        public string Login { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "RememberMe")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}