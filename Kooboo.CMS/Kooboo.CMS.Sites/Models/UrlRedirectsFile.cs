using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class UrlRedirectsFile : FileResource
    {
        public static string FileName = "UrlRedirects.config";
        public UrlRedirectsFile(string physicalPath)
            : base(physicalPath)
        {

        }
        public UrlRedirectsFile(Site site)
            : base(site, FileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }
    }
}
