using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class RepositoryPath : IPath
    {
        static string PATH_NAME = "Contents";
        public static string Cms_Data = "Cms_Data";
        public RepositoryPath(Repository repository)
        {
            this.PhysicalPath = Path.Combine(BasePhysicalPath, repository.Name);
            this.VirtualPath = UrlUtility.Combine("~/", Cms_Data, PATH_NAME, repository.Name);
            this.SettingFile = Path.Combine(PhysicalPath, PathHelper.SettingFileName);
        }

        public static string BasePhysicalPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Cms_Data, PATH_NAME);

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
