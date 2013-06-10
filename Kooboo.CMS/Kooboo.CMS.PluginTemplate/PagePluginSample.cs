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
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.View;
using System.Web;
using System.ComponentModel;
using System.IO;
using System.Web.Mvc;

namespace Kooboo.CMS.PluginTemplate
{
    public interface IResponseManager
    {
        void SetHeader(string name, string value);
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IResponseManager))]
    public class ResponseManager : IResponseManager
    {
        public void SetHeader(string name, string value)
        {
            HttpContext.Current.Response.AddHeader(name, "GET");
        }
    }
    [Description("Sample plugin")]
    public class PagePluginSample : IHttpMethodPagePlugin
    {
        IResponseManager _responseManager;
        public PagePluginSample(IResponseManager responseManager)
        {
            _responseManager = responseManager;
        }
       
        public System.Web.Mvc.ActionResult HttpGet(Page_Context context, PagePositionContext positionContext)
        {
            _responseManager.SetHeader("SamplePlugin", "GET");     
            return null;
        }

        public System.Web.Mvc.ActionResult HttpPost(Page_Context context, PagePositionContext positionContext)
        {
            _responseManager.SetHeader("SamplePlugin", "POST");
            return null;
        }       
    }
}
