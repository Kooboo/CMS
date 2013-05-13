#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension
{
    public interface IHttpMethodPagePlugin : ICommonPagePlugin
    {
        /// <summary>
        /// Write the page plug-in code when the page doing the Get request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="positionContext">The position context.</param>
        /// <returns></returns>
        ActionResult HttpGet(Page_Context context, PagePositionContext positionContext);

        /// <summary>
        /// Write the page plug-in code when the page doing the Post request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="positionContext">The position context.</param>
        /// <returns></returns>
        ActionResult HttpPost(Page_Context context, PagePositionContext positionContext);
    }
}
