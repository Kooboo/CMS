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
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using System.ServiceModel.Activation;
using Kooboo.CMS.Content.FileServer.Web.Services;

namespace Kooboo.CMS.Content.FileServer.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private static void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("MediaContentService",
                new WebServiceHostFactory(), typeof(MediaContentService)));
            RouteTable.Routes.Add(new ServiceRoute("MediaFolderService",
                new WebServiceHostFactory(), typeof(MediaFolderService)));
            RouteTable.Routes.Add(new ServiceRoute("TextContentFileService",
                new WebServiceHostFactory(), typeof(TextContentFileService)));

        }
       
    }
}