#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Membership
{
    public class SignOutMemberPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            SingOutCore(controllerContext);
            var redirectUrl = MemberPluginHelper.GetReturnUrl(controllerContext);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = controllerContext.HttpContext.Request.UrlReferrer.ToString();
            }

            if (controllerContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonResultData resultData = new JsonResultData();

                resultData.RedirectUrl = redirectUrl;
                resultData.Success = true;

                return new JsonResult() { Data = resultData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new RedirectResult(redirectUrl);
            }

        }

        public Dictionary<string, object> Parameters
        {
            get { return null; }
        }
        #endregion

        #region LoginCore
        protected virtual void SingOutCore(ControllerContext controllerContext)
        {
            controllerContext.HttpContext.Membership().SignOut();
        }
        #endregion

        #region IHttpMethodPagePlugin
        public System.Web.Mvc.ActionResult HttpGet(View.Page_Context context, View.PagePositionContext positionContext)
        {
            return null;
        }

        public System.Web.Mvc.ActionResult HttpPost(View.Page_Context context, View.PagePositionContext positionContext)
        {
            SingOutCore(context.ControllerContext);

            var redirectUrl = MemberPluginHelper.GetReturnUrl(context.ControllerContext);
            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = context.ControllerContext.HttpContext.Request.UrlReferrer.ToString();
            }
            return new RedirectResult(redirectUrl);
        }
        #endregion
    }
}
