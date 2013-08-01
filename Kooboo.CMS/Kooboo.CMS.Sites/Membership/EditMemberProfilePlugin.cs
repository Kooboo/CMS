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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Membership
{
    public class EditMemberProfilePlugin : IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region .ctor
        MembershipUserManager _manager;
        public EditMemberProfilePlugin(MembershipUserManager manager)
        {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting)
        {
            JsonResultData resultData = new JsonResultData();
            string redirectUrl;
            if (!EditCore(controllerContext, submissionSetting, out redirectUrl))
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
                    { "Email", "{Email}" },
                    { "PasswordQuestion", "{PasswordQuestion}" },
                    { "PasswordAnswer", "{PasswordAnswer}" },
                    { "Culture", "{Culture}" },
                    { "TimeZoneId", "{TimeZoneId}" },
                    { "Profiles[0].Key", "FirstName" },
                    { "Profiles[0].Value", "{FirstName}" },
                    { "Profiles[1].Key", "LastName" },
                    { "Profiles[1].Value", "{LastName}" }                   
                };
            }
        }
        #endregion

        #region EditCore
        protected virtual bool EditCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out string redirectUrl)
        {
            redirectUrl = "";

            var memberAuth = controllerContext.HttpContext.Member();
            if (memberAuth.GetMember().Identity.IsAuthenticated == false)
            {
                throw new HttpException((int)System.Net.HttpStatusCode.Unauthorized, "");
            }
            var membership = MemberPluginHelper.GetMembership();

            var membershipUser = controllerContext.HttpContext.Member().GetMembershipUser();

            var editMemberModel = new EditMemberProfileModel();

            bool valid = ModelBindHelper.BindModel(editMemberModel, "", controllerContext, submissionSetting);

            if (valid)
            {
                try
                {
                    _manager.EditMemberProfile(membership, memberAuth.GetMember().Identity.Name, editMemberModel.Email, editMemberModel.Culture,
                        editMemberModel.TimeZoneId, editMemberModel.PasswordQuestion, editMemberModel.PasswordAnswer, editMemberModel.Profiles);
                    valid = true;
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
            string redirectUrl;
            context.ControllerContext.Controller.ViewBag.MembershipSuccess = EditCore(context.ControllerContext, null, out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            return null;
        }
        #endregion
    }
}
