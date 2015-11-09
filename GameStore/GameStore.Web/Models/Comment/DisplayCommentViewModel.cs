using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.Comment
{
    public class DisplayCommentViewModel
    {
        public Int32 Id { get; set; }

        public String Quotes { get; set; }

        public String Name { get; set; }

        public String Body { get; set; }

        public IEnumerable<DisplayCommentViewModel> ChildComments { get; set; } 
    }
}