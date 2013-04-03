using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class SendingSettingPath : IPath
    {
        private static string DIR = "SendingSettings";
        public SendingSettingPath(Repository repository)
        {
            this.PhysicalPath = Path.Combine(new BroadcastingPath(repository).PhysicalPath, DIR);
            IO.IOUtility.EnsureDirectoryExists(PhysicalPath);
        }
        public SendingSettingPath(SendingSetting sendingSetting)
        {
            var fileName = sendingSetting.Name + ".config";
            this.SettingFile = this.PhysicalPath = Path.Combine(new BroadcastingPath(sendingSetting.Repository).PhysicalPath, DIR, fileName);
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

        public bool Exists()
        {
            return File.Exists(this.SettingFile);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
