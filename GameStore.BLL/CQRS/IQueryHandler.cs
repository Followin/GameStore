namespace GameStore.BLL.CQRS
{
    public interface IQueryHandler<in TParameter, out TResult>
        where TParameter : IQuery
        where TResult : IQueryResult
    {
        TResult Retrieve(TParameter query);
    }
}
