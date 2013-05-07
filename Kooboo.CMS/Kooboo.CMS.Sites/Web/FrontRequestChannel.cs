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

namespace Kooboo.CMS.Sites.Web
{
    public enum FrontRequestChannel
    {
        Unknown,
        /// <summary>
        /// s~site1
        /// </summary>
        Debug,
        /// <summary>
        /// www.site1.com
        /// </summary>
        Host,
        /// <summary>
        /// www.kooboo.com/site1
        /// </summary>
        HostNPath,
        /// <summary>
        /// 
        /// </summary>
        Design,
        Draft
    }
}
