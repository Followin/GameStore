using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Publisher
{
    public class CreatePublisherCommandHandler : ICommandHandler<CreatePublisherCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CreatePublisherCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CreatePublisherCommand command)
        {
            Validate(command);

            var publisher = Mapper.Map<CreatePublisherCommand, Domain.Entities.Publisher>(command);

            _db.Publishers.Add(publisher);
            _db.Save();

            return new CommandResult();
        }

        #region validation

        private void Validate(CreatePublisherCommand command)
        {
            command.CompanyName.Argument(NameGetter.GetName(() => command.CompanyName))
                               .NotNull()
                               .NotWhiteSpace();
            command.Description.Argument(NameGetter.GetName(() => command.Description))
                               .NotNull()
                               .NotWhiteSpace();
            command.HomePage.Argument(NameGetter.GetName(() => command.HomePage))
                            .NotNull()
                            .NotWhiteSpace();
            if (_db.Publishers.GetFirst(p => p.CompanyName == command.CompanyName) != null)
            {
                throw new ArgumentException(
                    "Publisher with such CompanyName already exist",
                    NameGetter.GetName(() => command.CompanyName));
            }
        }
        #endregion
    }
}
