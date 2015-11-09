using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.Models.Comment
{
    public class CreateCommentViewModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public String Name { get; set; }

        public String Quotes { get; set; }

        [Required]
        [StringLength(300,MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public String Body { get; set; }

        public Int32 GameId { get; set; }

        public Int32? ParentCommentId { get; set; }
    }
}