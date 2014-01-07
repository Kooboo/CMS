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
using System.Web;

namespace Kooboo.CMS.Sites.Membership
{
    public class ChangeMemberPasswordPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public ChangeMemberPasswordPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();

            if (!ChangePasswordCore(controllerContext, submissionSetting))
            {
                resultData.AddModelState(controllerContext.Controller.ViewData.ModelState);
                resultData.Success = false;
            }
            else
            {
                resultData.RedirectUrl = MemberPluginHelper.GetReturnUrl(controllerContext);
                resultData.Success = true;
            }
            return new JsonResult() { Data = resultData };
        }

        public Dictionary<string, object> Parameters
        {
            get { return new Dictionary<string, object>() { { "OldPassword", "{OldPassword}" }, { "NewPassword", "{NewPassword}" }, { "ConfirmPassword", "{ConfirmPassword}" }}; }
        }
        #endregion

        #region ChangePasswordCore
        protected virtual bool ChangePasswordCore(ControllerContext controllerContext, SubmissionSetting submissionSetting = null)
        {
            var memberAuth = controllerContext.HttpContext.Membership();
            if (memberAuth.GetMember().Identity.IsAuthenticated == false)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "");
            }

            var membership = MemberPluginHelper.GetMembership();

            var model = new ChangeMemberPasswordModel();
            bool valid = ModelBindHelper.BindModel(model, "", controllerContext, submissionSetting);
            if (valid)
            {
                valid = _manager.Validate(membership, memberAuth.GetMember().Identity.Name, model.OldPassword);
                if (valid)
                {
                    _manager.ChangePassword(membership, memberAuth.GetMember().Identity.Name, model.NewPassword);
                    valid = true;
                }
                else
                {
                    valid = false;
                    controllerContext.Controller.ViewData.ModelState.AddModelError("OldPassword", "The old password is incorrect.".RawLabel().ToString());
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

            var isValid = ChangePasswordCore(context.ControllerContext);
            var redirectUrl = MemberPluginHelper.GetReturnUrl(context.ControllerContext);

            if (isValid && !string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = isValid;
            return null;
        }
        #endregion
    }
}
