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
using System.Web.Mvc;
using Kooboo.CMS.Member.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Common.DataViolation;
using Kooboo.CMS.Sites.View;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Sites.Member
{
    public class ValidateMemberPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public ValidateMemberPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();
            ValidateMemberModel model;
            if (!LoginCore(controllerContext, submissionSetting, out model))
            {
                resultData.AddModelState(controllerContext.Controller.ViewData.ModelState);
                resultData.Success = false;
            }
            else
            {
                resultData.RedirectUrl = model.RedirectUrl;
                resultData.Model = model.UserName;
                resultData.Success = true;
            }
            return new JsonResult() { Data = resultData };
        }

        public Dictionary<string, object> Parameters
        {
            get { return new Dictionary<string, object>() { { "UserName", "{UserName}" }, { "Password", "{Password}" }, { "RedirectUrl", "~/Member/Profile" } }; }
        }
        #endregion

        #region LoginCore
        protected virtual bool LoginCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out ValidateMemberModel model)
        {

            var membership = MemberPluginHelper.GetMembership();

            model = new ValidateMemberModel();
            bool valid = ModelBindHelper.BindModel(model, "", controllerContext, submissionSetting);
            if (valid)
            {
                try
                {
                    valid = _manager.Validate(membership, model.UserName, model.Password);
                    if (valid)
                    {
                        controllerContext.HttpContext.MemberAuthentication().SetAuthCookie(model.UserName, model.RememberMe == null ? false : model.RememberMe.Value);

                        if (!string.IsNullOrEmpty(model.RedirectUrl))
                        {
                            model.RedirectUrl = MemberPluginHelper.ResolveSiteUrl(controllerContext, model.RedirectUrl);
                        }
                        if (!string.IsNullOrEmpty(MemberPluginHelper.GetReturnUrl(controllerContext)))
                        {
                            model.RedirectUrl = MemberPluginHelper.GetReturnUrl(controllerContext);
                        }
                    }
                    else
                    {
                        controllerContext.Controller.ViewData.ModelState.AddModelError("UserName", "Username and/or password are incorrect.".RawLabel().ToString());
                    }
                }
                catch (DataViolationException e)
                {
                    controllerContext.Controller.ViewData.ModelState.FillDataViolation(e.Violations);
                    valid = false;
                }
                catch (Exception e)
                {
                    controllerContext.Controller.ViewData.ModelState.AddModelError("", e.Message);
                    Kooboo.HealthMonitoring.Log.LogException(e);
                    valid = false;
                }
            }
            return valid;
        }


        #endregion

        #region IHttpMethodPagePlugin
        public System.Web.Mvc.ActionResult HttpGet(View.Page_Context context, View.PagePositionContext positionContext)
        {
            return null;
        }

        public System.Web.Mvc.ActionResult HttpPost(View.Page_Context context, View.PagePositionContext positionContext)
        {
            System.Web.Helpers.AntiForgery.Validate();
            ValidateMemberModel model;

            var isValid = LoginCore(context.ControllerContext, null, out model);

            if (isValid && !string.IsNullOrEmpty(model.RedirectUrl))
            {
                return new RedirectResult(model.RedirectUrl);
            }
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = isValid;
            return null;
        }
        #endregion
    }
}
