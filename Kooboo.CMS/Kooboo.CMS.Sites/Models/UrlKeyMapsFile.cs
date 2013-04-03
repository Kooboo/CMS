using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class UrlKeyMapsFile : FileResource
    {
        public static string FileName = "UrlKeyMaps.config";
        public UrlKeyMapsFile(string physicalPath)
            : base(physicalPath)
        {

        }
        public UrlKeyMapsFile(Site site)
            : base(site, FileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }
    }
}
