using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Web.Url;
using System.IO;
using Kooboo.CMS.Form;


namespace Kooboo.CMS.Content.Models.Paths
{
    public class SchemaPath : IPath
    {
        public SchemaPath(Schema schema)
        {
            RepositoryPath repositoryPath = new RepositoryPath(schema.Repository);
            var basePhysicalPath = GetBaseDir(schema.Repository);
            this.PhysicalPath = Path.Combine(basePhysicalPath, schema.Name);
            this.SettingFile = Path.Combine(PhysicalPath, PathHelper.SettingFileName);
            VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, PATH_NAME, schema.Name);
        }
        public static string GetBaseDir(Repository repository)
        {
            return Path.Combine(new RepositoryPath(repository).PhysicalPath, PATH_NAME);
        }
        const string PATH_NAME = "Schemas";
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


        public bool Exists()
        {
            return File.Exists(this.SettingFile);
        }

        public void Rename(string newName)
        {
            IO.IOUtility.RenameFile(this.PhysicalPath, @newName + ".config");
        }

        #endregion       
    }
}
