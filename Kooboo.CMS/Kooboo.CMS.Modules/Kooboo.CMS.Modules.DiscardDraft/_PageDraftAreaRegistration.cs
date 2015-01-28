#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.DiscardDraft
{
    public class _PageDraftAreaRegistration : AreaRegistrationEx
    {
        public override string AreaName
        {
            get { return "_PageDraft"; }
        }
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            context.MapRoute(
               "_PageDraft_default",
               "_PageDraft/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               new[] { "Kooboo.CMS.Modules.DiscardDraft", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
           );
            base.RegisterArea(context);
        }
    }
}
