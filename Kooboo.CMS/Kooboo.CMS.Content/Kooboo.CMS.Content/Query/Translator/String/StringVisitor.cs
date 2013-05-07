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
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Content.Query.Translator.String
{
    public class StringVisitor : ExpressionVisitor
    {
        private class InnerQuery : TranslatedQuery
        { }
        string selectText = "*";
        StringBuilder clauseText = new StringBuilder();
        StringBuilder sortText = new StringBuilder();
        TranslatedQuery query;
        public StringVisitor(TranslatedQuery query)
        {
            this.query = query;
        }
        public override void Visite(Expressions.IExpression expression)
        {
            selectText = "*";
            clauseText = new StringBuilder();
            sortText = new StringBuilder();

            base.Visite(expression);

            query.SelectText = selectText.ToString();
            query.ClauseText = clauseText.ToString();
            query.SortText = sortText.ToString();
        }
        protected override void VisitSkip(Expressions.SkipExpression expression)
        {
            query.Skip = expression.Count;
        }

        protected override void VisitSelect(Expressions.SelectExpression expression)
        {
            selectText = string.Join(",", expression.Fields);
        }

        protected override void VisitOrder(Expressions.OrderExpression expression)
        {
            sortText.AppendFormat("{0} {1}", expression.FieldName, expression.Descending ? "desc" : "asc");
        }

        protected override void VisitCall(Expressions.CallExpression expression)
        {
            query.CallType = expression.CallType;
        }
        protected override void VisitWhere(Expressions.IWhereExpression expression)
        {
            if (clauseText.Length != 0)
            {
                clauseText.Append("AND ");
            }
            base.VisitWhere(expression);
        }
        protected override void VisitWhereBetweenOrEqual(Expressions.WhereBetweenOrEqualExpression expression)
        {
            clauseText.AppendFormat("{0} <= {1} >= {2}", expression.Start, expression.FieldName, expression.End);
        }

        protected override void VisitWhereBetween(Expressions.WhereBetweenExpression expression)
        {
            clauseText.AppendFormat("{0} < {1} > {2}", expression.Start, expression.FieldName, expression.End);
        }

        protected override void VisitWhereContains(Expressions.WhereContainsExpression expression)
        {
            clauseText.AppendFormat("{0} Contains {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereEndsWith(Expressions.WhereEndsWithExpression expression)
        {
            clauseText.AppendFormat("{0} EndsWith {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereEquals(Expressions.WhereEqualsExpression expression)
        {
            clauseText.AppendFormat("{0} = {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereClause(Expressions.WhereClauseExpression expression)
        {
            clauseText.AppendFormat("{0}", expression.WhereClause);
        }

        protected override void VisitWhereGreaterThan(Expressions.WhereGreaterThanExpression expression)
        {
            clauseText.AppendFormat("{0} > {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereGreaterThanOrEqual(Expressions.WhereGreaterThanOrEqualExpression expression)
        {
            clauseText.AppendFormat("{0} >= {1}", expression.FieldName, expression.Value);
        }
        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            clauseText.AppendFormat("{0} < {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            clauseText.AppendFormat("{0} <= {1}", expression.FieldName, expression.Value);
        }
        protected override void VisitWhereStartsWith(Expressions.WhereStartsWithExpression expression)
        {
            clauseText.AppendFormat("{0} StartsWith {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitWhereNotEquals(Expressions.WhereNotEqualsExpression expression)
        {
            clauseText.AppendFormat("{0} <> {1}", expression.FieldName, expression.Value);
        }

        protected override void VisitAndAlso(Expressions.AndAlsoExpression expression)
        {
            string leftClause = "";
            if (!(expression.Left is TrueExpression))
            {
                leftClause = VisitInner(expression.Left).ClauseText;
            }

            string rightClause = "";
            if (!(expression.Right is TrueExpression))
            {
                rightClause = VisitInner(expression.Right).ClauseText;
            }

            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                clauseText.AppendFormat("(({0}) AND ({1}))", leftClause, rightClause);
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                clauseText.Append(leftClause);
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                clauseText.Append(rightClause);
            }
        }

        protected override void VisitOrElse(Expressions.OrElseExpression expression)
        {
            string leftClause = "";
            if (!(expression.Left is TrueExpression))
            {
                leftClause = VisitInner(expression.Left).ClauseText;
            }

            string rightClause = "";
            if (!(expression.Right is TrueExpression))
            {
                rightClause = VisitInner(expression.Right).ClauseText;
            }

            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                clauseText.AppendFormat("(({0}) OR ({1}))", leftClause, rightClause);
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                clauseText.Append(leftClause);
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                clauseText.Append(rightClause);
            }
        }

        private TranslatedQuery VisitInner(IExpression expression)
        {
            InnerQuery query = new InnerQuery();

            StringVisitor visitor = new StringVisitor(query);
            visitor.Visite(expression);
            //combine the order expressions.
            this.OrderFields.AddRange(visitor.OrderFields);

            return query;
        }

        protected override void VisitTake(TakeExpression expression)
        {
            query.TakeCount = expression.Count;
        }

        protected override void VisitWhereCategory(WhereCategoryExpression expression)
        {
            if (query is TranslatedTextContentQuery)
            {
                TranslatedTextContentQuery textContentQuery = (TranslatedTextContentQuery)query;

                var categoryQuery = (new TextContentQueryTranslator()).Translate(expression.CategoryQuery);

                textContentQuery.CategroyQueries = textContentQuery.CategroyQueries.Concat(new[] { (TranslatedTextContentQuery)categoryQuery });
            }
        }

        protected override void VisitFalse(FalseExpression expression)
        {
            clauseText.AppendFormat("(false)");
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            clauseText.AppendFormat("(true)");
        }

        protected override void VisitWhereIn(WhereInExpression expression)
        {
            clauseText.AppendFormat("({0} IN ({1}))", expression.FieldName, string.Join(",", expression.Values));
        }
    }
}
