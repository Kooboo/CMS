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
using System.Web.Routing;

namespace Kooboo.CMS.Sites.Extension.UI.TopToolbar
{
    public interface IToolbarProvider
    {
        MvcRoute[] ApplyTo { get; }

        IEnumerable<ToolbarGroup> GetGroups(RequestContext requestContext);

        IEnumerable<ToolbarButton> GetButtons(RequestContext requestContext);
    }
}
