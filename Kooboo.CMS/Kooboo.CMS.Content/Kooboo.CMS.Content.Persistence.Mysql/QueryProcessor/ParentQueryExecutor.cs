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

namespace Kooboo.CMS.Content.Persistence.Mysql.QueryProcessor
{
    public class ParentQueryExecutor : TextContentQueryExecutorBase
    {
        public ParentQueryExecutor(ParentQuery query)
            : base(query)
        {
            this.parentQuery = query;
        }
        private ParentQuery parentQuery;

        public override string BuildQuerySQL(MysqlVisitor<Models.TextContent> visitor, out IEnumerable<Parameter> parameters)
        {
            var innerVisitor = new MysqlVisitor<Models.TextContent>(visitor.Parameters);
            innerVisitor.Visite(parentQuery.ChildrenQuery.Expression);
            var innerExecutor = (new TextContentTranslator()).Translate(parentQuery.ChildrenQuery);
            var innerQuerySQL = innerExecutor.BuildQuerySQL(innerVisitor, out parameters);

            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }

            string whereClause = visitor.WhereClause;

            if (this.parentQuery.ParentFolder != null)
            {
                string str = visitor.AppendParameter(this.parentQuery.ParentFolder.FullName);
                whereClause = whereClause + " AND FolderName=" + str;
            }

            string sql = string.Format(@"
            SELECT {0} FROM `{1}` parent                            
               WHERE  EXISTS(
                        SELECT UUID
                            FROM ({2})children
                            WHERE parent.UUID = children.ParentUUID 
                      ) AND {3}", selectClause
                                , parentQuery.ParentSchema.GetTableName()
                                , innerQuerySQL
                                , whereClause);

            return sql;
        }
    }
}
