// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;

namespace Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public abstract class NullCheckRewriteTestingExpressionVisitorBase : ExpressionVisitorBase
    {
        private readonly QueryCompilationContext _queryCompilationContext;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public NullCheckRewriteTestingExpressionVisitorBase([NotNull] QueryCompilationContext queryCompilationContext)
        {
            Check.NotNull(queryCompilationContext, nameof(queryCompilationContext));

            _queryCompilationContext = queryCompilationContext;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual IQuerySource QuerySource { get; [param: NotNull] set; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual string PropertyName { get; [param: NotNull] set; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual bool? CanRewrite { get; [param: NotNull] set; }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual bool CanRewriteNullCheck(
            [NotNull] Expression testExpression, 
            [NotNull] Expression resultExpression)
        {
            Check.NotNull(testExpression, nameof(testExpression));
            Check.NotNull(resultExpression, nameof(resultExpression));

            AnalyzeTestExpression(testExpression);
            if (QuerySource == null)
            {
                return false;
            }

            Visit(resultExpression);

            return CanRewrite ?? false;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual void AnalyzeTestExpression([NotNull] Expression expression)
        {
            Check.NotNull(expression, nameof(expression));

            var processedExpression = expression.RemoveConvert();
            if (processedExpression is QuerySourceReferenceExpression querySourceReferenceExpression)
            {
                QuerySource = querySourceReferenceExpression.ReferencedQuerySource;
                PropertyName = null;

                return;
            }

            if (processedExpression is MemberExpression memberExpression
                && memberExpression.Expression.RemoveConvert() is QuerySourceReferenceExpression querySourceInstance)
            {
                QuerySource = querySourceInstance.ReferencedQuerySource;
                PropertyName = memberExpression.Member.Name;

                return;
            }

            if (processedExpression is MethodCallExpression methodCallExpression
                && methodCallExpression.Method.IsEFPropertyMethod())
            {
                if (methodCallExpression.Arguments[0] is QuerySourceReferenceExpression querySourceCaller)
                {
                    if (methodCallExpression.Arguments[1] is ConstantExpression propertyNameExpression)
                    {
                        QuerySource = querySourceCaller.ReferencedQuerySource;
                        PropertyName = (string)propertyNameExpression.Value;

                        // compensate for case when optimization is not possible on the QueryModel level and entity == null gets converted to entity.Key == null
                        if ((_queryCompilationContext.FindEntityType(QuerySource)
                             ?? _queryCompilationContext.Model.FindEntityType(QuerySource.ItemType))
                            ?.FindProperty(PropertyName)?.IsPrimaryKey()
                            ?? false)
                        {
                            PropertyName = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            CanRewrite
                = expression.ReferencedQuerySource == QuerySource
                  && PropertyName == null;

            return expression;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member.Name == PropertyName)
            {
                if (node.Expression is QuerySourceReferenceExpression querySource)
                {
                    CanRewrite = querySource.ReferencedQuerySource == QuerySource;

                    return node;
                }
            }

            return base.VisitMember(node);
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert)
            {
                return Visit(node.Operand);
            }

            CanRewrite = false;

            return node;
        }
    }
}
