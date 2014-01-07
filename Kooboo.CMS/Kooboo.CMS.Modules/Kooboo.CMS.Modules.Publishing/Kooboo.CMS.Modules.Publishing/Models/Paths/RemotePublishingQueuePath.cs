using Kooboo.CMS.Common;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models.Paths
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPath<RemotePublishingQueue>))]
    public class RemotePublishingQueuePath : IPath<RemotePublishingQueue>
    {
        const string PATH_NAME = "RemotePublishingQueue";
        public RemotePublishingQueuePath(IBaseDir baseDir)
        {
            this.PhysicalPath = Path.Combine(baseDir.Cms_DataPhysicalPath, PathHelper.PublishingFolderName, PATH_NAME);
            this.VirtualPath = UrlUtility.Combine(baseDir.Cms_DataVirtualPath, PathHelper.PublishingFolderName, PATH_NAME);
        }
        public RemotePublishingQueuePath(RemotePublishingQueue entity, IBaseDir baseDir)
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
