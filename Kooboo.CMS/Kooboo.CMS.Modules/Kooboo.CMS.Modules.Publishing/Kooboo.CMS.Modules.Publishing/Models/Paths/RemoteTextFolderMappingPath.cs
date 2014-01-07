using Kooboo.CMS.Common;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models.Paths
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPath<RemoteTextFolderMapping>))]
    public class RemoteTextFolderMappingPath:IPath<RemoteTextFolderMapping>
    {
        const string PATH_NAME = "RemoteTextFolderMappings";
        public RemoteTextFolderMappingPath(IBaseDir baseDir)
        {
            this.PhysicalPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PathHelper.PublishingFolderName, PATH_NAME);
            this.VirtualPath = UrlUtility.Combine(baseDir.Cms_DataVirtualPath, PathHelper.PublishingFolderName, PATH_NAME);
        }
        public RemoteTextFolderMappingPath(RemoteTextFolderMapping entity, IBaseDir baseDir)
        {
            this.PhysicalPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PathHelper.PublishingFolderName, PATH_NAME);
            this.VirtualPath = UrlUtility.Combine(baseDir.Cms_DataVirtualPath, PathHelper.PublishingFolderName, PATH_NAME);
            this.DataFile = Path.Combine(this.PhysicalPath, entity.UUID + ".config");
        }

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


        public string DataFile
        {
            get;
            private set;
        }
    }
}
