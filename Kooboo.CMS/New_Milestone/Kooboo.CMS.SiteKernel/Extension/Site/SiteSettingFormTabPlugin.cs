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

namespace Kooboo.CMS.SiteKernel.Extension.Site
{
    public abstract class SiteSettingFormTabPlugin : AbstractFormTabBase
    {
        public override IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo
        {
            get { return new[] { SiteExtensionPoints.SiteSetting }; }
        }
    }
}
