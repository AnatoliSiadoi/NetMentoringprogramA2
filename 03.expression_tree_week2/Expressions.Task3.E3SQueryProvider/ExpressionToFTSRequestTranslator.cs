using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        readonly StringBuilder _resultStringBuilder;
        private TranslatorAction _action;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
            _action = TranslatorAction.Default;
        }

        public string Translate(Expression exp)
        {
            _resultStringBuilder.Append("[");
            Visit(exp);
            _resultStringBuilder.Append("]");

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            switch (node.Method.Name)
            {
                case "Contains":
                    _action = TranslatorAction.Contains;
                    break;
                case "StartsWith":
                    _action = TranslatorAction.StartsWith;
                    break;
                case "EndsWith":
                    _action = TranslatorAction.EndsWith;
                    break;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:

                    if (node.Left.NodeType == ExpressionType.MemberAccess && node.Right.NodeType == ExpressionType.Constant)
                    {
                        Visit(node.Left);
                        Visit(node.Right);
                    }
                    else if (node.Left.NodeType == ExpressionType.Constant && node.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        Visit(node.Right);
                        Visit(node.Left);
                    }

                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append(",");
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append($"\"{node.Member.Name}:");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            switch (_action)
            {
                case TranslatorAction.Default:
                    _resultStringBuilder.Append($"({node.Value})\"");
                    break;
                case TranslatorAction.Contains:
                    _resultStringBuilder.Append($"(*{node.Value}*)\"");
                    break;
                case TranslatorAction.StartsWith:
                    _resultStringBuilder.Append($"({node.Value}*)\"");
                    break;
                case TranslatorAction.EndsWith:
                    _resultStringBuilder.Append($"(*{node.Value})\"");
                    break;
                default:
                    throw new NotSupportedException($"Operation '{_action}' is not supported");
            }

            _action = TranslatorAction.Default;

            return node;
        }

        #endregion
    }
}
