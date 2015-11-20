using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Domain.Entities
{
    public class Comment : Entity<int>
    {
        public string Name { get; set; }

        public string Quotes { get; set; }

        public string Body { get; set; }

        public int? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public int GameId { get; set; }

        [NotMapped]
        public virtual Game Game { get; set; }

        public virtual ICollection<Comment> ChildComments { get; set; }
    }
}
