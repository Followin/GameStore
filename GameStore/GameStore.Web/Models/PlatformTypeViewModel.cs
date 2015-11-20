using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.Models
{
    public class PlatformTypeViewModel
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes),
            Name = "Name")]
        public string Name { get; set; }
    }
}
