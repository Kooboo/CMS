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
using MongoDB.Bson;
using MongoDB.Driver;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query.Expressions;
using QueryBuilder = MongoDB.Driver.Builders;
using MongoDB.Driver.Builders;
using Kooboo.Common;
namespace Kooboo.CMS.Content.Persistence.MongoDB.Query
{
    public abstract class MongoDBQuery
    {
        public MongoDBQuery(IContentQuery<TextContent> contentQuery)
        {
            this.ContentQuery = contentQuery;
        }

        public IContentQuery<TextContent> ContentQuery { get; private set; }

        protected abstract MongoCursor<BsonDocument> Query(MongoDBVisitor visitor);

        public virtual object Execute()
        {
            var visitor = new MongoDBVisitor();
            visitor.Visite(ContentQuery.Expression);
            var mongoCursor = Query(visitor);
            if (mongoCursor == null)
            {
                return DefaultValueExecute(visitor.CallType);
            }
            if (visitor.Skip != 0)
            {
                mongoCursor.Skip = visitor.Skip;
            }

            SortByBuilder sortBuilder = new SortByBuilder();
            
            foreach (var item in visitor.OrderFields)
            {
                if (item.Descending)
                {
                    sortBuilder.Descending(item.FieldName);
                }
                else
                {
                    sortBuilder.Ascending(item.FieldName);
                }
            }
            mongoCursor = mongoCursor.SetSortOrder(sortBuilder);

            object result = null;
            switch (visitor.CallType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    result = Convert.ToInt32(mongoCursor.Count());
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    result = mongoCursor.First().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    result = mongoCursor.Last().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    result = mongoCursor.Last().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    result = mongoCursor.FirstOrDefault().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:
                    if (visitor.Take != 0)
                    {
                        result = mongoCursor.Take(visitor.Take).Select(it => it.ToContent());
                    }
                    else
                        result = mongoCursor.Select(it => it.ToContent());
                    break;
            }

            //if (mongoCursor.Database.Server.State != MongoServerState.Disconnected)
            //{
            //    mongoCursor.Database.Server.Disconnect();
            //}
            return result;
        }

        protected virtual object DefaultValueExecute(CallType callType)
        {
            switch (callType)
            {
                case CallType.Count:
                    return 0;
                case CallType.First:
                    throw new InvalidOperationException(SR.GetString("NoElements"));
                case CallType.Last:
                    throw new InvalidOperationException(SR.GetString("NoElements"));
                case CallType.LastOrDefault:
                    return null;
                case CallType.FirstOrDefault:
                    return null;
                case CallType.Unspecified:
                default:
                    return new object[0];
            }
        }
    }
}
