using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models.User
{
    public class BanViewModel
    {
        public int UserId { get; set; }

        public bool Permanent { get; set; }

        public int? Hours { get; set; }

        public int? Days { get; set; }

        public int? Months { get; set; }
    }
}