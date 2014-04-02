#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kooboo.CMS.Sites.Models
{
    public class LabelPath : DirectoryResource
    {
        public LabelPath(Site site)
            : base(site, "Labels")
        {
        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            return relativePaths.Take(relativePaths.Count() - 1);
        }
    }
}
