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

namespace Kooboo.CMS.Sites.Models
{
    public class UrlRedirectsFile : FileResource
    {
        public static string UrlRedirectFileName = "UrlRedirects.config";
        public UrlRedirectsFile(string physicalPath)
            : base(physicalPath)
        {

        }
        public UrlRedirectsFile(Site site)
            : base(site, UrlRedirectFileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }
    }
}
