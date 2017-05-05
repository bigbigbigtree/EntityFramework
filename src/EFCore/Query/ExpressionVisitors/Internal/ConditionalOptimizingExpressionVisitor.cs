// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Remotion.Linq.Clauses.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class ConditionalOptimizingExpressionVisitor : ExpressionVisitorBase
    {
        private readonly NullCheckRewriteTestingExpressionVisitor _nullCheckRewriteTestingVisitor;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public ConditionalOptimizingExpressionVisitor([NotNull] QueryCompilationContext queryCompilationContext)
        {
            Check.NotNull(queryCompilationContext, nameof(queryCompilationContext));

            _nullCheckRewriteTestingVisitor = new NullCheckRewriteTestingExpressionVisitor(queryCompilationContext);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitConditional(ConditionalExpression conditionalExpression)
        {
            if (conditionalExpression.Test is BinaryExpression binaryExpression)
            {
                // Converts '[q] != null ? [q] : [s]' into '[q] ?? [s]'
                if (binaryExpression.NodeType == ExpressionType.NotEqual
                    && binaryExpression.Left is QuerySourceReferenceExpression querySourceReferenceExpression1
                    && binaryExpression.Right.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfTrue, querySourceReferenceExpression1))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts 'null != [q] ? [q] : [s]' into '[q] ?? [s]'
                if (binaryExpression.NodeType == ExpressionType.NotEqual
                    && binaryExpression.Right is QuerySourceReferenceExpression querySourceReferenceExpression2
                    && binaryExpression.Left.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfTrue, querySourceReferenceExpression2))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts '[q] == null ? [s] : [q]' into '[s] ?? [q]'
                if (binaryExpression.NodeType == ExpressionType.Equal
                    && binaryExpression.Left is QuerySourceReferenceExpression querySourceReferenceExpression3
                    && binaryExpression.Right.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfFalse, querySourceReferenceExpression3))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }

                // Converts 'null == [q] ? [s] : [q]' into '[s] ?? [q]'
                if (binaryExpression.NodeType == ExpressionType.Equal
                    && binaryExpression.Right is QuerySourceReferenceExpression querySourceReferenceExpression4
                    && binaryExpression.Left.IsNullConstantExpression()
                    && ReferenceEquals(conditionalExpression.IfFalse, querySourceReferenceExpression4))
                {
                    return Expression.Coalesce(conditionalExpression.IfTrue, conditionalExpression.IfFalse);
                }
            }

            if (conditionalExpression.IsNullPropagationCandidate(out var testExpression, out var resultExpression)
                && _nullCheckRewriteTestingVisitor.CanRewriteNullCheck(testExpression, resultExpression))
            {
                return new NullConditionalExpression(testExpression, testExpression, resultExpression);
            }

            return base.VisitConditional(conditionalExpression);
        }

        private class NullCheckRewriteTestingExpressionVisitor : NullCheckRewriteTestingExpressionVisitorBase
        {
            public NullCheckRewriteTestingExpressionVisitor(QueryCompilationContext queryCompilationContext)
                : base(queryCompilationContext)
            {
            }

            public override Expression Visit(Expression node)
                => CanRewrite == false
                || !(node is MemberExpression
                    || node is QuerySourceReferenceExpression
                    || node is MethodCallExpression
                    || node is TypeBinaryExpression
                    || node is UnaryExpression)
                ? node
                : base.Visit(node);

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsEFPropertyMethod())
                {
                    if (node.Arguments[0] is QuerySourceReferenceExpression querySource
                        && node.Arguments[1] is ConstantExpression propertyNameExpression
                        && (string)propertyNameExpression.Value == PropertyName)
                    {
                        CanRewrite = querySource.ReferencedQuerySource == QuerySource;

                        return node;
                    }
                }

                CanRewrite = false;

                return node;
            }

            protected override Expression VisitTypeBinary(TypeBinaryExpression node)
            {
                if (node.NodeType == ExpressionType.TypeAs)
                {
                    return Visit(node.Expression);
                }

                CanRewrite = false;

                return node;
            }
        }
    }
}
