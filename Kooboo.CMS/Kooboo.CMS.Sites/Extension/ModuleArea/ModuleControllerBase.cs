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

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    /// <summary>
    /// The base controller of front end
    /// The other option is to use the ModulActionFilter, it will allow you to inherit from the Controller instead of ModuleControllerBase
    /// </summary>
    public class ModuleControllerBase : Controller
    {
        #region .ctor
        static ModuleControllerBase()
        {

        }
        public ModuleControllerBase()
        {

        }
        #endregion

        #region ModuleContext
        public ModuleContext ModuleContext
        {
            get
            {
                return ((ModuleRequestContext)(ControllerContext.RequestContext)).ModuleContext;
            }
        }
        #endregion

        #region EnableTheme
        public virtual bool EnableTheme
        {
            get
            {
                return ModuleContext.EnableTheme;
            }
            set
            {
                ModuleContext.EnableTheme = value;
            }
        }
        #endregion

        #region EnableScript
        public virtual bool EnableScript
        {
            get
            {
                return ModuleContext.EnableScript;
            }
            set
            {
                ModuleContext.EnableScript = value;
            }
        }
        #endregion

        #region PageControllerContext
        public ControllerContext PageControllerContext
        {
            get
            {
                return ((ModuleRequestContext)(ControllerContext.RequestContext)).PageControllerContext;
            }
        }
        #endregion

        #region Json
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                if (Request.ContentType.Contains("multipart/form-data;"))
                {
                    contentType = "text/plain";
                }
            }
            return base.Json(data, contentType, contentEncoding, behavior);
        }
        #endregion
    }
}
