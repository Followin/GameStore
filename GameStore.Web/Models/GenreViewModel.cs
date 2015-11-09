﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Web.Models
{
    public class GenreViewModel
    {
        public Int32 Id { get; set; }

        public String NameRu { get; set; }

        public String NameEn { get; set; }

        public IEnumerable<GenreViewModel> ChildGenres { get; set; } 
    }
}
