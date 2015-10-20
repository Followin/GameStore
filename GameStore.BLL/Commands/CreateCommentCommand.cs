using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands
{
    public class CreateCommentCommand : ICommand
    {
        public String Name { get; set; }
        public String Body { get; set; }
        public Int32? GameId { get; set; }
        public Int32? ParentCommentId { get; set; }
    }
}
