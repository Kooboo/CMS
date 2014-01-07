using Kooboo.CMS.Content.Query.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
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

    public class CouchbaseVisitor : ExpressionVisitor
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public CallType CallType { get; private set; }
        public IEnumerable<IContentQuery<TextContent>> CategoryQueries { get; private set; }
        private StringBuilder whereClause = new StringBuilder();
        public string WhereClause
        {
            get
            {
                return whereClause.Length > 0 ?
                    whereClause.ToString() : string.Empty;
            }
        }
        private StringBuilder viewNameBuilder = new StringBuilder();
        public string ViewName
        {
            get
            {
                return viewNameBuilder.ToString();
            }
        }

        //optimize the query for UUID.  单独为根据UUID查询做优化，如果没有其他条件就不需要建动态View了。
        private List<string> eqUUIDs = new List<string>();
        public IEnumerable<string> EQUUIDs { get { return eqUUIDs; } }

        //optimize the query for UserKey. 单独为UserKey查询做优化，所有根据UserKey查询的都用一个固定的View.
        private List<string> eqUserKeys = new List<string>();
        public IEnumerable<string> EQUserKeys { get { return eqUserKeys; } }

        public OrderClause OrderClause
        {
            get;
            set;
        }

        public CouchbaseVisitor()
        {
            CategoryQueries = new IContentQuery<TextContent>[0];
        }

        public override void Visite(IExpression expression)
        {
            base.Visite(expression);
        }

        protected override void VisitOrder(OrderExpression expression)
        {
            OrderClause = new OrderClause() { FieldName = expression.FieldName, Descending = expression.Descending };
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

        public string MakeValue(object val)
        {
            if (val == null)
            {
                return "null";
            }
            else if (val is String)
            {
                val = "\\\"" + ((string)val).Replace("\"", "\\\\\\\"") + "\\\"";
            }
            else if (val is bool)
            {
                val = val.ToString().ToLower();
            }
            return val.ToString();
        }
        public string AsViewNameString(object val)
        {
            if (val == null)
            {
                return "null";
            }

            return val.ToString().ToLower().Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }
        protected override void VisitWhereBetween(WhereBetweenExpression expression)
        {
            this.whereClause.AppendFormat("(doc[{0}]>{1}&&doc[{0}]<{2})", MakeValue(expression.FieldName), MakeValue(expression.Start), MakeValue(expression.End));
            this.viewNameBuilder.AppendFormat("{1}_LT_{0}_GT_{2}", expression.FieldName, AsViewNameString(expression.Start), AsViewNameString(expression.End));
        }

        protected override void VisitWhereBetweenOrEqual(WhereBetweenOrEqualExpression expression)
        {
            this.whereClause.AppendFormat("(doc[{0}]>={1}&&doc[{0}]<={2})", MakeValue(expression.FieldName), MakeValue(expression.Start), MakeValue(expression.End));
            this.viewNameBuilder.AppendFormat("{1}_LTE_{0}_GTE_{2}", expression.FieldName, AsViewNameString(expression.Start), AsViewNameString(expression.End));
        }

        protected override void VisitWhereContains(WhereContainsExpression expression)
        {

            whereClause.AppendFormat("(doc[{0}]&&doc[{0}].indexOf({1})>=0)", MakeValue(expression.FieldName), MakeValue(expression.Value));
            this.viewNameBuilder.AppendFormat("{0}_CONTAINS_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereStartsWith(WhereStartsWithExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}]&&doc[{0}].match(/^{1}/))", field, value);
            this.viewNameBuilder.AppendFormat("{0}_STARTSWITH_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereEndsWith(WhereEndsWithExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}]&&doc[{0}].match(/{1}$/))", MakeValue(expression.FieldName), value);
            this.viewNameBuilder.AppendFormat("{0}_ENDSWITH_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereEquals(WhereEqualsExpression expression)
        {
            if (expression.FieldName.ToUpper() == "UUID")
            {
                if (expression.Value != null)
                {
                    eqUUIDs.Add(expression.Value.ToString());
                }
            }
            else if (expression.FieldName.ToUpper() == "USERKEY")
            {
                if (expression.Value != null)
                {
                    eqUserKeys.Add(expression.Value.ToString());
                }
            }
            else
            {
                var field = MakeValue(expression.FieldName);
                var value = MakeValue(expression.Value);
                whereClause.AppendFormat("(doc[{0}]=={1})", MakeValue(expression.FieldName), value);
                this.viewNameBuilder.AppendFormat("{0}_EQ_{1}", expression.FieldName, AsViewNameString(expression.Value));
            }
        }

        protected override void VisitWhereClause(WhereClauseExpression expression)
        {
            whereClause.AppendFormat("({0})", expression.WhereClause);
            this.viewNameBuilder.AppendFormat("({0})", AsViewNameString(expression.WhereClause));
        }

        protected override void VisitWhereGreaterThan(WhereGreaterThanExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}] > {1})", field, value);
            this.viewNameBuilder.AppendFormat("{0}_GT_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereGreaterThanOrEqual(WhereGreaterThanOrEqualExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}] >= {1})", field, value);
            this.viewNameBuilder.AppendFormat("{0}_GTE_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereLessThan(WhereLessThanExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}] < {1})", field, value);
            this.viewNameBuilder.AppendFormat("{0}_LT_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereLessThanOrEqual(WhereLessThanOrEqualExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}] <= {1})", field, value);
            this.viewNameBuilder.AppendFormat("{0}_LTE_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhereNotEquals(WhereNotEqualsExpression expression)
        {
            var field = MakeValue(expression.FieldName);
            var value = MakeValue(expression.Value);
            whereClause.AppendFormat("(doc[{0}] != {1})", field, value);
            this.viewNameBuilder.AppendFormat("{0}_NE_{1}", expression.FieldName, AsViewNameString(expression.Value));
        }

        protected override void VisitWhere(IWhereExpression expression)
        {
            if (!((expression is WhereInExpression) || (expression is WhereNotInExpression)))
            {
                if (whereClause.Length > 0)
                {
                    whereClause.Append(" && ");

                    this.viewNameBuilder.AppendFormat("_AND_");
                }
            }

            base.VisitWhere(expression);
        }

        private CouchbaseVisitor VisitInner(IExpression expression)
        {
            CouchbaseVisitor visitor = new CouchbaseVisitor();
            visitor.Visite(expression);

            //combine the order expressions.
            this.OrderFields.AddRange(visitor.OrderFields);

            this.CategoryQueries = this.CategoryQueries.Concat(visitor.CategoryQueries);
            return visitor;
        }

        protected override void VisitAndAlso(Content.Query.Expressions.AndAlsoExpression expression)
        {
            string leftClause = "";
            string leftViewName = "";
            if (!(expression.Left is TrueExpression))
            {
                var leftVisitor = VisitInner(expression.Left);
                leftClause = leftVisitor.WhereClause;
                leftViewName = leftVisitor.ViewName;
                if (leftVisitor.OrderClause!=null)
                {
                    this.OrderClause = leftVisitor.OrderClause;    
                }
                
                this.eqUserKeys.AddRange(leftVisitor.EQUserKeys);
            }

            string rightClause = "";
            string rightViewName = "";
            if (!(expression.Right is TrueExpression))
            {
                var rightVisitor = VisitInner(expression.Right);
                rightClause = rightVisitor.WhereClause;
                rightViewName = rightVisitor.ViewName;
                if (rightVisitor.OrderClause != null)
                {
                    this.OrderClause = rightVisitor.OrderClause;
                }
                this.eqUserKeys.AddRange(rightVisitor.EQUserKeys);
            }


            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format(" ({0} && {1})", leftClause, rightClause));
                viewNameBuilder.AppendFormat("_({0}_AND_{1})", leftViewName, rightViewName);
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                whereClause.AppendFormat(string.Format("{0}", leftClause));
                viewNameBuilder.AppendFormat("_{0}", leftViewName);
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("{0}", rightClause));
                viewNameBuilder.AppendFormat("_{0}", rightViewName);
            }
        }

        protected override void VisitOrElse(OrElseExpression expression)
        {
            string leftClause = "";
            string leftViewName = "";
            if (!(expression.Left is FalseExpression))
            {
                var leftVisitor = VisitInner(expression.Left);
                leftClause = leftVisitor.WhereClause;
                leftViewName = leftVisitor.ViewName;
                if (leftVisitor.OrderClause != null)
                {
                    this.OrderClause = leftVisitor.OrderClause;
                }
            }

            string rightClause = "";
            string rightViewName = "";
            if (!(expression.Right is FalseExpression))
            {
                var rightVisitor = VisitInner(expression.Right);
                rightClause = rightVisitor.WhereClause;
                rightViewName = rightVisitor.ViewName;
                if (rightVisitor.OrderClause != null)
                {
                    this.OrderClause = rightVisitor.OrderClause;
                }
            }

            if (!string.IsNullOrEmpty(leftClause) && !string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("({0}||{1})", leftClause, rightClause));
                viewNameBuilder.AppendFormat("_({0}_OR_{1})", leftViewName, rightViewName);
            }
            else if (!string.IsNullOrEmpty(leftClause))
            {
                whereClause.AppendFormat(string.Format("{0}", leftClause));
                viewNameBuilder.AppendFormat("_{0}", leftViewName);
            }
            else if (!string.IsNullOrEmpty(rightClause))
            {
                whereClause.AppendFormat(string.Format("{0}", rightClause));
                viewNameBuilder.AppendFormat("_{0}", rightViewName);
            }
        }

        protected override void VisitFalse(FalseExpression expression)
        {
            whereClause.Append(" (!1) ");
            viewNameBuilder.AppendFormat("!1");
        }

        protected override void VisitTrue(TrueExpression expression)
        {
            whereClause.Append(" (!0) ");
            viewNameBuilder.AppendFormat("!0");
        }
    }
}
