using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models
{
    public class CreateCommentViewModel
    {
        [Required]
        [MinLength(3)]
        public String Name { get; set; }

        [Required]
        [MinLength(5)]
        public String Body { get; set; }
        public Int32? GameId { get; set; }
        public Int32? ParentCommentId { get; set; }
    }

    public class DisplayCommentViewModel
    {
        public String Name { get; set; }
        public String Body { get; set; }
        public IEnumerable<DisplayCommentViewModel> ChildComments { get; set; } 
    }
}