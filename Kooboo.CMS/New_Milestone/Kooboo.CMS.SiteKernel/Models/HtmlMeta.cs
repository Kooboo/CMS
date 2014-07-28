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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public class HtmlMeta
    {
        public string Author { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> Customs
        {
            get;
            set;
        }
        public string HtmlTitle { get; set; }
        public string Canonical { get; set; }
    }
}
