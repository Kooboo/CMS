using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor
{
    public class CategorizablesQueryExecutor : TextContentQueryExecutorBase
    {
        public CategorizablesQueryExecutor(CategorizablesQuery query)
            : base(query)
        {
            this.categorizablesQuery = query;
        }
        private CategorizablesQuery categorizablesQuery;
        public override string BuildQuerySQL(SQLCeVisitor<Models.TextContent> visitor, out IEnumerable<Parameter> parameters)
        {
            var innerVisitor = new SQLCeVisitor<Models.TextContent>(visitor.Parameters);
            innerVisitor.Visite(categorizablesQuery.CategoryQuery.Expression);
            var innerExecutor = (new TextContentTranslator()).Translate(categorizablesQuery.CategoryQuery);
            var innerQuerySQL = innerExecutor.BuildQuerySQL(innerVisitor, out parameters);

            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }

            string whereClause = visitor.WhereClause;

            var paraName = visitor.AppendParameter(categorizablesQuery.CategorizableFolder.FullName);
            whereClause = whereClause + " AND FolderName=" + paraName;

            string sql = string.Format(@"
            SELECT {0} FROM [{1}] content
               WHERE  EXISTS(
                        SELECT ContentCategory.CategoryUUID 
                            FROM [{2}] ContentCategory,
                                ({3})category
                            WHERE content.UUID = ContentCategory.UUID AND ContentCategory.CategoryUUID = category.UUID 
                      ) AND {4}", selectClause
                                , categorizablesQuery.CategorizableFolder.GetSchema().GetTableName()
                                , categorizablesQuery.Repository.GetCategoryTableName()
                                , innerQuerySQL
                                , whereClause);

            return sql;
        }
    }
}
