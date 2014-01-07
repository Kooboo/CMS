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
using System.Web;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Grid
{
    public interface IItemColumnRender
    {
        IHtmlString Render(object dataItem, object value, ViewContext viewContext);
    }
}
