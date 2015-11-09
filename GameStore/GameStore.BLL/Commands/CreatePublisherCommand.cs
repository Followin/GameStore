using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Commands
{
    public class CreatePublisherCommand : ICommand
    {
        public String CompanyName { get; set; }

        public String Description { get; set; }

        public String HomePage { get; set; }
    }
}
