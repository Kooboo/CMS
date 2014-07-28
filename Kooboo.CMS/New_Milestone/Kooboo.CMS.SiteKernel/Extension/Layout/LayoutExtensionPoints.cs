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

namespace Kooboo.CMS.SiteKernel.Extension.Layout
{
    public static class LayoutExtensionPoints
    {
        public static MvcRoute Layout_Grid = new MvcRoute() { Area = "Sites", Controller = " Layout", Action = "Index" };
        public static MvcRoute Layout_Create = new MvcRoute() { Area = "Sites", Controller = " Layout", Action = "Create" };
        public static MvcRoute Layout_Edit = new MvcRoute() { Area = "Sites", Controller = " Layout", Action = "Edit" };
    }
}
