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
using Kooboo.CMS.Content.Query.Translator;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using QueryBuilder = MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using MongoDB.Driver;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Query
{
    public class MongoDBVisitor : ExpressionVisitor
    {
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public CallType CallType { get; private set; }
        public IEnumerable<IContentQuery<TextContent>> CategoryQueries { get; private set; }
        public IMongoQuery MongoQuery { get; private set; }
        public MongoDBVisitor()
        {
            CategoryQueries = new IContentQuery<TextContent>[0];
        }
        public void SetQuery(IMongoQuery mongoQuery)
        {
            if (this.MongoQuery != null)
            {
                this.MongoQuery = QueryBuilder.Query.And(this.MongoQuery, mongoQuery);
            }
            else
            {
                this.MongoQuery = mongoQuery;
            }
        }
        protected override void VisitOrder(OrderExpression expression)
        {
            OrderFields.Add(new OrderField() { FieldName = ModelExtensions.GetCaseInsensitiveFieldName(expression.FieldName), Descending = expression.Descending });
        }
        protected override void VisitSkip(Content.Query.Expressions.SkipExpression expression)
        {
            Skip = expression.Count;
        }

        protected override void VisitTake(Content.Query.Expressions.TakeExpression expression)
        {
            Take = expression.Count;
        }

        protected override void VisitSelect(Content.Query.Expressions.SelectExpression expression)
        {
            throw new NotSupportedException();
        }

        protected override void VisitCall(Content.Query.Expressions.CallExpression expression)
        {
            this.CallType = expression.CallType;
        }

        protected override void VisitWhereCategory(Content.Query.Expressions.WhereCategoryExpression expression)
        {
            CategoryQueries = CategoryQueries.Concat(new[] { expression.CategoryQuery });
        }

        protected override void VisitWhereBetweenOrEqual(Content.Query.Expressions.WhereBetweenOrEqualExpression expression)
        {
            var query = QueryBuilder.Query.And(QueryBuilder.Query.GTE(expression.FieldName, BsonHelper.Create(expression.Start)),
               QueryBuilder.Query.LTE(expression.FieldName, BsonHelper.Create(expression.End)));
            SetQuery(query);
        }

        protected override void VisitWhereBetween(Content.Query.Expressions.WhereBetweenExpression expression)
        {
            var query = QueryBuilder.Query.And(QueryBuilder.Query.GT(expression.FieldName, BsonHelper.Create(expression.Start)),
                QueryBuilder.Query.LT(expression.FieldName, BsonHelper.Create(expression.End)));
            SetQuery(query);
        }

        protected override void VisitWhereContains(Content.Query.Expressions.WhereContainsExpression expression)
        {
            var query = QueryBuilder.Query.Matches(expression.FieldName, new BsonRegularExpression(Regex.Escape(expression.Value.ToString()), "i"));
            SetQuery(query);
        }

        protected override void VisitWhereEndsWith(Content.Query.Expressions.WhereEndsWithExpression expression)
        {
            var query = QueryBuilder.Query.Matches(expression.FieldName, new BsonRegularExpression(Regex.Escape(expression.Value.ToString()) + "$", "i"));
            SetQuery(query);
        }

        protected override void VisitWhereEquals(Content.Query.Expressions.WhereEqualsExpression expression)
        {
            var query = MongoDBHelper.EQIgnoreCase(expression.FieldName, expression.Value);
            SetQuery(query);
        }

        protected override void VisitWhereClause(Content.Query.Expressions.WhereClauseExpression expression)
        {
            var query = QueryBuilder.Query.Where(expression.WhereClause);
            SetQuery(query);
        }

        protected override void VisitWhereGreaterThan(Content.Query.Expressions.WhereGreaterThanExpression expression)
        {
            var query = QueryBuilder.Query.GT(expression.FieldName, BsonHelper.Create(expression.Value));
            SetQuery(query);
        }

        protected override void VisitWhereGreaterThanOrEqual(Content.Query.Expressions.WhereGreaterThanOrEqualExpression expression)
        {
            var query = QueryBuilder.Query.GTE(expression.FieldName, BsonHelper.Create(expression.Value));
            SetQuery(query);
        }
        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            var query = QueryBuilder.Query.LT(expression.FieldName, BsonHelper.Create(expression.Value));
            SetQuery(query);
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            var query = QueryBuilder.Query.LTE(expression.FieldName, BsonHelper.Create(expression.Value));
            SetQuery(query);
        }
        protected override void VisitWhereStartsWith(Content.Query.Expressions.WhereStartsWithExpression expression)
        {
            var query = QueryBuilder.Query.Matches(expression.FieldName, new BsonRegularExpression("^" + Regex.Escape(expression.Value.ToString()), "i"));
            SetQuery(query);
        }

        protected override void VisitWhereNotEquals(Content.Query.Expressions.WhereNotEqualsExpression expression)
        {

            var query = MongoDBHelper.NEIgnoreCase(expression.FieldName, expression.Value);
            SetQuery(query);

        }
        protected override void VisitNot(NotExpression expression)
        {
            if (expression.InnerExpression != null)
            {
                var rightClause = VisitInner(expression.InnerExpression);              
                SetQuery(QueryBuilder.Query.Not(rightClause));
            }
        }
        protected override void VisitAndAlso(Content.Query.Expressions.AndAlsoExpression expression)
        {
            var andAlso = (AndAlsoExpression)expression;

            IMongoQuery leftClause = null;
            if (!(andAlso.Left is TrueExpression))
            {
                leftClause = VisitInner(andAlso.Left);
            }

            IMongoQuery rightClause = null;
            if (!(andAlso.Right is TrueExpression))
            {
                rightClause = VisitInner(andAlso.Right);
            }

            if (leftClause != null && rightClause != null)
            {
                var queryComplete = QueryBuilder.Query.And(leftClause, rightClause);
                SetQuery(queryComplete);
            }
            else if (leftClause != null)
            {
                SetQuery(leftClause);
            }
            else if (rightClause != null)
            {
                SetQuery(rightClause);
            }

        }

        protected override void VisitOrElse(Content.Query.Expressions.OrElseExpression expression)
        {

            IMongoQuery leftClause = null;
            if (!(expression.Left is TrueExpression))
            {
                leftClause = VisitInner(expression.Left);
            }

            IMongoQuery rightClause = null;
            if (!(expression.Right is TrueExpression))
            {
                rightClause = VisitInner(expression.Right);
            }

            if (leftClause != null && rightClause != null)
            {
                var queryComplete = QueryBuilder.Query.Or(leftClause, rightClause);
                SetQuery(queryComplete);
            }
            else if (leftClause != null)
            {
                SetQuery(leftClause);
            }
            else if (rightClause != null)
            {
                SetQuery(rightClause);
            }

        }

        private IMongoQuery VisitInner(IExpression expression)
        {
            MongoDBVisitor visitor = new MongoDBVisitor();
            visitor.Visite(expression);
            //combine the order expressions.
            this.OrderFields.AddRange(visitor.OrderFields);

            this.CategoryQueries = this.CategoryQueries.Concat(visitor.CategoryQueries);
            return visitor.MongoQuery;
        }


        protected override void VisitFalse(FalseExpression expression)
        {
            SetQuery(QueryBuilder.Query.Where(new BsonJavaScript("false")));
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            SetQuery(QueryBuilder.Query.Where(new BsonJavaScript("true")));
        }
        protected override void VisitWhereIn(WhereInExpression expression)
        {
            var query = QueryBuilder.Query.In(expression.FieldName, expression.Values.Select(it => BsonHelper.Create(it)).ToArray());
            SetQuery(query);
        }
        protected override void VisitWhereNotIn(WhereNotInExpression expression)
        {
            var query = QueryBuilder.Query.NotIn(expression.FieldName, expression.Values.Select(it => BsonHelper.Create(it)).ToArray());
            SetQuery(query);
        }
    }
}
