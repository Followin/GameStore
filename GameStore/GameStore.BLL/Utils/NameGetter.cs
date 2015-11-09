using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Utils
{
    public static class NameGetter
    {
        public static String GetName<T>(this Expression<Func<T>> property)
        {
            var expression = GetMemberInfo(property);
            string path = string.Concat(expression.Member.DeclaringType.FullName,
                ".", expression.Member.Name);
            var lastdotIndex = path.LastIndexOf('.');
            return lastdotIndex > -1 ? path.Substring(lastdotIndex + 1) : path;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}
