using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.Publisher
{
    public class GetPublisherByCompanyNameQueryHandler : IQueryHandler<GetPublisherByCompanyNameQuery, PublisherQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetPublisherByCompanyNameQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public PublisherQueryResult Retrieve(GetPublisherByCompanyNameQuery query)
        {
            Validate(query);
            return
                Mapper.Map<Domain.Entities.Publisher, PublisherQueryResult>(
                    _db.Publishers.GetSingle(x => x.CompanyName == query.CompanyName));
        }

        private void Validate(GetPublisherByCompanyNameQuery query)
        {
            query.CompanyName.Argument(NameGetter.GetName(() => query.CompanyName))
                             .NotNull()
                             .NotWhiteSpace();
        }
    }
}
