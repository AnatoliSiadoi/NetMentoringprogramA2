using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        // todo: feel free to add your code here
        public Expression<T> IncDecExpression<T>(Expression<T> node)
        {
            return base.VisitAndConvert(node, "");
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (IsExpectedNode(node))
            {
                var param = node.Left as ParameterExpression;
                var constant = node.Right as ConstantExpression;

                if (IsExpectedParameterAndConst(param, constant))
                {
                    switch (node.NodeType)
                    {
                        case ExpressionType.Add:
                            return Expression.Increment(param);

                        case ExpressionType.Subtract:
                            return Expression.Decrement(param);

                        default:
                            throw new InvalidOperationException("Invalid operation");
                    }
                }
            }

            return base.VisitBinary(node);
        }

        private bool IsExpectedNode(BinaryExpression node)
        {
            return (node.NodeType == ExpressionType.Add || node.NodeType == ExpressionType.Subtract)
                   && IsExpectedNodeTypes(node.Left.NodeType, node.Right.NodeType);
        }

        private bool IsExpectedNodeTypes(ExpressionType left, ExpressionType right)
        {
            return (left == ExpressionType.Parameter && right == ExpressionType.Constant);
        }

        private bool IsExpectedParameterAndConst(ParameterExpression param, ConstantExpression constant)
        {
            return (param != null && constant != null && constant.Type == typeof(int) 
                && (int)constant.Value == 1);
        }

    }
}
