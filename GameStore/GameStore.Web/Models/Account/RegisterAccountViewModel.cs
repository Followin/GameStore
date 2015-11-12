using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models.Account
{
    public class RegisterAccountViewModel
    {
        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "UserName")]
        [StringLength(30, MinimumLength = 5, 
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "Length_5_30")]
        public String Name { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "PasswordsMustMatch")]
        public String PasswordConfirm { get; set; }
    }
}