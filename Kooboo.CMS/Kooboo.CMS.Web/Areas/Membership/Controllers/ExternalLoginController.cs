#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Member.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Member.OAuthClients;
using System.Net;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    public class ExternalLoginController : Controller
    {
        #region .ctor
        MembershipConnectManager _manager;
        MembershipUserManager _membershipUserManager;
        public ExternalLoginController(MembershipConnectManager manager, MembershipUserManager membershipUserManager)
        {
            this._manager = manager;
            this._membershipUserManager = membershipUserManager;
        }
        #endregion

        #region ExternalLoginResult
        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(Kooboo.CMS.Member.Models.Membership membership, string membershipConnectName, string returnUrl)
            {
                this.Membership = membership;
                this.MembershipConnnectName = membershipConnectName;
                ReturnUrl = returnUrl;
            }

            public Kooboo.CMS.Member.Models.Membership Membership { get; private set; }
            public string MembershipConnnectName { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Membership, MembershipConnnectName, context.HttpContext, ReturnUrl);
            }
        }
        #endregion

        #region ExternalLogin
        public ActionResult Index(string connectName, string returnUrl)
        {
            var membership = GetMembership();

            var callbackUrl = GetCallbackUrl(returnUrl);

            return new ExternalLoginResult(membership, connectName, callbackUrl);
        }

        private string GetCallbackUrl(string returnUrl)
        {
            var callbackUrl = Url.Action("ExternalLoginCallback", new { returnUrl = returnUrl });
            callbackUrl = Url.FrontUrl().WrapperUrl(callbackUrl).ToString();
            return callbackUrl;
        }

        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var membership = GetMembership();
            var callbackUrl = GetCallbackUrl(returnUrl);
            var result = OAuthWebSecurity.VerifyAuthentication(membership, this.HttpContext, callbackUrl);
            if (!result.IsSuccessful)
            {
                var reason = "";
                if (result.Error != null)
                {
                    reason = result.Error.Message;
                    Kooboo.HealthMonitoring.Log.LogException(result.Error);
                }
                return RedirectToAction("ExternalLoginFailure", new { returnUrl = returnUrl, reason = reason });
            }
            else
            {
                var membershipConnect = new MembershipConnect() { Membership = membership, Name = result.Provider }.AsActual();
                if (membershipConnect != null)
                {
                    var membershipUser = _membershipUserManager.CreateOrUpdateOAuthMember(membership, membershipConnect, result, null);

                    if (membershipUser.IsApproved && !membershipUser.IsLockedOut)
                    {
                        HttpContext.MemberAuthentication().SetAuthCookie(membershipUser.UserName, false);
                    }
                    else
                    {
                        return RedirectToAction("ExternalLoginFailure", new { returnUrl = returnUrl, reason = "The member was locked out." });
                    }
                }

            }
            return RedirectToLocal(returnUrl);

        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure(string reason, string returnUrl)
        {
            ViewData.Model = reason;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("~/");
            }
        }
        #endregion

        #region GetMembership
        private static Kooboo.CMS.Member.Models.Membership GetMembership()
        {
            if (Site.Current == null)
            {
                throw new NullSiteContextException();
            }

            var membership = Site.Current.GetMembership();
            if (membership == null)
            {
                throw new InvalidOperationException("The site does not relate to any membership.");
            }

            return membership;
        }
        #endregion

    }
}
