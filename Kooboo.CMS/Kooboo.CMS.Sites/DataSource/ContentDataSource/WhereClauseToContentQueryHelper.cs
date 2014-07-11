#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Query.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    public static class WhereClauseToContentQueryHelper
    {
        public static IWhereExpression Parse(this IEnumerable<WhereClause> clauses, Schema schema, IValueProvider valueProvider)
        {
            IWhereExpression expression = null;
            IWhereExpression thenRight = null;
            Logical defaultValue = Logical.And;
            Logical? thenLogical = null;
            Logical lastLogical = defaultValue;
            foreach (var clause in clauses)
            {
                var exp = GetExpression(null, clause, schema, valueProvider);
                if (exp == null)
                {
                    continue;
                }
                thenRight = BuildLogicalExpression(lastLogical, thenRight, exp);
                switch (clause.Logical)
                {
                    case Logical.And:
                    case Logical.Or:
                        lastLogical = clause.Logical;
                        break;
                    case Logical.ThenAnd:
                        if (thenLogical.HasValue)
                        {
                            expression = BuildLogicalExpression(thenLogical.Value, expression, thenRight);
                        }
                        else
                        {
                            expression = thenRight;
                        }
                        thenRight = null;
                        thenLogical = Logical.And;
                        lastLogical = defaultValue;
                        break;
                    case Logical.ThenOr:
                        if (thenLogical.HasValue)
                        {
                            expression = BuildLogicalExpression(thenLogical.Value, expression, thenRight);
                        }
                        else
                        {
                            expression = thenRight;
                        }
                        thenRight = null;
                        thenLogical = Logical.Or;
                        lastLogical = defaultValue;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            if (thenLogical.HasValue)
            {
                expression = BuildLogicalExpression(thenLogical.Value, expression, thenRight);
            }
            else
            {
                expression = thenRight;
            }
            return expression;
        }
        /// <summary>
        /// Build And/Or Expression
        /// </summary>
        /// <param name="logical"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static IWhereExpression BuildLogicalExpression(Logical logical, IWhereExpression left, IWhereExpression right)
        {
            //And/Or
            switch (logical)
            {
                case Logical.And:
                    return new AndAlsoExpression(left, right);
                case Logical.Or:
                    return new OrElseExpression(left, right);
                default:
                    throw new NotSupportedException();
            }
        }
        private static IWhereExpression GetExpression(IWhereExpression inner, WhereClause clause, Schema schema, IValueProvider valueProvider)
        {
            var binder = EngineContext.Current.Resolve<ITextContentBinder>();
            var value = ParameterizedFieldValue.GetFieldValue(clause.Value1, valueProvider);
            var fieldValue = binder.ConvertToColumnType(schema, clause.FieldName, value);
            if (clause.Logical == Logical.Or && string.IsNullOrEmpty(value))
            {
                return null;
            }
            switch (clause.Operator)
            {
                case Operator.Equals:
                    return new WhereEqualsExpression(inner, clause.FieldName, fieldValue);
                case Operator.NotEquals:
                    return new WhereNotEqualsExpression(inner, clause.FieldName, fieldValue);
                case Operator.GreaterThan:
                    return new WhereGreaterThanExpression(inner, clause.FieldName, fieldValue);
                case Operator.LessThan:
                    return new WhereLessThanExpression(inner, clause.FieldName, fieldValue);
                case Operator.Contains:
                    return new WhereContainsExpression(inner, clause.FieldName, fieldValue);
                case Operator.StartsWith:
                    return new WhereStartsWithExpression(inner, clause.FieldName, fieldValue);
                case Operator.EndsWith:
                    return new WhereEndsWithExpression(inner, clause.FieldName, fieldValue);
                case Operator.Between:
                    var value2 = ParameterizedFieldValue.GetFieldValue(clause.Value2, valueProvider);
                    var fieldValue2 = binder.ConvertToColumnType(schema, clause.Value2, value2);
                    return new WhereBetweenExpression(inner, clause.FieldName, fieldValue, fieldValue2);
                case Operator.NotNull:
                    return new AndAlsoExpression(new WhereNotEqualsExpression(inner, clause.FieldName, null), new WhereNotEqualsExpression(inner, clause.FieldName, ""));
                case Operator.IsNull:
                    return new OrElseExpression(new WhereEqualsExpression(inner, clause.FieldName, null), new WhereEqualsExpression(inner, clause.FieldName, ""));
                default:
                    return null;
            }
        }
    }
}
