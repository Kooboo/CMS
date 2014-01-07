using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public class CouchbaseChildrenQuery : CouchbaseQuery
    {
        private ChildrenQuery _childrenQuery = null;
        public CouchbaseChildrenQuery(ChildrenQuery contentQuery)
            : base(contentQuery)
        {
            this._childrenQuery = contentQuery;
        }


        protected override string BuildIfClause(CouchbaseVisitor visitor, out string viewName, out string[] keys)
        {
            keys = null;


            string clause = visitor.WhereClause;
            viewName = visitor.ViewName;
            CouchbaseQueryTranslator translator = new CouchbaseQueryTranslator();
            if (this._childrenQuery.EmbeddedFolder != null)
            {
                var folderName = visitor.MakeValue(this._childrenQuery.EmbeddedFolder.FullName);
                clause += string.Format("{0}({1}=={2})",
                    string.IsNullOrEmpty(clause) ? string.Empty : "&&",
                    "doc[\\\"FolderName\\\"]",
                    folderName);

                viewName = string.Format("FolderName_EQ_{0}_", visitor.AsViewNameString(this._childrenQuery.EmbeddedFolder.FullName)) + viewName;
            }

            var parents = ((IEnumerable<TextContent>)(translator.Translate(this._childrenQuery.ParentQuery)).Execute()).ToList();
            if (parents.Count() > 0)
            {
                keys = parents.SelectMany(it => QueryChildren(this.ContentQuery.Repository, it.UUID)).ToArray();
            }
            else
            {
                keys = new string[0];
            }

            return clause;
        }
        private IEnumerable<string> QueryChildren(Repository repository, string parentUUID)
        {
            return repository.QueryChildrenBy(parentUUID).Select(row => row.Info["value"].ToString());
        }
    }
}
