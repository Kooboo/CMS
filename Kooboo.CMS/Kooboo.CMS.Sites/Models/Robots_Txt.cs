using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class Robots_Txt : FileResource
    {
        public Robots_Txt(Site site)
            : base(site, "robots.txt")
        {

        }
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[0]; }
        }
    }
}
