#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web.Button;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Extension.Site
{
    /// <summary>
    /// 不生成链接的ButtonPlugin
    /// </summary>
    public abstract class SiteClusterButtonPlugin : ManagedButtonPlugin
    {
        public override IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo
        {
            get
            {
                return new[] { SiteExtensionPoints.SiteCluster };
            }
        }
    }
}
