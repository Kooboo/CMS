#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;

using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;

using Kooboo.CMS.Sites.View;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Kooboo.Common.Web;
using Kooboo.Common.Data.DataViolation;

namespace Kooboo.CMS.Sites.Membership
{
    public class RegisterMemberPlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public RegisterMemberPlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();
            string redirectUrl;
            if (!RegisterCore(controllerContext, submissionSetting, out redirectUrl))
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
                    { "Email","{Email}"},
                    { "Password", "{Password}" },
                    { "ConfirmPassword", "{ConfirmPassword}" },
                    { "PasswordQuestion", "{PasswordQuestion}" },
                    { "PasswordAnswer", "{PasswordAnswer}" },
                    { "Culture", "{Culture}" },
                    { "TimeZoneId", "{TimeZoneId}" },
                    { "Profiles[0].Key", "FirstName" },
                    { "Profiles[0].Value", "{FirstName}" },
                    { "Profiles[1].Key", "LastName" },
                    { "Profiles[1].Value", "{LastName}" },
                    {"RedirectUrl", "~/Member/ConfirmMail"},
                    {"IsApproved","false"},
                    {"EmailSubject","Kooboo CMS SampleSite"},
                    {"EmailBody","You created Kooboo CMS samplesite account, please click <a href='{0}'>here</a> to activate your account."},
                    {"ActivateUrl","~/Member/Activate?member={0}&code={1}"}
                };
            }
        }
        #endregion

        #region RegisterCore
        protected virtual bool RegisterCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out string redirectUrl)
        {
            redirectUrl = "";
            var membership = MemberPluginHelper.GetMembership();

            var registerMemberModel = new RegisterMemberModel();

            bool valid = Kooboo.CMS.Sites.Extension.ModelBindHelper.BindModel(registerMemberModel, "", controllerContext, submissionSetting);

            if (valid)
            {
                redirectUrl = registerMemberModel.RedirectUrl;
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = MemberPluginHelper.ResolveSiteUrl(controllerContext, redirectUrl);
                }
                try
                {
                    var membershipUser = _manager.Create(membership, registerMemberModel.UserName, registerMemberModel.Email, registerMemberModel.Password, registerMemberModel.IsApproved, registerMemberModel.Culture
                    , registerMemberModel.TimeZoneId, registerMemberModel.PasswordQuestion, registerMemberModel.PasswordAnswer, null, registerMemberModel.Profiles, null);

                    if (registerMemberModel.IsApproved == false)
                    {
                        SendActivateMail(controllerContext, Site.Current, membershipUser, registerMemberModel, membershipUser.ActivateCode);
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
                   Kooboo.Common.Logging.Logger.Error(e.Message, e);
                    valid = false;
                }
            }
            return valid;
        }

        protected virtual void SendActivateMail(ControllerContext controllerContext, Site site, MembershipUser memberUser, RegisterMemberModel registerMemberModel, string activateCode)
        {
            var activateUrl = registerMemberModel.ActivateUrl;
            if (string.IsNullOrEmpty(activateUrl))
            {
                throw new ArgumentNullException("ActivateUrl is required.");
            }

            activateUrl = string.Format(MemberPluginHelper.ResolveSiteUrl(controllerContext, activateUrl)
                , memberUser.UserName, activateCode);
            activateUrl = UrlUtility.ToHttpAbsolute(activateUrl);

            var subject = registerMemberModel.EmailSubject;
            var body = string.Format(registerMemberModel.EmailBody, activateUrl);

            site.SendMailToCustomer(registerMemberModel.Email, subject, body, true, null);
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
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = RegisterCore(context.ControllerContext, null, out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            return null;
        }
        #endregion
    }
}
