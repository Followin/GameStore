﻿using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class PublisherCommandHandler : 
        ICommandHandler<CreatePublisherCommand>
    {
        private IGameStoreUnitOfWork db;
        private ILogger logger;

        public PublisherCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public void Execute(CreatePublisherCommand command)
        {
            Validate(command);

            var publisher = Mapper.Map<CreatePublisherCommand, Publisher>(command);

            db.Publishers.Add(publisher);
            db.Save();
        }

#region validation

        private void Validate(CreatePublisherCommand command)
        {
            command.CompanyName.Argument("CompanyName")
                               .NotNull()
                               .NotWhiteSpace();
            command.Description.Argument("Description")
                               .NotNull()
                               .NotWhiteSpace();
            command.HomePage.Argument("HomePage")
                            .NotNull()
                            .NotWhiteSpace();
            if (db.Publishers.GetSingle(p => p.CompanyName == command.CompanyName) != null)
                throw new ArgumentException("Publisher with such CompanyName already exist", "CompanyName");
        }
#endregion
    }
}
