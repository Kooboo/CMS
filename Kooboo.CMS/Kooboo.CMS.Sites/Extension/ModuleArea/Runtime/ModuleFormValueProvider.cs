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
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Helpers;
namespace Kooboo.CMS.Sites.Extension.ModuleArea.Runtime
{
    public class ModuleFormValueProvider : NameValueCollectionValueProvider
    {
        public ModuleFormValueProvider(ControllerContext controllerContext)
            : base(controllerContext.HttpContext.Request.Unvalidated().Form, CultureInfo.CurrentCulture)
        {

        }
    }

    public class ModuleQueryStringValueProvider : NameValueCollectionValueProvider
    {
        public ModuleQueryStringValueProvider(ControllerContext controllerContext)
            : base(controllerContext.HttpContext.Request.QueryString, CultureInfo.CurrentCulture)
        {

        }
    }
}
