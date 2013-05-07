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
    public class UrlKeyMapsFile : FileResource
    {
        public static string UrlKeyMapFileName = "UrlKeyMaps.config";
        public UrlKeyMapsFile(string physicalPath)
            : base(physicalPath)
        {

        }
        public UrlKeyMapsFile(Site site)
            : base(site, UrlKeyMapFileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }
    }
}
