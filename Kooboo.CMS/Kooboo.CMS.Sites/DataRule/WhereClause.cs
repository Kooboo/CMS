using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Sites.DataRule
{
    public enum Logical
    {
        And = 0,
        Or = 1,
        ThenAnd = 2,
        ThenOr = 3,
    }
    public enum Operator
    {
        Equals = 0,
        NotEquals = 1,
        GreaterThan = 2,
        LessThan = 3,
        Contains = 4,
        StartsWith = 5,
        EndsWith = 6,
        Between = 7
    }
    public class WhereClause
    {
        public Logical Logical { get; set; }
        public string FieldName { get; set; }
        public Operator Operator { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
    public static class WhereClauseToContentQueryHelper
    {
        //public static IContentQuery<ContentBase> Parse(this WhereClause clause, IContentQuery<ContentBase> contentQuery, DataRuleContext dataRuleContext)
        //{
        //    var expression = GetExpression(null, clause, dataRuleContext);
        //    if (clause.Logical == Logical.Or)
        //    {
        //        return contentQuery.Or(expression);
        //    }
        //    else
        //    {
        //        return contentQuery.Where(expression);
        //    }
        //}
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
            var value = ParameterizedFieldValue.GetFieldValue(clause.Value1, valueProvider);
            var fieldValue = Kooboo.CMS.Content.Models.Binder.TextContentBinder.DefaultBinder.ConvertToColumnType(schema, clause.FieldName, value);
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
                    var fieldValue2 = Kooboo.CMS.Content.Models.Binder.TextContentBinder.DefaultBinder.ConvertToColumnType(schema, clause.Value2, value2);
                    return new WhereBetweenExpression(inner, clause.FieldName, fieldValue, fieldValue2);
                default:
                    return null;
            }
        }

    }
}
