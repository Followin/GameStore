using System.Collections.Generic;
using System.Web.Mvc;

namespace GameStore.Web.Models.Manage
{
    public class ManageViewModel
    {
        public IEnumerable<SelectListItem> NotificationTypes { get; set; } 
    }
}