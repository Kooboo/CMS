#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.FrontAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.Services
{
    public interface IViewRenderer
    {
        IHtmlString Render(IFrontHtmlHelper frontHtmlHelper, string viewName, ViewDataDictionary viewData, object parameters, bool executeDataRule);
    }
}
