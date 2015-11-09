using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models.Order
{
    public class ShipperViewModel
    {
        public Int32 Id { get; set; }

        public String CompanyName { get; set; }

        public String Phone { get; set; }
    }

    public class DisplayShippersViewModel
    {
        public IEnumerable<ShipperViewModel> Shippers { get; set; } 
    }
}