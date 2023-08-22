using System.Linq.Expressions;

namespace Giantnodes.Infrastructure.Expressions;

// https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool
public static class ExpressionComposer
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Compose(left, right, Expression.And);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        return Compose(left, right, Expression.Or);
    }

    private static Expression<Func<T, bool>> Compose<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, BinaryExpression> @operator)
    {
        var leftParameter = left.Parameters[0];
        var rightParameter = right.Parameters[0];

        var visitor = new ReplaceParameterVisitor(rightParameter, leftParameter);

        return Expression.Lambda<Func<T, bool>>(@operator(left.Body, visitor.Visit(right.Body)), leftParameter);
    }

    private sealed class ReplaceParameterVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _old;
        private readonly ParameterExpression _new;

        public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
        {
            _old = oldParameter;
            _new = newParameter;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ReferenceEquals(node, _old) ? _new : base.VisitParameter(node);
        }
    }
}