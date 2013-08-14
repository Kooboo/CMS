#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Interoperability.MetaWeblog
{
    public class MetaWeblogRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var repositoryName = Repository.Current == null ? "" : Repository.Current.Name;
            return new MetaWeblogAPIHandler(repositoryName);
        }
    }
}