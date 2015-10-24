using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class PublisherCommandHandler : 
    #region interfaces
        ICommandHandler<CreatePublisherCommand>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public PublisherCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Execute(CreatePublisherCommand command)
        {
            Validate(command);

            var publisher = Mapper.Map<CreatePublisherCommand, Publisher>(command);

            _db.Publishers.Add(publisher);
            _db.Save();
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
            if (_db.Publishers.GetSingle(p => p.CompanyName == command.CompanyName) != null)
            {
                throw new ArgumentException(
                    "Publisher with such CompanyName already exist",
                    NameGetter.GetName(() => command.CompanyName));
            }
        }
#endregion
    }
}
