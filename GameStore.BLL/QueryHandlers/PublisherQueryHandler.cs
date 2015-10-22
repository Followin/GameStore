using System.Collections.Generic;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;
using EntryState = GameStore.Domain.Abstract.EntryState;

namespace GameStore.BLL.QueryHandlers
{
    public class PublisherQueryHandler : 
    #region interfaces
        IQueryHandler<GetPublisherByCompanyNameQuery, PublisherQueryResult>,
        IQueryHandler<GetAllPublishersQuery, PublishersQueryResult>
    #endregion
    {
        private IGameStoreUnitOfWork db;
        private ILogger logger;

        public PublisherQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public PublisherQueryResult Retrieve(GetPublisherByCompanyNameQuery query)
        {
            Validate(query);
            return
                Mapper.Map<Publisher, PublisherQueryResult>(
                    db.Publishers.GetSingle(x => x.CompanyName == query.CompanyName));
        }

        public PublishersQueryResult Retrieve(GetAllPublishersQuery query)
        {
            logger.Debug("GetAllPublishers enter");
            return new PublishersQueryResult(
                          Mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherDTO>>(
                              db.Publishers.Get(p => p.EntryState == EntryState.Active)));
        }

#region validators
        private void Validate(GetPublisherByCompanyNameQuery query)
        {
            query.CompanyName.Argument("CompanyName")
                             .NotNull()
                             .NotWhiteSpace();
        }
#endregion

        
    }
}
