using Couchbase;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public abstract class CouchbaseQuery
    {
        static Dictionary<string, string> viewNamesCache = new Dictionary<string, string>();
        public IContentQuery<TextContent> ContentQuery { get; private set; }
        public CouchbaseQuery(IContentQuery<TextContent> contentQuery)
        {
            this.ContentQuery = contentQuery;
        }

        private string ToViewName(string str)
        {
            return str
                .Replace("(", "_PL_")//parenleft
                .Replace(")", "_PR_")//parenright
                .Replace("[", "_BL_")//bracketleft
                .Replace("]", "_BR_")//bracketright
                .Replace("/", "_SL_")//slash left
                .Replace("\\", "_SR_")//slash right
                .Replace("\"", "_DQ_")//double quote
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }

        public string BuildView(CouchbaseVisitor visitor, string designName, out string viewName, out string[] keys)
        {
            StringBuilder viewBuilder = new StringBuilder();

            string viewBody = BuildViewBody(visitor, out viewName, out keys);

            viewBuilder.Append("{\"views\":{");
            viewBuilder.Append("\"");
            viewBuilder.Append(viewName);
            viewBuilder.Append("\":");
            viewBuilder.Append("{");
            viewBuilder.Append("\"map\":");
            viewBuilder.Append("\"function(doc,meta){");
            viewBuilder.Append(viewBody);
            viewBuilder.Append("}\"");
            viewBuilder.Append("}");
            viewBuilder.Append("}");
            viewBuilder.Append("}");
            //viewBuilder.Append("},");
            //viewBuilder.Append("\"options\": {\"updateMinChanges\": 1000}}");
            //   viewBuilder.AppendFormat(@"{{""views"": {{{0}:{{""map"":""function(doc,meta){{{1}}}""}}}},""options"": {{""updateMinChanges"": 1}}}}", viewName,viewBody);

            return viewBuilder.ToString()
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "");
        }
        protected virtual string BuildViewBody(CouchbaseVisitor visitor, out string viewName, out string[] keys)
        {
            StringBuilder viewBody = new StringBuilder();
            var clause = BuildIfClause(visitor, out viewName, out keys);

            string keyExpression = "doc.UUID";

            if (visitor.OrderClause != null && (keys == null || keys.Length == 0))
            {
                keyExpression = "[doc." + visitor.OrderClause.FieldName + "," + keyExpression + "]";
                viewName = viewName + "_ORDERBY_" + visitor.OrderClause.FieldName;
            }

            viewBody.Append(string.IsNullOrEmpty(clause) ? string.Empty : "if(")
               .Append(clause)
               .Append(string.IsNullOrEmpty(clause) ? string.Empty : ")")
               .AppendFormat("emit({0},{{UUID:doc.UUID}});", keyExpression);

            return viewBody.ToString();
        }
        protected abstract string BuildIfClause(CouchbaseVisitor visitor, out string viewName, out string[] keys);

        public virtual object Execute()
        {
            var visitor = new CouchbaseVisitor();
            visitor.Visite(ContentQuery.Expression);


            if (string.IsNullOrEmpty(visitor.ViewName) && visitor.EQUUIDs.Count() > 0)
            {
                return QueryByUUID(visitor);
            }
            else if (string.IsNullOrEmpty(visitor.ViewName) && visitor.EQUserKeys.Count() > 0)
            {
                return QueryByUserKey(visitor);
            }
            else
            {
                return QueryByView(visitor);
            }
        }

        private object QueryByUUID(CouchbaseVisitor visitor)
        {
            object result = null;
            var getResult = this.ContentQuery.Repository.GetClient().ExecuteGet(visitor.EQUUIDs);

            switch (visitor.CallType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    result = getResult.Count;
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    result = getResult.Values.First().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    result = getResult.Values.Last().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    result = getResult.Count == 0 ? null : getResult.Values.Last().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    result = getResult.Count == 0 ? null : getResult.Values.First().ToContent();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:
                    result = getResult.Values.Select(it => it.ToContent());
                    break;
            }

            return result;
        }

        private object QueryByUserKey(CouchbaseVisitor visitor)
        {
            var view = this.ContentQuery.Repository.GetClient().GetView(this.ContentQuery.Repository.GetDefaultViewDesign(), "Sort_By_UserKey").Reduce(false).Stale(StaleMode.False);
            view = view.Keys(visitor.EQUserKeys);

            return ExecuteView(visitor, view);
        }

        private object QueryByView(CouchbaseVisitor visitor)
        {
            //create view
            var designName = string.Empty;//"__TempViews__";
            var viewName = string.Empty;

            string[] keys;
            string viewDocument = this.BuildView(visitor, designName, out viewName, out keys);
            designName = viewName;

            if (keys != null && keys.Length == 0)
            {
                return DefaultValueExecute(visitor.CallType);
            }

            IView<IViewRow> couchbaseCursor = null;
            couchbaseCursor = this.ContentQuery.Repository.GetClient().GetView(designName, viewName);
            var cacheKey = string.Format("{0}---{1}/{2}", this.ContentQuery.Repository.Name, designName, viewName);
            var viewIsExists = viewNamesCache.ContainsKey(cacheKey);

            if (!viewIsExists)
            {
                viewIsExists = couchbaseCursor.CheckExists();
                if (!viewIsExists)
                {
                    viewIsExists = DatabaseHelper.CreateDesignDocument(this.ContentQuery.Repository.GetBucketName(), designName, viewDocument);
                    if (viewIsExists)
                    {
                        viewNamesCache[cacheKey] = cacheKey;
                    }
                }
                else
                {
                    viewNamesCache[cacheKey] = cacheKey;
                }
            }
            couchbaseCursor = couchbaseCursor.Reduce(false).Stale(StaleMode.False);

            if (keys != null && keys.Length > 0)
            {
                couchbaseCursor.Keys(keys);
            }

            if (viewIsExists)
            {
                return ExecuteView(visitor, couchbaseCursor);
            }
            else
            {
                throw new Exception("The view creation failed.");
            }


        }
        private object ExecuteView(CouchbaseVisitor visitor, IView<IViewRow> couchbaseCursor)
        {

            object result = null;
            if (couchbaseCursor == null)
            {
                return DefaultValueExecute(visitor.CallType);
            }
            if (visitor.OrderClause != null)
            {
                couchbaseCursor.Descending(visitor.OrderClause.Descending);
            }
            if (visitor.Skip != 0)
            {
                couchbaseCursor = couchbaseCursor.Skip(visitor.Skip);
            }
            if (visitor.Take != 0)
            {
                couchbaseCursor = couchbaseCursor.Limit(visitor.Take);
            }
            switch (visitor.CallType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    result = couchbaseCursor.Count();
                    break;
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    return ViewRowToTextContent(couchbaseCursor.First());
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    return ViewRowToTextContent(couchbaseCursor.Last());
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    return ViewRowToTextContent(couchbaseCursor.LastOrDefault());
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    return ViewRowToTextContent(couchbaseCursor.FirstOrDefault());
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:

                    var uuidList = couchbaseCursor.Select(it => GetUUID(it)).ToArray();
                    return this.ContentQuery.Repository.GetClient().ExecuteGet(uuidList).Select(it => it.Value.ToContent());
            }
            return result;
        }
        private TextContent ViewRowToTextContent(IViewRow viewRow)
        {
            if (viewRow == null)
            {
                return null;
            }
            var uuid = GetUUID(viewRow);
            return this.ContentQuery.Repository.GetClient().ExecuteGet(uuid).ToContent();
        }

        private static string GetUUID(IViewRow viewRow)
        {
            var uuid = viewRow.DictionaryValue()["UUID"];
            return uuid.ToString();
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
                    return new TextContent[0];
            }
        }
    }
}
