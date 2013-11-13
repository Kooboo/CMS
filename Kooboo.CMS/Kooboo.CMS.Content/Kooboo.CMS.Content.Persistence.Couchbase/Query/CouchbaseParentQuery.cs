using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public class CouchbaseParentQuery : CouchbaseQuery
    {
        private ParentQuery _parentQuery = null;
        public CouchbaseParentQuery(ParentQuery contentQuery)
            : base(contentQuery)
        {
            this._parentQuery = contentQuery;
        }

        protected override string BuildIfClause(CouchbaseVisitor visitor, out string viewName, out string[] keys)
        {
            keys = null;

            string clause = visitor.WhereClause;
            viewName = visitor.ViewName;

            CouchbaseQueryTranslator translator = new CouchbaseQueryTranslator();

            if (this._parentQuery.ParentFolder != null)
            {
                var folderName = visitor.MakeValue(this._parentQuery.ParentFolder.FullName);
                clause += string.Format("{0}({1}=={2})",
                    string.IsNullOrEmpty(clause) ? string.Empty : "&&",
                    "doc[\\\"FolderName\\\"]",
                    folderName);

                viewName = string.Format("FolderName_EQ_{0}_", visitor.AsViewNameString(this._parentQuery.ParentFolder.FullName)) + viewName;
            }

            var children = ((IEnumerable<TextContent>)(translator.Translate(this._parentQuery.ChildrenQuery)).Execute()).ToList();
            if (children.Count() > 0)
            {
                keys = children.Select(it => it.ParentUUID).ToArray();
            }
            else
            {
                keys = new string[0];
            }

            return clause;
        }
    }
}
