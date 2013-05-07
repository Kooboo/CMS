#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.Linq;
using Kooboo.Extensions;
namespace Kooboo.CMS.Content.Persistence.Default.ContentQuery
{
    internal class OrderExpression
    {
        public Expression<Func<TextContent, object>> OrderExprssion { get; set; }
        public bool Descending { get; set; }
    }
    internal class QueryExpressionTranslator : Kooboo.CMS.Content.Query.Translator.ExpressionVisitor
    {
        public List<OrderExpression> OrderExpressions { get; set; }
        public Expression<Func<TextContent, bool>> LinqExpression { get; set; }
        public CallType CallType { get; set; }
        public IEnumerable<IContentQuery<TextContent>> CategoryQueries { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public QueryExpressionTranslator()
        {
            LinqExpression = it => true;

            CategoryQueries = new IContentQuery<TextContent>[0];
            OrderExpressions = new List<OrderExpression>();
        }
        public IQueryable<TextContent> Translate(IExpression expression, IQueryable<TextContent> contentQueryable)
        {
            this.Visite(expression);

            contentQueryable = contentQueryable.Where(this.LinqExpression);

            return contentQueryable;
        }
        public override void Visite(IExpression expression)
        {
            base.Visite(expression);
        }
        protected override void VisitSkip(Query.Expressions.SkipExpression expression)
        {
            Skip = expression.Count;
        }

        protected override void VisitTake(Query.Expressions.TakeExpression expression)
        {
            Take = expression.Count;
        }
        protected override void VisitOrder(Query.Expressions.OrderExpression expression)
        {
            OrderExpressions.Add(new OrderExpression() { OrderExprssion = it => it[expression.FieldName], Descending = expression.Descending });
            base.VisitOrder(expression);
        }

        protected override void VisitSelect(Query.Expressions.SelectExpression expression)
        {
            throw new NotSupportedException("Please instead of using IQueryable<TextContent>.Select()");
        }

        protected override void VisitCall(Query.Expressions.CallExpression expression)
        {
            this.CallType = expression.CallType;
        }

        protected override void VisitWhereBetweenOrEqual(Query.Expressions.WhereBetweenOrEqualExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereBetweenOrEqual(it[expression.FieldName], expression.Start, expression.End));
        }
        private bool WhereBetweenOrEqual(object value, object start, object end)
        {
            if (value is int)
            {
                int value1 = value.GetValue<int>(0);
                int startValue = start.GetValue<int>(0);
                int endValue = end.GetValue<int>(0);
                return (value1 >= startValue) && (value1 <= endValue);
            }
            else if (value is decimal)
            {
                decimal value1 = value.GetValue<decimal>(0);
                decimal startValue = start.GetValue<decimal>(0);
                decimal endValue = end.GetValue<decimal>(0);
                return (value1 >= startValue) && (value1 <= endValue);
            }
            else if (value is DateTime)
            {
                DateTime value1 = value.GetValue<DateTime>(DateTime.MinValue);
                DateTime startValue = start.GetValue<DateTime>(DateTime.MinValue);
                DateTime endValue = end.GetValue<DateTime>(DateTime.MinValue);
                return (value1 >= startValue) && (value1 <= endValue);
            }
            else
            {
                return false; //throw new NotSupportedException(string.Format("The type '{0}' does not support WhereBetweenOrEqual method", value == null ? "null" : value.GetType().FullName));
            }
        }

        protected override void VisitWhereBetween(Query.Expressions.WhereBetweenExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereBetween(it[expression.FieldName], expression.Start, expression.End));
        }
        private bool WhereBetween(object value, object start, object end)
        {
            if (value is int)
            {
                int value1 = value.GetValue<int>(0);
                int startValue = start.GetValue<int>(0);
                int endValue = end.GetValue<int>(0);
                return (value1 > startValue) && (value1 < endValue);
            }
            else if (value is decimal)
            {
                decimal value1 = value.GetValue<decimal>(0);
                decimal startValue = start.GetValue<decimal>(0);
                decimal endValue = end.GetValue<decimal>(0);
                return (value1 > startValue) && (value1 < endValue);
            }
            else if (value is DateTime)
            {
                DateTime value1 = value.GetValue<DateTime>(DateTime.MinValue);
                DateTime startValue = start.GetValue<DateTime>(DateTime.MinValue);
                DateTime endValue = end.GetValue<DateTime>(DateTime.MinValue);
                return (value1 > startValue) && (value1 < endValue);
            }
            else
            {
                return false;// throw new NotSupportedException(string.Format("The type '{0}' does not support WhereBetween method", value == null ? "null" : value.GetType().FullName));
            }
        }

        protected override void VisitWhereContains(Query.Expressions.WhereContainsExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereContains(it[expression.FieldName], expression.Value));
        }
        private bool WhereContains(object value, object value1)
        {
            string str1 = value == null ? "" : value.ToString();
            string str2 = value1 == null ? "" : value1.ToString();
            return str1.Contains(str2, StringComparison.CurrentCultureIgnoreCase);
        }

        protected override void VisitWhereEndsWith(Query.Expressions.WhereEndsWithExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => EndsWith(it[expression.FieldName], expression.Value));
        }
        private bool EndsWith(object value, object value1)
        {
            string str1 = value == null ? "" : value.ToString();
            string str2 = value1 == null ? "" : value1.ToString();
            return str1.EndsWith(str2, StringComparison.CurrentCultureIgnoreCase);
        }
        protected override void VisitWhereEquals(Query.Expressions.WhereEqualsExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereEquals(it[expression.FieldName], expression.Value));
        }
        private bool WhereEquals(object value, object value1)
        {
            if (value == value1)
            {
                return true;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            if (value is string)
            {
                return (value == null ? "" : value.ToString()).ToString().EqualsOrNullEmpty(value1 == null ? "" : value1.ToString(), StringComparison.OrdinalIgnoreCase);
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            return comparable1.Equals(comparable2);
        }

        protected override void VisitWhereClause(Query.Expressions.WhereClauseExpression expression)
        {
            throw new NotSupportedException();
        }

        protected override void VisitWhereGreaterThan(Query.Expressions.WhereGreaterThanExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereGreaterThan(it[expression.FieldName], expression.Value));
        }
        private bool WhereGreaterThan(object value, object value1)
        {
            if (value == value1)
            {
                return false;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            var result = comparable1.CompareTo(comparable2);
            return result > 0;
        }
        protected override void VisitWhereGreaterThanOrEqual(Query.Expressions.WhereGreaterThanOrEqualExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereGreaterThanOrEqual(it[expression.FieldName], expression.Value));
        }
        private bool WhereGreaterThanOrEqual(object value, object value1)
        {
            if (value == value1)
            {
                return true;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            var result = comparable1.CompareTo(comparable2);
            return result > 0 || result == 0;
        }

        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereLessThan(it[expression.FieldName], expression.Value));
        }
        private bool WhereLessThan(object value, object value1)
        {
            if (value == value1)
            {
                return false;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            var result = comparable1.CompareTo(comparable2);
            return result < 0;
        }
        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereLessThanOrEqual(it[expression.FieldName], expression.Value));
        }
        private bool WhereLessThanOrEqual(object value, object value1)
        {
            if (value == value1)
            {
                return true;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            var result = comparable1.CompareTo(comparable2);
            return result < 0 || result == 0;
        }

        protected override void VisitWhereStartsWith(Query.Expressions.WhereStartsWithExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereStartsWith(it[expression.FieldName], expression.Value));
        }
        private bool WhereStartsWith(object value, object value1)
        {
            string str1 = value == null ? "" : value.ToString();
            string str2 = value == null ? "" : value1.ToString();
            return str1.StartsWith(str2, StringComparison.CurrentCultureIgnoreCase);
        }
        protected override void VisitWhereNotEquals(Query.Expressions.WhereNotEqualsExpression expression)
        {
            LinqExpression = PredicateBuilder.And(LinqExpression, it => WhereNotEquals(it[expression.FieldName], expression.Value));
        }
        private bool WhereNotEquals(object value, object value1)
        {
            if (value == value1)
            {
                return true;
            }
            if (value == null || value1 == null)
            {
                return false;
            }
            IComparable comparable1 = (IComparable)value;
            IComparable comparable2 = (IComparable)Convert.ChangeType(value1, value.GetType());
            var result = comparable1.CompareTo(comparable2);
            return result != 0;
        }
        private Expression<Func<TextContent, bool>> VisitInner(IExpression expression)
        {
            var visitor = new QueryExpressionTranslator();
            visitor.Visite(expression);
            //combine the order expressions.
            this.OrderExpressions.AddRange(visitor.OrderExpressions);

            this.CategoryQueries = this.CategoryQueries.Concat(visitor.CategoryQueries);
            return visitor.LinqExpression;
        }
        protected override void VisitAndAlso(Query.Expressions.AndAlsoExpression expression)
        {
            Expression<Func<TextContent, bool>> leftClause = it => true;
            if (!(expression.Left is TrueExpression))
            {
                leftClause = VisitInner(expression.Left);
            }

            Expression<Func<TextContent, bool>> rightClause = it => true;
            if (!(expression.Right is TrueExpression))
            {
                rightClause = VisitInner(expression.Right);
            }

            LinqExpression = PredicateBuilder.And(LinqExpression, PredicateBuilder.And(leftClause, rightClause));
        }

        protected override void VisitOrElse(Query.Expressions.OrElseExpression expression)
        {
            Expression<Func<TextContent, bool>> leftClause = it => false;
            if (!(expression.Left is FalseExpression))
            {
                leftClause = VisitInner(expression.Left);
            }

            Expression<Func<TextContent, bool>> rightClause = it => false;
            if (!(expression.Right is FalseExpression))
            {
                rightClause = VisitInner(expression.Right);
            }

            var exp = PredicateBuilder.Or(leftClause, rightClause);

            LinqExpression = PredicateBuilder.And(LinqExpression, exp);
        }

        protected override void VisitWhereCategory(WhereCategoryExpression expression)
        {
            CategoryQueries = CategoryQueries.Concat(new[] { expression.CategoryQuery });
        }

        protected override void VisitFalse(FalseExpression expression)
        {
            this.LinqExpression = PredicateBuilder.And(LinqExpression, it => false);
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            this.LinqExpression = PredicateBuilder.And(LinqExpression, it => true);
        }
    }
}
