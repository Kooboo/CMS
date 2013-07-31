#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.DataViolation;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
using Kooboo.CMS.Sites.View;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Membership
{
    public class ForgotPasswordPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public ForgotPasswordPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();
            string redirectUrl;
            if (!ForgotPasswordCore(controllerContext, submissionSetting, out redirectUrl))
            {
                resultData.AddModelState(controllerContext.Controller.ViewData.ModelState);
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
            get
            {
                return new Dictionary<string, object>() {
                    { "UserName", "{UserName}" },                 
                    {"RedirectUrl", "~/Member/ConfirmMail"},
                    {"EmailSubject","Kooboo CMS SampleSite"},
                    {"EmailBody","Please click <a href='{0}'>here</a> to reset your password."},
                    {"ResetPasswordUrl","~/Member/ResetPassword?member={0}&code={1}"}
                };
            }
        }
        #endregion

        #region ForgotPasswordCore
        protected virtual bool ForgotPasswordCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out string redirectUrl)
        {
            redirectUrl = "";
            var membership = MemberPluginHelper.GetMembership();

            var forgotPasswordModel = new ForgotPasswordModel();

            bool valid = ModelBindHelper.BindModel(forgotPasswordModel, "", controllerContext, submissionSetting);

            if (valid)
            {
                redirectUrl = forgotPasswordModel.RedirectUrl;
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = MemberPluginHelper.ResolveSiteUrl(controllerContext, redirectUrl);
                }
                try
                {
                    var membershipUser = _manager.ForgotPassword(membership, forgotPasswordModel.UserName);

                    SendMail(controllerContext, Site.Current, membershipUser, forgotPasswordModel);

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

        protected virtual void SendMail(ControllerContext controllerContext, Site site, MembershipUser membershipUser, ForgotPasswordModel forgotPasswordModel)
        {
            var resetPasswordUrl = forgotPasswordModel.ResetPasswordUrl;
            if (string.IsNullOrEmpty(resetPasswordUrl))
            {
                throw new ArgumentNullException("ResetPasswordUrl is required.");
            }

            resetPasswordUrl = string.Format(MemberPluginHelper.ResolveSiteUrl(controllerContext, resetPasswordUrl)
                , forgotPasswordModel.UserName, membershipUser.ActivateCode);
            resetPasswordUrl = UrlUtility.ToHttpAbsolute(resetPasswordUrl);

            var subject = forgotPasswordModel.EmailSubject;
            var body = string.Format(forgotPasswordModel.EmailBody, resetPasswordUrl);

            site.SendMailToCustomer(membershipUser.Email, subject, body, true, null);
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
            string redirectUrl;
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = ForgotPasswordCore(context.ControllerContext, null, out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            return null;
        }
        #endregion
    }
}
