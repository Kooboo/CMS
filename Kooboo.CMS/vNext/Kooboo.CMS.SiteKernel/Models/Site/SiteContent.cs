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
    public partial class SiteContent : ISiteObject
    {
        public Site Site
        {
            get;
            set;
        }
    }
    public partial class SiteContent
    {
        /// <summary>
        /// 
        /// 比如：Content Database的名称，Membership的名称，ECommerce的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据类型
        /// 比如：ContentDatabase, Membership,Commerce
        /// </summary>
        public string Type { get; set; }
    }
}
