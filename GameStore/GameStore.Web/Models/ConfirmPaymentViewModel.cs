using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class ConfirmPaymentViewModel
    {
        [Required]
        [Display(ResourceType = typeof(GlobalRes), Name="ConfirmationCode")]
        public String Code { get; set; }
    }
}