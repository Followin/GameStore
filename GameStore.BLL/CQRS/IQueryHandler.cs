namespace GameStore.BLL.CQRS
{
    /// <summary>
    /// Handles queries and retrives results
    /// </summary>
    /// <typeparam name="TParameter">Query type</typeparam>
    /// <typeparam name="TResult">Query result type</typeparam>
    public interface IQueryHandler<in TParameter, out TResult>
        where TParameter : IQuery
        where TResult : IQueryResult
    {
        /// <summary>
        /// Executes query
        /// </summary>
        /// <param name="query">Query object</param>
        /// <returns>Query result</returns>
        TResult Retrieve(TParameter query);
    }
}
