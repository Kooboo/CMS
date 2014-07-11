#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.View.PositionRender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View
{
    using Kooboo.Common.ObjectContainer;
    public static class HtmlExtensions
    {
        public static FrontHtmlHelper FrontHtml(this HtmlHelper html)
        {
            return new FrontHtmlHelper(Page_Context.Current, html, EngineContext.Current.Resolve<ViewRender>(), EngineContext.Current.Resolve<ProxyRender>());
        }
    }
}
