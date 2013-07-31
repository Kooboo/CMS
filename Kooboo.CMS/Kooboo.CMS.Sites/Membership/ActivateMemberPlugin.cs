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
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Common.DataViolation;

namespace Kooboo.CMS.Sites.Membership
{
    public class ActivateMemberPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public ActivateMemberPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();
            string redirectUrl;
            if (!ActivateCore(controllerContext, submissionSetting, out redirectUrl))
            {
                resultData.AddModelState(controllerContext.Controller.ViewData.ModelState);
                resultData.RedirectUrl = redirectUrl;
                resultData.Success = false;
            }
            else
            {
                resultData.RedirectUrl = redirectUrl;
                resultData.Success = true;
            }
            return new JsonResult() { Data = resultData };
        }

        public Dictionary<string, object> Parameters
        {
            get { return new Dictionary<string, object>() { { "Member", "{Member}" }, { "Code", "{Code}" }, { "SuccessUrl", "" }, { "FailedUrl", "" } }; }
        }
        #endregion

        #region ActivateCore
        protected virtual bool ActivateCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out string redirectUrl)
        {
            redirectUrl = "";

            var membership = MemberPluginHelper.GetMembership();

            var model = new ActivateMemberModel();
            bool valid = ModelBindHelper.BindModel(model, "", controllerContext, submissionSetting);
            if (valid)
            {
                try
                {
                    valid = _manager.Activate(membership, model.Member, model.Code);
                    if (valid)
                    {
                        redirectUrl = MemberPluginHelper.ResolveSiteUrl(controllerContext, model.SuccessUrl);
                    }
                    else
                    {
                        redirectUrl = MemberPluginHelper.ResolveSiteUrl(controllerContext, model.FailedUrl);
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
            string redirectUrl;

            var isValid = ActivateCore(context.ControllerContext, null, out redirectUrl);

            if (isValid && !string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = isValid;
            return null;
        }

        public System.Web.Mvc.ActionResult HttpPost(View.Page_Context context, View.PagePositionContext positionContext)
        {
            System.Web.Helpers.AntiForgery.Validate();
            return HttpGet(context, positionContext);
        }
        #endregion
    }
}
