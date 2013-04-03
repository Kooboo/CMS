using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.SqlServer.QueryProcessor;
using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.Content.Persistence.SqlServer
{
    public class MediaContentProvider : IMediaContentProvider
    {
        #region IContentProvider<MediaContent> Members

        public void Add(Models.MediaContent content)
        {
            ((IPersistable)content).OnSaving();
            string sql = string.Format("INSERT INTO {0}(UUID,FolderName,FileName,VirtualPath,UserId) VALUES(@UUID,@FolderName,@FileName,@VirtualPath,@UserId)"
                , content.GetRepository().GetMediaContentTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("UUID", content.UUID));
            command.Parameters.Add(new SqlParameter("FolderName", content.FolderName));
            command.Parameters.Add(new SqlParameter("FileName", content.FileName));
            command.Parameters.Add(new SqlParameter("VirtualPath", content.VirtualPath));
            command.Parameters.Add(new SqlParameter("@UserId", content.UserId));
            SQLServerHelper.BatchExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
            ((IPersistable)content).OnSaved();
        }

        public void Update(Models.MediaContent @new, Models.MediaContent old)
        {
            ((IPersistable)@new).OnSaving();
            string sql = string.Format("UPDATE {0} SET FolderName = @FolderName,FileName = @FileName,VirtualPath=@VirtualPath) WHERE UUID=@UUID"
                , @new.GetRepository().GetMediaContentTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("UUID", @new.UUID));
            command.Parameters.Add(new SqlParameter("FolderName", @new.FolderName));
            command.Parameters.Add(new SqlParameter("FileName", @new.FileName));
            command.Parameters.Add(new SqlParameter("VirtualPath", @new.VirtualPath));
            SQLServerHelper.BatchExecuteNonQuery(@new.GetRepository().GetConnectionString(), command);
            ((IPersistable)@new).OnSaved();
        }

        public void Delete(Models.MediaContent content)
        {
            string sql = string.Format("DELETE FROM {0} WHERE UUID=@UUID", content.GetRepository().GetMediaContentTableName());
            SqlCommand command = new SqlCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("UUID", content.UUID));
            SQLServerHelper.BatchExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
        }

        public object Execute(Query.IContentQuery<Models.MediaContent> query)
        {
            MediaContentQueryExecutor executor = new MediaContentQueryExecutor((MediaContentQuery)query);
            return executor.Execute();
        }

        #endregion
    }
}
