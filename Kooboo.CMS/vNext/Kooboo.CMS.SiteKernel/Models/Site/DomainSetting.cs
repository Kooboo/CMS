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
        string[] Domains { get; set; }
        string SitePath { get; set; }
        string ResourceDomain { get; set; }
        /// <summary>
        /// 相同同域名根据UserAgent查找不同站点
        /// </summary>
        string UserAgent { get; set; }
    }
}
