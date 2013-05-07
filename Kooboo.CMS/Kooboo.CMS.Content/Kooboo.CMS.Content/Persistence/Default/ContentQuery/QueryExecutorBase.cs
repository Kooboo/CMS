#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default.ContentQuery
{
    internal abstract class QueryExecutorBase
    {
        public abstract object Execute();

        protected virtual object Execute(IQueryable<TextContent> queryable, IEnumerable<OrderExpression> orders, CallType callType, int? skip, int? take)
        {
            #region Order query
            IOrderedQueryable<TextContent> ordered = null;
            foreach (var orderItem in orders)
            {
                if (!orderItem.Descending)
                {
                    if (ordered == null)
                    {
                        ordered = queryable.OrderBy(orderItem.OrderExprssion);
                    }
                    else
                    {
                        ordered = ordered.ThenBy(orderItem.OrderExprssion);
                    }

                }
                else
                {
                    if (ordered == null)
                    {
                        ordered = queryable.OrderByDescending(orderItem.OrderExprssion);
                    }
                    else
                    {
                        ordered = ordered.ThenByDescending(orderItem.OrderExprssion);
                    }
                }
            }
            if (ordered != null)
            {
                queryable = ordered;
            }

            #endregion


            if (skip.HasValue)
            {
                queryable = queryable.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }
            switch (callType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    return queryable.Count();
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    return queryable.First();
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    return queryable.Last();
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    return queryable.LastOrDefault();
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    return queryable.FirstOrDefault();
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:
                    return queryable.ToArray();
            }
        }
    }
}
