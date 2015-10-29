using System.Collections.Generic;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.BLL.Utils;
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
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public PublisherQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
           _db = db;
            _logger = logger;
        }

        public PublisherQueryResult Retrieve(GetPublisherByCompanyNameQuery query)
        {
            Validate(query);
            return
                Mapper.Map<Publisher, PublisherQueryResult>(
                    _db.Publishers.GetSingle(x => x.CompanyName == query.CompanyName));
        }

        public PublishersQueryResult Retrieve(GetAllPublishersQuery query)
        {
            _logger.Debug("GetAllPublishers enter");
            return new PublishersQueryResult(
                          Mapper.Map<IEnumerable<Publisher>, IEnumerable<PublisherDTO>>(
                              _db.Publishers.Get(p => p.EntryState == EntryState.Active)));
        }

#region validators
        private void Validate(GetPublisherByCompanyNameQuery query)
        {
            query.CompanyName.Argument(NameGetter.GetName(() => query.CompanyName))
                             .NotNull()
                             .NotWhiteSpace();
        }
#endregion
    }
}
