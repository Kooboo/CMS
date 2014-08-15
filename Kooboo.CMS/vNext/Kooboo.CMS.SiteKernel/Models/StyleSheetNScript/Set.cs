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
    /// <summary>
    /// 样式或脚本文件分组
    /// </summary>
    public class Set
    {
        public string SetName { get; set; }

        public string[] Files { get; set; }
    }
}
