using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.Comment
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
}