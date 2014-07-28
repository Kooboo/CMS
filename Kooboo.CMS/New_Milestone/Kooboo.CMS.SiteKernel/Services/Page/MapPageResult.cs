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
    public class MapPageResult
    {
        public Page Page { get; set; }
        /// <summary>
        /// 用于标识页面的哪段URL Path
        /// </summary>
        public string MatchedVirtualPath { get; set; }
        /// <summary>
        /// 用于传参的那段URL Path
        /// </summary>
        public string QueryStringPath { get; set; }
    }
}
