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
            IEnumerable<Category> categoryContents = null;
            if (visitor.CategoryQueries != null && visitor.CategoryQueries.Count() > 0)
            {
                CouchbaseQueryTranslator translator = new CouchbaseQueryTranslator();
                //TODO
                foreach (var item in visitor.CategoryQueries)
                {
                    var categories = ((IEnumerable<TextContent>)translator.Translate(item).Execute()).ToArray();
                    if (categories.Length > 0)
                    {
                        var relation = this._textContentQuery.Repository.GetCategories().Select(it => it.ToCategory())
                            .Where(it => it.CategoryFolder.Equals(((TextContentQueryBase)item).Folder.FullName))
                            .Where(it => categories.Select(cg => cg.UUID).Contains(it.CategoryUUID));
                        if (relation != null)
                        {
                            categoryContents = relation;
                        }
                    }
                }

                keys = categoryContents.Select(it => it.ContentUUID).ToArray();
            }         

            return clause;
        }
    }
}
