using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlServerCe;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor;
using Kooboo.CMS.Content.Query;
namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public class MediaContentProvider : IMediaContentProvider
    {
        #region IContentProvider<MediaContent> Members

        public void Add(Models.MediaContent content)
        {
            ((IPersistable)content).OnSaving();
            string sql = string.Format("INSERT INTO {0}(UUID,FolderName,FileName,VirtualPath,UserId) VALUES(@UUID,@FolderName,@FileName,@VirtualPath,@UserId)"
                , content.GetRepository().GetMediaContentTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("UUID", content.UUID));
            command.Parameters.Add(new SqlCeParameter("FolderName", content.FolderName));
            command.Parameters.Add(new SqlCeParameter("FileName", content.FileName));
            command.Parameters.Add(new SqlCeParameter("VirtualPath", content.VirtualPath));
            command.Parameters.Add(new SqlCeParameter("UserId", content.UserId));
            SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
            ((IPersistable)content).OnSaved();
        }

        public void Update(Models.MediaContent @new, Models.MediaContent old)
        {
            ((IPersistable)@new).OnSaving();
            string sql = string.Format("UPDATE {0} SET FolderName = @FolderName,FileName = @FileName,VirtualPath=@VirtualPath) WHERE UUID=@UUID"
                , @new.GetRepository().GetMediaContentTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("UUID", @new.UUID));
            command.Parameters.Add(new SqlCeParameter("FolderName", @new.FolderName));
            command.Parameters.Add(new SqlCeParameter("FileName", @new.FileName));
            command.Parameters.Add(new SqlCeParameter("VirtualPath", @new.VirtualPath));
            SQLCeHelper.ExecuteNonQuery(@new.GetRepository().GetConnectionString(), command);
            ((IPersistable)@new).OnSaved();
        }

        public void Delete(Models.MediaContent content)
        {
            string sql = string.Format("DELETE FROM {0} WHERE UUID=@UUID", content.GetRepository().GetMediaContentTableName());
            SqlCeCommand command = new SqlCeCommand();
            command.CommandText = sql;
            command.Parameters.Add(new SqlCeParameter("UUID", content.UUID));
            SQLCeHelper.ExecuteNonQuery(content.GetRepository().GetConnectionString(), command);
        }

        public object Execute(Query.IContentQuery<Models.MediaContent> query)
        {
            MediaContentQueryExecutor executor = new MediaContentQueryExecutor((MediaContentQuery)query);
            return executor.Execute();
        }

        #endregion
    }
}
