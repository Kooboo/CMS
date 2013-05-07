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

namespace Kooboo.CMS.Common
{
    public interface IHttpApplicationEvents
    {
        void Init(HttpApplication httpApplication);
        void Application_Start(object sender, EventArgs e);
        void Application_End(object sender, EventArgs e);
        void Application_AuthenticateRequest(object sender, EventArgs e);
        void Application_BeginRequest(object sender, EventArgs e);
        void Application_EndRequest(object sender, EventArgs e);
        void Application_Error(object sender, EventArgs e);
        void Session_Start(object sender, EventArgs e);
        void Session_End(object sender, EventArgs e);
    }
    public class HttpApplicationEvents : IHttpApplicationEvents
    {

        public virtual void Init(HttpApplication httpApplication)
        {

        }

        public virtual void Application_Start(object sender, EventArgs e)
        {

        }

        public virtual void Application_End(object sender, EventArgs e)
        {

        }

        public virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        public virtual void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        public virtual void Application_EndRequest(object sender, EventArgs e)
        {

        }

        public virtual void Application_Error(object sender, EventArgs e)
        {

        }

        public virtual void Session_Start(object sender, EventArgs e)
        {

        }

        public virtual void Session_End(object sender, EventArgs e)
        {

        }
    }
}
