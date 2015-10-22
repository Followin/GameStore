using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    /// <summary>
    /// Dispatches queries
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Find a right handler and execute query
        /// </summary>
        /// <typeparam name="TParameter">Query type</typeparam>
        /// <typeparam name="TResult">QueryResult type</typeparam>
        /// <param name="query">query object</param>
        /// <returns>query result</returns>
        TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult;
    }
}
