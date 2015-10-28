using System.Linq.Expressions;

namespace GameStore.BLL.GameExpressionPipeline
{
    public interface IExpression<T> where T : Expression
    {
        T Execute(T item);
        void Register(IExpression<T> nextItem);
    }
}
