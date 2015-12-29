using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        [Remote("IsUsernameFree", "Account",
            ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "NicknameAlreadyExists")]
        public string Name { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(ResourceType = typeof(GlobalRes),
            Name = "Password")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceType = typeof(GlobalRes),
            ErrorMessageResourceName = "PasswordsMustMatch")]
        public string PasswordConfirm { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name="Phone")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}