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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Translator;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Persistence.Mysql.QueryProcessor
{
    public class OrderClause
    {
        public string FieldName { get; set; }
        public bool Descending { get; set; }
        public override int GetHashCode()
        {
            return FieldName.ToLower().GetHashCode();
        }
    }
    public class OrderClauseComparer : IEqualityComparer<OrderClause>
    {
        #region IEqualityComparer<OrderClause> Members

        public bool Equals(OrderClause x, OrderClause y)
        {
            if (string.Compare(x.FieldName, y.FieldName, true) == 0)
            {
                return true;
            }
            return x.Equals(y);
        }

        public int GetHashCode(OrderClause obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
    public class MysqlVisitor<T> : ExpressionVisitor
        where T : ContentBase
    {
        public MysqlVisitor()
            : this(new List<Parameter>())
        {
        }
        public MysqlVisitor(List<Parameter> parameters)
        {
            this.parameters = parameters;
            CategoryQueries = new IContentQuery<TextContent>[0];
        }

        public int Take { get; private set; }
        public int Skip { get; private set; }

        private StringBuilder whereClause = new StringBuilder();
        public string WhereClause
        {
            get
            {
                if (whereClause.Length == 0)
                {
                    return "1=1";
                }
                return whereClause.ToString();
            }
        }

        private List<Parameter> parameters = null;
        public List<Parameter> Parameters
        {
            get
            {
                return parameters;
            }
        }

        public string[] SelectFields { get; private set; }

        public CallType CallType { get; private set; }

        private List<OrderClause> orderClauses = new List<OrderClause>();
        public List<OrderClause> OrderClauses
        {
            get
            {
                return orderClauses;
            }
        }
        public IEnumerable<IContentQuery<TextContent>> CategoryQueries { get; set; }
        public string AppendParameter(object value)
        {
            string paraName = "?Param" + Parameters.Count;

            if (value is string)
            {
                string sValue = value.ToString().ToLower();
                if (sValue == "true")
                {
                    value = true;
                }
                if (sValue == "false")
                {
                    value = false;
                }
            }

            Parameters.Add(new Parameter() { Name = paraName, Value = value });

            return paraName;
        }

        protected override void VisitSkip(Query.Expressions.SkipExpression expression)
        {
            this.Skip = this.Skip + expression.Count;
        }

        protected override void VisitTake(Query.Expressions.TakeExpression expression)
        {
            this.Take = expression.Count;
        }

        protected override void VisitSelect(Query.Expressions.SelectExpression expression)
        {
            this.SelectFields = expression.Fields;
        }

        protected override void VisitOrder(Query.Expressions.OrderExpression expression)
        {
            OrderClauses.Add(new OrderClause() { FieldName = expression.FieldName, Descending = expression.Descending });
        }

        protected override void VisitCall(Query.Expressions.CallExpression expression)
        {
            this.CallType = expression.CallType;
        }

        protected override void VisitWhereBetweenOrEqual(Query.Expressions.WhereBetweenOrEqualExpression expression)
        {
            string paraName1 = AppendParameter(expression.Start);

            string paraName2 = AppendParameter(expression.End);

            whereClause.AppendFormat("(`{0}` >= {1} AND `{0}` <= {2})", expression.FieldName, paraName1, paraName2);
        }



        protected override void VisitWhereBetween(Query.Expressions.WhereBetweenExpression expression)
        {
            string paraName1 = AppendParameter(expression.Start);

            string paraName2 = AppendParameter(expression.End);
            whereClause.AppendFormat("(`{0}` > {1} AND `{0}` < {2})", expression.FieldName, paraName1, paraName2);
        }

        protected override void VisitWhereContains(Query.Expressions.WhereContainsExpression expression)
        {

            var value = expression.Value.ToString();
            if (!value.StartsWith("%"))
            {
                value = "%" + value;
            }
            if (!value.EndsWith("%"))
            {
                value = value + "%";
            }
            string paraName = AppendParameter(value);
            whereClause.AppendFormat("(`{0}` LIKE {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereEndsWith(Query.Expressions.WhereEndsWithExpression expression)
        {
            var value = expression.Value.ToString();
            if (!value.StartsWith("%"))
            {
                value = "%" + value;
            }
            string paraName = AppendParameter(value);
            whereClause.AppendFormat("(`{0}` LIKE {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereEquals(Query.Expressions.WhereEqualsExpression expression)
        {
            if (expression.Value == null)
            {
                whereClause.AppendFormat("(`{0}` IS NULL)", expression.FieldName);
            }
            else
            {
                string paraName = AppendParameter(expression.Value);
                whereClause.AppendFormat("(`{0}` = {1})", expression.FieldName, paraName);
            }

        }

        protected override void VisitWhereClause(Query.Expressions.WhereClauseExpression expression)
        {
            whereClause.AppendFormat("({0})", expression.WhereClause);
        }

        protected override void VisitWhereGreaterThan(Query.Expressions.WhereGreaterThanExpression expression)
        {
            string paraName = AppendParameter(expression.Value);
            whereClause.AppendFormat("(`{0}` > {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereGreaterThanOrEqual(Query.Expressions.WhereGreaterThanOrEqualExpression expression)
        {
            string paraName = AppendParameter(expression.Value);
            whereClause.AppendFormat("(`{0}` >= {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            string paraName = AppendParameter(expression.Value);
            whereClause.AppendFormat("(`{0}` < {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            string paraName = AppendParameter(expression.Value);
            whereClause.AppendFormat("(`{0}` <= {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereStartsWith(Query.Expressions.WhereStartsWithExpression expression)
        {
            var value = expression.Value.ToString();
            if (!value.EndsWith("%"))
            {
                value = value + "%";
            }
            string paraName = AppendParameter(value);
            whereClause.AppendFormat("(`{0}` LIKE {1})", expression.FieldName, paraName);
        }

        protected override void VisitWhereNotEquals(Query.Expressions.WhereNotEqualsExpression expression)
        {
            if (expression.Value == null)
            {
                whereClause.AppendFormat("(`{0}` IS NOT NULL)", expression.FieldName);
            }
            else
            {
                string paraName = AppendParameter(expression.Value);
                whereClause.AppendFormat("(`{0}` <> {1})", expression.FieldName, paraName);
            }
        }
        private MysqlVisitor<T> VisitInner(IExpression expression)
        {
            MysqlVisitor<T> visitor = new MysqlVisitor<T>(this.Parameters);
            visitor.Visite(expression);

            this.CategoryQueries = this.CategoryQueries.Concat(visitor.CategoryQueries);


            return visitor;
        }
        protected override void VisitAndAlso(Query.Expressions.AndAlsoExpression expression)
        {
            string leftClause = "";
            if (!(expression.Left is TrueExpression))
            {
                var leftVisitor = VisitInner(expression.Left);
                leftClause = leftVisitor.WhereClause;
            }

            string rightClause = "";
            if (!(expression.Right is TrueExpression))
            {
                var rightVisitor = VisitInner(expression.Right);
                rightClause = rightVisitor.WhereClause;
            }

            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format(" (({0}) AND ({1}))", leftClause, rightClause));
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                whereClause.AppendFormat(string.Format("({0})", leftClause));
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("({0})", rightClause));
            }
        }

        protected override void VisitOrElse(Query.Expressions.OrElseExpression expression)
        {
            string leftClause = "";
            if (!(expression.Left is FalseExpression))
            {
                var leftVisitor = VisitInner(expression.Left);
                leftClause = leftVisitor.WhereClause;
            }

            string rightClause = "";
            if (!(expression.Right is FalseExpression))
            {
                var rightVisitor = VisitInner(expression.Right);
                rightClause = rightVisitor.WhereClause;
            }

            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("(({0}) OR ({1}))", leftClause, rightClause));
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                whereClause.AppendFormat(string.Format("({0})", leftClause));
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("({0})", rightClause));
            }
        }

        protected override void VisitWhere(IWhereExpression expression)
        {
            if (!((expression is WhereInExpression) || (expression is WhereNotInExpression)))
            {
                if (whereClause.Length > 0)
                {
                    whereClause.AppendFormat(" AND ");
                }
            }
            base.VisitWhere(expression);
        }
        protected override void VisitWhereCategory(WhereCategoryExpression expression)
        {
            CategoryQueries = CategoryQueries.Concat(new[] { expression.CategoryQuery });
        }

        protected override void VisitFalse(FalseExpression expression)
        {
            whereClause.Append(" (1<>1) ");
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            whereClause.Append(" (1=1) ");
        }
    }
}
