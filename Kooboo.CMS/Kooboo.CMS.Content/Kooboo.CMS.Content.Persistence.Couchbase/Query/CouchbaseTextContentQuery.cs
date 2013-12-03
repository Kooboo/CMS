using Couchbase;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public class CouchbaseTextContentQuery : CouchbaseQuery
    {
        private TextContentQuery _textContentQuery = null;
        public CouchbaseTextContentQuery(TextContentQuery contentQuery)
            : base(contentQuery)
        {
            this._textContentQuery = contentQuery;
        }

        protected override string BuildIfClause(CouchbaseVisitor visitor, out string viewName, out string[] keys)
        {

            string clause = visitor.WhereClause;
            viewName = visitor.ViewName;

            if (this._textContentQuery.Folder != null)
            {
                var folderName = visitor.MakeValue(this._textContentQuery.Folder.FullName);
                clause += string.Format("{0}({1}=={2})",
                    string.IsNullOrEmpty(clause) ? string.Empty : "&&",
                    "doc[\\\"FolderName\\\"]",
                    folderName);
                viewName = string.Format("FolderName_EQ_{0}_", visitor.AsViewNameString(this._textContentQuery.Folder.FullName)) + viewName;
            }
            else if (this._textContentQuery.Schema != null)
            {
                clause += string.Format("{0}({1}=={2})",
                    string.IsNullOrEmpty(clause) ? string.Empty : "&&",
                    "doc[\\\"SchemaName\\\"]",
                    visitor.MakeValue(this._textContentQuery.Schema.Name));
                viewName = string.Format("SchemaName_EQ_{0}_", visitor.AsViewNameString(this._textContentQuery.Schema.Name)) + viewName;
            }

            keys = null;
            IEnumerable<string> categoryContents = new string[0];
            if (visitor.CategoryQueries != null && visitor.CategoryQueries.Count() > 0)
            {
                CouchbaseQueryTranslator translator = new CouchbaseQueryTranslator();
                //TODO
                foreach (var item in visitor.CategoryQueries)
                {
                    var categories = ((IEnumerable<TextContent>)translator.Translate(item).Execute()).ToArray();
                    if (categories.Length > 0)
                    {
                        var categoryUUIDs = categories.SelectMany(it => QueryByCategory(this._textContentQuery.Repository, it.UUID)).ToArray();
                        categoryContents = categoryContents.Concat(categoryUUIDs);
                    }
                }

                keys = categoryContents.Distinct().ToArray();
            }

            return clause;
        }
        private IEnumerable<string> QueryByCategory(Repository repository, string categoryUUID)
        {
            return repository.QueryByCategory(categoryUUID).Select(row => (IDictionary<string, object>)row.Info["value"]).Select(it => it["ContentUUID"].ToString());
        }
    }
}
