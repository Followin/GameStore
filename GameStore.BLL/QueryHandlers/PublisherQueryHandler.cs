using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;
using GameStore.BLL.QueryResults;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.QueryHandlers
{
    public class PublisherQueryHandler : 
        IQueryHandler<GetPublisherByCompanyNameQuery, PublisherQueryResult>
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
