using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Comment
{
    public class CreateCommentCommand : ICommand
    {
        public string Name { get; set; }

        public string Quotes { get; set; }

        public string Body { get; set; }

        public int? GameId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}
