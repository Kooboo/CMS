#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Extension.View
{
    public static class ViewExtensionPoints
    {
        public static MvcRoute View_Grid = new MvcRoute() { Area = "Sites", Controller = " View", Action = "Index" };
        public static MvcRoute View_Create = new MvcRoute() { Area = "Sites", Controller = " View", Action = "Create" };
        public static MvcRoute View_Edit = new MvcRoute() { Area = "Sites", Controller = " View", Action = "Edit" };
    }
}
