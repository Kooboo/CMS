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
using System.Web.Routing;

namespace Kooboo.Web.Mvc
{
    public class SelectListItemEx : SelectListItem
    {
        public IDictionary<string, object> HtmlAttributes { get; set; }
    }
}
