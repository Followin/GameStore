using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands.Publisher
{
    public class CreatePublisherCommand : ICommand
    {
        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }
    }
}
