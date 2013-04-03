using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ResourceGridModel
    {
        /// <summary>
        /// Current Direcotry
        /// </summary>
        public DirectoryResource Directory { get; set; }


        public IEnumerable<DirectoryEntry> Directories { get; set; }

        public IEnumerable<FileEntry> Files { get; set; }
    }
}