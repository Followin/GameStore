using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.Comment
{
    public class DisplayCommentViewModel
    {
        public String Name { get; set; }

        public String Body { get; set; }

        public IEnumerable<DisplayCommentViewModel> ChildComments { get; set; } 
    }
}