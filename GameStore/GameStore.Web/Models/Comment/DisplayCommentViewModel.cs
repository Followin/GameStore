using System;
using System.Collections.Generic;

namespace GameStore.Web.Models.Comment
{
    public class DisplayCommentViewModel
    {
        public int Id { get; set; }

        public string Quotes { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public IEnumerable<DisplayCommentViewModel> ChildComments { get; set; } 
    }
}