using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class ChangeParameterVisitor : ExpressionVisitor
    {
        private Dictionary<string, object> _parameters;

        public Expression<T> ChangeParameter<T>(Expression<T> expression, Dictionary<string, object> parameters)
        {
            if(parameters != null)
            {
                _parameters = parameters;
            }

            return base.VisitAndConvert(expression, "");
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var result = base.Visit(node.Body);

            if (result != null)
            {
                return Expression.Lambda(result, node.Parameters);
            }

            return base.VisitLambda(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_parameters.TryGetValue(node.Name, out var value) || _parameters != null)
            {
                return Expression.Constant(value);
            }

            return base.VisitParameter(node);
        }
    }

}
