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
    public static class IContentQueryExtensions
    {
        public static Lazy<int> LazyCount<T>(this IContentQuery<T> contentQuery)
           where T : ContentBase
        {
            contentQuery = contentQuery.Create(new CallExpression(contentQuery.Expression, CallType.Count));
            return new Lazy<int>(() => (int)contentQuery.Count());
        }

        public static Lazy<T> LazyFirst<T>(this IContentQuery<T> contentQuery)
            where T : ContentBase
        {
            contentQuery = contentQuery.Create(new CallExpression(contentQuery.Expression, CallType.First));
            return new Lazy<T>(() => contentQuery.First());
        }

        public static Lazy<T> LazyFirstOrDefault<T>(this IContentQuery<T> contentQuery)
            where T : ContentBase
        {
            contentQuery = contentQuery.Create(new CallExpression(contentQuery.Expression, CallType.FirstOrDefault));
            return new Lazy<T>(() => contentQuery.FirstOrDefault());
        }

        public static Lazy<T> LazyLast<T>(this IContentQuery<T> contentQuery)
            where T : ContentBase
        {
            contentQuery = contentQuery.Create(new CallExpression(contentQuery.Expression, CallType.Last));
            return new Lazy<T>(() => contentQuery.Last());
        }

        public static Lazy<T> LazyLastOrDefault<T>(this IContentQuery<T> contentQuery)
            where T : ContentBase
        {
            contentQuery = contentQuery.Create(new CallExpression(contentQuery.Expression, CallType.LastOrDefault));
            return new Lazy<T>(() => contentQuery.LastOrDefault());
        }
    }
}
