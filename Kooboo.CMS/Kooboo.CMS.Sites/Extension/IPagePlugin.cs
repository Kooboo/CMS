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
using System.Web.Mvc;
using Kooboo.CMS.Sites.Controllers;
using Kooboo.CMS.Sites.View;
using System.Web;

namespace Kooboo.CMS.Sites.Extension
{
    public interface IPagePlugin
    {
        /// <summary>
        /// Executes the specified page view context.
        /// </summary>
        /// <param name="pageContext">The page context.</param>
        /// <param name="positionContext">The value will be null when executing a plugin in page.</param>
        /// <returns></returns>
        ActionResult Execute(Page_Context pageContext, PagePositionContext positionContext);
    }
}
