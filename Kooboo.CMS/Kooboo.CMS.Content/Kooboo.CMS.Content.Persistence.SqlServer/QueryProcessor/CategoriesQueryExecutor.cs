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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using System.Data.SqlClient;
namespace Kooboo.CMS.Content.Persistence.SqlServer.QueryProcessor
{
    public class CategoriesQueryExecutor : TextContentQueryExecutorBase
    {
        public CategoriesQueryExecutor(CategoriesQuery query)
            : base(query)
        {
            this.categoriesQuery = query;
        }

        private CategoriesQuery categoriesQuery;

        public override string BuildQuerySQL(SQLServerVisitor<TextContent> visitor, out IEnumerable<Parameter> parameters)
        {
            var innerVisitor = new SQLServerVisitor<Models.TextContent>(visitor.Parameters);
            innerVisitor.Visite(categoriesQuery.InnerQuery.Expression);
            var innerExecutor = (new TextContentTranslator()).Translate(categoriesQuery.InnerQuery);
            var innerQuerySQL = innerExecutor.BuildQuerySQL(innerVisitor, out parameters);

            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }

            string whereClause = visitor.WhereClause;

            var paraName = visitor.AppendParameter(categoriesQuery.CategoryFolder.FullName);
            whereClause = whereClause + " AND FolderName=" + paraName;


            string sql = string.Format(@"
            SELECT {0} FROM [{1}] category
               WHERE  EXISTS(
                        SELECT ContentCategory.CategoryUUID 
                            FROM [{2}] ContentCategory,
                                ({3})content
                            WHERE content.UUID = ContentCategory.UUID AND ContentCategory.CategoryUUID = category.UUID 
                      ) AND {4}", selectClause
                                , categoriesQuery.CategoryFolder.GetSchema().GetTableName()
                                , categoriesQuery.Repository.GetCategoryTableName()
                                , innerQuerySQL
                                , whereClause);

            return sql;
        }
    }
}
