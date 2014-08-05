#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public class SiteNode
    {
        public Site Site { get; set; }
        public bool AbleToEdit { get; set; }
        public IEnumerable<SiteNode> Children { get; set; }
    }
}
