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
using Kooboo.CMS.Common.Runtime;
namespace Kooboo.CMS.Common
{
    public class HttpApplicationHooker : System.Web.HttpApplication
    {
        protected virtual void RunEvents(Action<IHttpApplicationEvents> action)
        {
            var events = EngineContext.Current.ResolveAll<IHttpApplicationEvents>();

            foreach (var item in events)
            {
                action(item);
            }
        }
        #region Init
        public override void Init()
        {
            base.Init();

            RunEvents((events) =>
            {
                events.Init(this);
            });
        }

        #endregion

        #region Application_Start
        public virtual void Application_Start(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_Start(sender, e);
            });
        }
        #endregion

        #region Application_End
        public virtual void Application_End(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_End(sender, e);
            });
        }
        #endregion

        #region Application_AuthenticateRequest
        public virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_AuthenticateRequest(sender, e);
            });
        }
        #endregion

        #region Application_BeginRequest
        public virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_BeginRequest(sender, e);
            });
        }

        #endregion

        #region Application_EndRequest
        public virtual void Application_EndRequest(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_EndRequest(sender, e);
            });
        }
        #endregion

        #region Application_Error
        public virtual void Application_Error(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Application_Error(sender, e);
            });
        }
        #endregion

        #region Session_Start
        public virtual void Session_Start(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Session_Start(sender, e);
            });
        }
        #endregion

        #region Session_End
        public virtual void Session_End(object sender, EventArgs e)
        {
            RunEvents((events) =>
            {
                events.Session_End(sender, e);
            });
        }
        #endregion
    }
}
