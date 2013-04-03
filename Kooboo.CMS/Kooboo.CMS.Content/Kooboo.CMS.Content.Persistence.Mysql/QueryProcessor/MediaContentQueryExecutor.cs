using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Persistence.SqlServer.QueryProcessor
{
    public class MediaContentQueryExecutor : ContentQueryExecutorBase<MediaContent>
    {
        public MediaContentQueryExecutor(MediaContentQuery mediaContentQuery)
            : base(mediaContentQuery)
        {
            this.mediaContentQuery = mediaContentQuery;
        }
        private MediaContentQuery mediaContentQuery;
        public override string BuildQuerySQL(SQLServerVisitor<MediaContent> visitor, out IEnumerable<Parameter> parameters)
        {
            string selectClause = "*";
            if (visitor.SelectFields != null && visitor.SelectFields.Length > 0)
            {
                selectClause = string.Join(",", visitor.SelectFields);
            }


            string whereClause = visitor.WhereClause;

            var paraName = visitor.AppendParameter(mediaContentQuery.MediaFolder.FullName);
            whereClause = whereClause + "AND FolderName=" + paraName;


            parameters = visitor.Parameters;
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2} ", selectClause, mediaContentQuery.Repository.GetMediaContentTableName(), whereClause);

            return sql;
        }
    }
}
