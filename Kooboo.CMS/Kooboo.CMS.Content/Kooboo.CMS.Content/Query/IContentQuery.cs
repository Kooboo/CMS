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
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Content.Query
{
    public interface IContentQuery<out T> : IEnumerable<T>
        where T : ContentBase
    {
        Repository Repository { get; }

        IExpression Expression { get; }

        IContentQuery<T> Select(params string[] fields);

        IContentQuery<T> Skip(int count);

        IContentQuery<T> OrderBy(string field);
        IContentQuery<T> OrderByDescending(string field);
        IContentQuery<T> OrderBy(OrderExpression expression);

        IContentQuery<T> Or(IWhereExpression expression);
        IContentQuery<T> Where(IWhereExpression expression);
        IContentQuery<T> Where(string whereClause);
        IContentQuery<T> WhereBetween(string fieldName, object start, object end);
        IContentQuery<T> WhereBetweenOrEqual(string fieldName, object start, object end);
        IContentQuery<T> WhereContains(string fieldName, object value);
        IContentQuery<T> WhereEndsWith(string fieldName, object value);
        IContentQuery<T> WhereEquals(string fieldName, object value);
        IContentQuery<T> WhereNotEquals(string fieldName, object value);
        IContentQuery<T> WhereGreaterThan(string fieldName, object value);
        IContentQuery<T> WhereGreaterThanOrEqual(string fieldName, object value);
        IContentQuery<T> WhereLessThan(string fieldName, object value);
        IContentQuery<T> WhereLessThanOrEqual(string fieldName, object value);
        IContentQuery<T> WhereStartsWith(string fieldName, object value);
        IContentQuery<T> WhereIn(string fieldName, params object[] values);
        IContentQuery<T> WhereNotIn(string fieldName, params object[] values);
        IContentQuery<T> Take(int count);
        IContentQuery<T> WhereIsNullOrEmpty(string fieldName);

        int Count();
        T First();
        T FirstOrDefault();
        T Last();
        T LastOrDefault();

        IContentQuery<T> Create(IExpression expression);
    }
}
