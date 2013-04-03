using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Query
{
    public class ContentQuery<T> : IContentQuery<T>
        where T : ContentBase
    {
        public ContentQuery(Repository repository)
            : this(repository, null)
        {

        }
        public ContentQuery(Repository repository, IExpression expression)
        {
            this.Repository = repository;
            this.Expression = expression;
        }

        public Repository Repository { get; private set; }

        public virtual IContentProvider<T> ContentProvider
        {
            get
            {
                return Persistence.Providers.DefaultProviderFactory.GetProvider<IContentProvider<T>>();
            }
        }

        #region IContentQuery<T> Members

        public IExpression Expression
        {
            get;
            private set;
        }

        public IContentQuery<T> Select(params string[] fields)
        {
            var expression = new SelectExpression(this.Expression, fields);
            return this.Create(expression);
        }

        public IContentQuery<T> Skip(int count)
        {
            var expression = new SkipExpression(this.Expression, count);
            return this.Create(expression);
        }
        public IContentQuery<T> Take(int count)
        {
            var expression = new TakeExpression(this.Expression, count);
            return this.Create(expression);
        }
        public IContentQuery<T> OrderBy(string fieldName)
        {
            var expression = new OrderExpression(this.Expression, fieldName, false);
            return this.Create(expression);
        }

        public IContentQuery<T> OrderByDescending(string fieldName)
        {
            var expression = new OrderExpression(this.Expression, fieldName, true);
            return this.Create(expression);
        }
        public IContentQuery<T> OrderBy(OrderExpression expression)
        {
            return this.Create(expression);
        }
        public IContentQuery<T> Or(IWhereExpression expression)
        {
            IExpression exp = null;
            if (this.Expression is IWhereExpression)
            {
                exp = new OrElseExpression((IWhereExpression)this.Expression, expression);
            }
            else
            {
                exp = new OrElseExpression(new FalseExpression(), expression);
            }
            return this.Create(exp);
        }
        public IContentQuery<T> Where(IWhereExpression expression)
        {
            IExpression exp = null;
            if (this.Expression is IWhereExpression)
            {
                exp = new AndAlsoExpression((IWhereExpression)Expression, expression);
            }
            else
            {
                exp = new AndAlsoExpression(new TrueExpression(), expression);
            }

            return this.Create(exp);
        }
        public IContentQuery<T> Where(string whereClause)
        {
            var expression = new WhereClauseExpression(this.Expression, whereClause);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereBetween(string fieldName, object start, object end)
        {
            var expression = new WhereBetweenExpression(this.Expression, fieldName, start, end);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereBetweenOrEqual(string fieldName, object start, object end)
        {
            var expression = new WhereBetweenOrEqualExpression(this.Expression, fieldName, start, end);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereContains(string fieldName, object value)
        {
            var expression = new WhereContainsExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereEndsWith(string fieldName, object value)
        {
            var expression = new WhereEndsWithExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereEquals(string fieldName, object value)
        {
            var expression = new WhereEqualsExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }
        public IContentQuery<T> WhereNotEquals(string fieldName, object value)
        {
            var expression = new WhereNotEqualsExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereGreaterThan(string fieldName, object value)
        {
            var expression = new WhereGreaterThanExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereGreaterThanOrEqual(string fieldName, object value)
        {
            var expression = new WhereGreaterThanOrEqualExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereLessThan(string fieldName, object value)
        {
            var expression = new WhereLessThanExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereLessThanOrEqual(string fieldName, object value)
        {
            var expression = new WhereLessThanOrEqualExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }

        public IContentQuery<T> WhereStartsWith(string fieldName, object value)
        {
            var expression = new WhereStartsWithExpression(this.Expression, fieldName, value);
            return this.Create(expression);
        }
        public IContentQuery<T> WhereIn(string fieldName, params object[] values)
        {
            //IWhereExpression exp = new FalseExpression();
            //foreach (var value in values)
            //{
            //    exp = new OrElseExpression(exp, new WhereEqualsExpression(null, fieldName, value));
            //}
            //return this.Where(exp);
            var expression = new WhereInExpression(this.Expression, fieldName, values);
            return this.Create(expression);
        }
        public IContentQuery<T> WhereNotIn(string fieldName, params object[] values)
        {
            var expression = new WhereNotInExpression(this.Expression, fieldName, values);
            return this.Create(expression);
        }
        public int Count()
        {
            var contentQuery = this.Create(new CallExpression(this.Expression, CallType.Count));
            return (int)ContentProvider.Execute(contentQuery);
        }

        public T First()
        {
            var contentQuery = this.Create(new CallExpression(this.Expression, CallType.First));
            return (T)ContentProvider.Execute(contentQuery);
        }

        public T FirstOrDefault()
        {
            var contentQuery = this.Create(new CallExpression(this.Expression, CallType.FirstOrDefault));
            return (T)ContentProvider.Execute(contentQuery);
        }

        public T Last()
        {
            var contentQuery = this.Create(new CallExpression(this.Expression, CallType.Last));
            return (T)ContentProvider.Execute(contentQuery);
        }

        public T LastOrDefault()
        {
            var contentQuery = this.Create(new CallExpression(this.Expression, CallType.LastOrDefault));
            return (T)ContentProvider.Execute(contentQuery);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)ContentProvider.Execute(this)).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IContentQuery<T> Members


        public virtual IContentQuery<T> Create(IExpression expression)
        {
            return new ContentQuery<T>(this.Repository, expression);
        }

        #endregion



    }
}
