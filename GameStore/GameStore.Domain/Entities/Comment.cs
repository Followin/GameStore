using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Comment : Entity<Int32>
    {
        public String Name { get; set; }

        public String Quotes { get; set; }

        public String Body { get; set; }

        public Int32? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public Int32 GameId { get; set; }

        [NotMapped]
        public virtual Game Game { get; set; }

        public virtual ICollection<Comment> ChildComments { get; set; }
    }
}
