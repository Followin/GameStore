using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models.User
{
    public class BanViewModel
    {
        public Int32 UserId { get; set; }

        public Boolean Permanent { get; set; }

        public Int32 Hours { get; set; }

        public Int32 Days { get; set; }

        public Int32 Months { get; set; }
    }
}