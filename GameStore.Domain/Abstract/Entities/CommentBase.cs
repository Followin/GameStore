using System;

namespace GameStore.Domain.Abstract.Entities
{
    public abstract class CommentBase : Entity<Int32>
    {
        public abstract String Name { get; set; }

        public abstract String Body { get; set; }

        public abstract Int32? ParentCommentId { get; set; }

        public abstract CommentBase ParentComment { get; set; }
    }
}