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
    public class DomainSetting
    {
        public string[] Domains { get; set; }
        public string SitePath { get; set; }
        public string ResourceDomain { get; set; }
        /// <summary>
        /// 相同同域名根据UserAgent查找不同站点
        /// </summary>
        public string UserAgent { get; set; }
    }
}
