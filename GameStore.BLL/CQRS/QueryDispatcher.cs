using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace GameStore.BLL.CQRS
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private IKernel _kernel;

        public QueryDispatcher(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult
        {
            var handler = _kernel.Get<IQueryHandler<TParameter, TResult>>();
            return handler.Retrieve(query);
        }
    }
}
