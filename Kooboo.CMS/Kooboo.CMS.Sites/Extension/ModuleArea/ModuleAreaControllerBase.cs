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

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    /// <summary>
    /// The base controller of back end
    /// </summary>
    public class ModuleAreaControllerBase : AreaControllerBase
    {
        public string ModuleName
        {
            get
            {
                return Kooboo.Common.Web.AreaHelpers.GetAreaName(RouteData);
            }
        }
    }
}
