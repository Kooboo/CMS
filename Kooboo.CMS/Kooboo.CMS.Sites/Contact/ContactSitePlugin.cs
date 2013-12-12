using Kooboo.CMS.Common;
using Kooboo.CMS.Common.DataViolation;
using Kooboo.CMS.Membership.Services;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using Kooboo.CMS.Sites.Extension;

namespace Kooboo.CMS.Sites.Contact {
    public class ContactSitePlugin : IHttpMethodPagePlugin, ISubmissionPlugin {
        #region .ctor
        MembershipUserManager _manager;
        public ContactSitePlugin(MembershipUserManager manager) {
            _manager = manager;
        }
        #endregion

        #region ISubmissionPlugin

        public System.Web.Mvc.ActionResult Submit(Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Models.SubmissionSetting submissionSetting) {
            JsonResultData resultData = new JsonResultData();
            string redirectUrl;
            if (!ContactSiteCore(controllerContext, submissionSetting, out redirectUrl)) {
                resultData.AddModelState(controllerContext.Controller.ViewData.ModelState);
                resultData.Success = false;
            } else {
                resultData.RedirectUrl = redirectUrl;
                resultData.Success = true;
            }
            return new JsonResult() { Data = resultData };

        }

        public Dictionary<string, object> Parameters {
            get {
                return new Dictionary<string, object>() {
                    {"From", "{From}" },                 
                    {"Subject", "{Subject}"},
                    {"Body","{Body}"},
                    {"EmailBody","<b>{0}</b> Sent an email <br/> With Subject: {1}<br/> With Message:{2}<br/>"}
                };
            }
        }
        #endregion

        #region ContactSiteCore
        protected virtual bool ContactSiteCore(ControllerContext controllerContext, SubmissionSetting submissionSetting, out string redirectUrl) {
            redirectUrl = "";

            var ContactSiteModel = new ContactSiteModel();

            bool valid = ModelBindHelper.BindModel(ContactSiteModel, "", controllerContext, submissionSetting);

            if (valid) {
                try {

                    SendMail(controllerContext, Site.Current, ContactSiteModel, controllerContext.HttpContext.Request.Files);

                } catch (DataViolationException e) {
                    controllerContext.Controller.ViewData.ModelState.FillDataViolation(e.Violations);
                    valid = false;
                } catch (Exception e) {
                    controllerContext.Controller.ViewData.ModelState.AddModelError("", e.Message);
                    Kooboo.HealthMonitoring.Log.LogException(e);
                    valid = false;
                }
            }
            return valid;
        }

        protected virtual void SendMail(ControllerContext controllerContext, Site site, ContactSiteModel ContactSiteModel, HttpFileCollectionBase files) {

            var from = ContactSiteModel.From;
            var subject = ContactSiteModel.Subject;
            var body = string.Format(ContactSiteModel.EmailBody ,ContactSiteModel.From, ContactSiteModel.Subject, ContactSiteModel.Body);

            site.SendMailToSiteManager(from, subject, body, true , files);
        }
        #endregion

        #region IHttpMethodPagePlugin
        public System.Web.Mvc.ActionResult HttpGet(View.Page_Context context, View.PagePositionContext positionContext) {
            return null;
        }

        public System.Web.Mvc.ActionResult HttpPost(View.Page_Context context, View.PagePositionContext positionContext) {
            System.Web.Helpers.AntiForgery.Validate();
            string redirectUrl;
            context.ControllerContext.Controller.ViewBag.ContactSuccess = ContactSiteCore(context.ControllerContext, null, out redirectUrl);
            if (!string.IsNullOrEmpty(redirectUrl)) {
                return new RedirectResult(redirectUrl);
            }
            return null;
        }
        #endregion
    }
}
