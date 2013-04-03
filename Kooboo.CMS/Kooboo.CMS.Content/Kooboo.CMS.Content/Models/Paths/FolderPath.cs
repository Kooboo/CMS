using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Web.Url;
using System.IO;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class FolderPath : IPath
    {
        public FolderPath(Folder folder)
        {
            var repositoryPath = new RepositoryPath(folder.Repository);
            if (folder.Parent != null)
            {
                var parentPath = new FolderPath(folder.Parent);
                this.PhysicalPath = Path.Combine(parentPath.PhysicalPath, folder.Name);
                VirtualPath = UrlUtility.Combine(parentPath.VirtualPath, folder.Name);
            }
            else
            {
                this.PhysicalPath = Path.Combine(repositoryPath.PhysicalPath, GetRootPath(folder.GetType()), folder.Name);
                VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, GetRootPath(folder.GetType()), folder.Name);
            }
            this.SettingFile = Path.Combine(PhysicalPath, PathHelper.SettingFileName);
        }

        public static string GetBaseDir<T>(Repository repository)
        {
            var repositoryPath = new RepositoryPath(repository);

            return Path.Combine(repositoryPath.PhysicalPath, GetRootPath(typeof(T)));
        }
        private static string GetRootPath(Type folderType)
        {
            if (folderType == typeof(TextFolder))
            {
                return "Folders";
            }
            else
            {
                return "Media";
            }
        }
        #region IPath Members

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string SettingFile
        {
            get;
            private set;
        }

        #endregion

        #region IPath Members


        public bool Exists()
        {
            return Directory.Exists(this.PhysicalPath);
        }

        #endregion

        #region IPath Members


        public void Rename(string newName)
        {
            IO.IOUtility.RenameDirectory(this.PhysicalPath, @newName);
        }

        #endregion
    }
}
