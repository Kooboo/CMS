using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Couchbase.Query
{
    public class CouchbaseCategoryQuery : CouchbaseQuery
    {/*
        public CouchbaseCategoryQuery(IContentQuery<TextContent> contentQuery)
            : base(contentQuery)
        {

        }*/
        public CouchbaseCategoryQuery(CategoriesQuery contentQuery)
            : base(contentQuery)
        {
            this._categoriesQuery = contentQuery;
        }
        private CategoriesQuery _categoriesQuery = null;

        protected override string BuildIfClause(CouchbaseVisitor visitor, out string viewName, out string[] keys)
        {

            CouchbaseQueryTranslator translator = new CouchbaseQueryTranslator();
            string clause = visitor.WhereClause;
            viewName = visitor.ViewName;

            if (this._categoriesQuery.CategoryFolder != null)
            {
                var folderName = visitor.MakeValue(this._categoriesQuery.CategoryFolder.FullName);
                clause += string.Format("{0}({1}=={2})",
                    string.IsNullOrEmpty(clause) ? string.Empty : "&&",
                    "doc[\\\"FolderName\\\"]",
                    folderName);

                viewName = string.Format("FolderName_EQ_{0}_", visitor.AsViewNameString(this._categoriesQuery.CategoryFolder.FullName)) + viewName;
            }
            keys = null;
            var subQuery = translator.Translate(this._categoriesQuery.InnerQuery);
            var contents = ((IEnumerable<TextContent>)subQuery.Execute()).ToList();
            if (contents.Count() > 0)
            {
                var uuids = contents.Select(it => it.UUID);
                keys = uuids.SelectMany(it => QueryCategories(this._categoriesQuery.Repository, it)).ToArray();
            }
            else
            {
                keys = new string[0];
            }

            return clause;
        }
        private IEnumerable<string> QueryCategories(Repository repository, string contentUUID)
        {
            return repository.QueryCategoriesBy(contentUUID).Select(row => (IDictionary<string, object>)row.Info["value"]).Select(it => it["CategoryUUID"].ToString());
        }
    }
}
