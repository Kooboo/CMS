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
    public class CustomErrorsFile : FileResource
    {
        public static string CustomErrorFileName = "CustomErrors.config";
        public CustomErrorsFile(string physicalPath)
            : base(physicalPath)
        {

        }
        public CustomErrorsFile(Site site)
            : base(site, CustomErrorFileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { yield return ""; }
        }
    }
}
