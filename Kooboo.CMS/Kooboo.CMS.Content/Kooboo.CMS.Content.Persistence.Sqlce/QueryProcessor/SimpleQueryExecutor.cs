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
using System.Data.SqlServerCe;
using System.Data;
namespace Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor
{
    public class SimpleQueryExecutor : TextContentQueryExecutorBase
    {
        public SimpleQueryExecutor(TextContentQuery textContentQuery)
            : base(textContentQuery)
        {
            this.textContentQuery = textContentQuery;
        }
        private TextContentQuery textContentQuery;

        public override string BuildQuerySQL(SQLCeVisitor<TextContent> visitor, out IEnumerable<Parameter> parameters)
        {
            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }


            string whereClause = visitor.WhereClause;
            if (textContentQuery.Folder != null)
            {
                var paraName = visitor.AppendParameter(textContentQuery.Folder.FullName);
                whereClause = whereClause + " AND FolderName=" + paraName;
            }

            var categoryClauses = string.Join(" AND ", GetCategoryClause(textContentQuery.Repository, visitor.CategoryQueries, visitor.Parameters).ToArray());

            if (!string.IsNullOrEmpty(categoryClauses))
            {
                whereClause = whereClause + " AND " + categoryClauses;
            }


            parameters = visitor.Parameters;

            string sql = string.Format("SELECT {0} FROM [{1}] content WHERE {2} ", selectClause, textContentQuery.Schema.GetTableName(), whereClause);


            return sql;
        }
        private IEnumerable<string> GetCategoryClause(Repository repository, IEnumerable<IContentQuery<TextContent>> categoryQueries, List<Parameter> parameters)
        {
            TextContentTranslator translator = new TextContentTranslator();
            List<string> categoryQueryList = new List<string>();
            foreach (var categoryQuery in categoryQueries)
            {
                var executor = translator.Translate(categoryQuery);
                var categoryVisitor = new SQLCeVisitor<TextContent>(parameters);
                categoryVisitor.Visite(categoryQuery.Expression);

                IEnumerable<Parameter> outParameters;
                var categoryQuerySQL = executor.BuildQuerySQL(categoryVisitor, out outParameters);

                categoryQueryList.Add(string.Format(@"EXISTS(
                        SELECT ContentCategory.CategoryUUID 
                            FROM [{0}] ContentCategory,
                                ({1})category
                            WHERE content.UUID = ContentCategory.UUID AND ContentCategory.CategoryUUID = category.UUID 
                      )", repository.GetCategoryTableName()
                        , categoryQuerySQL));

            }
            return categoryQueryList;
        }
    }
}
