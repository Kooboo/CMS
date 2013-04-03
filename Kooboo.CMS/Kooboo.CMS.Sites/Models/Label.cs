using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kooboo.CMS.Sites.Models
{
    public class Label : DirectoryResource
    {
        public Label(Site site)
            : base(site, "Labels")
        {
        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }

        internal override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            return relativePaths.Take(relativePaths.Count() - 1);
        }
    }
}
