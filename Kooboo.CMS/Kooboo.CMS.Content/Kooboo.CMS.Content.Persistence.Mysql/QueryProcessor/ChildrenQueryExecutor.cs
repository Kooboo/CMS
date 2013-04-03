using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Persistence.Mysql.QueryProcessor
{
    public class ChildrenQueryExecutor : TextContentQueryExecutorBase
    {
        public ChildrenQueryExecutor(ChildrenQuery query)
            : base(query)
        {
            this.childrenQuery = query;
        }

        private ChildrenQuery childrenQuery;

        public override string BuildQuerySQL(MysqlVisitor<Models.TextContent> visitor, out IEnumerable<Parameter> parameters)
        {
            var innerVisitor = new MysqlVisitor<Models.TextContent>(visitor.Parameters);
            innerVisitor.Visite(childrenQuery.ParentQuery.Expression);
            var innerExecutor = (new TextContentTranslator()).Translate(childrenQuery.ParentQuery);
            var innerQuerySQL = innerExecutor.BuildQuerySQL(innerVisitor, out parameters);

            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }

            string whereClause = visitor.WhereClause;

            if (this.childrenQuery.EmbeddedFolder != null)
            {
                string str = visitor.AppendParameter(this.childrenQuery.EmbeddedFolder.FullName);
                whereClause = whereClause + " AND FolderName=" + str;
            }

            string sql = string.Format(@"
            SELECT {0} FROM `{1}` children                            
               WHERE  EXISTS(
                        SELECT UUID
                            FROM ({2})parent
                            WHERE parent.UUID = children.ParentUUID 
                      ) AND {3}", selectClause
                                , childrenQuery.ChildSchema.GetTableName()
                                , innerQuerySQL
                                , whereClause);

            return sql;
        }
    }
}
