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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Web.Areas.Account.Models;
using Kooboo.Connect;
using Kooboo.CMS.Sites;
using Kooboo.Globalization;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.DataViolation;
namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    public class LogOnController : ControllerBase
    {
        public UserManager UserManager { get; private set; }
        public LogOnController(UserManager userManager)
        {
            UserManager = userManager;
        }
        public virtual ActionResult Index()
        {
            if (Session["Message"] != null)
            {
                ModelState.AddModelError("", Session["Message"].ToString());
                Session.Remove("Message");
            }
            return View();
        }
        [HttpPost]
        public virtual ActionResult Index(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var isLockout = false;
                    if (UserManager.ValidateUser(loginModel.UserName, loginModel.Password, out isLockout) != false)
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(loginModel.UserName, loginModel.RememberMe);

                        if (loginModel.RedirectToHome)
                        {
                            return Redirect(Url.Content("~/"));
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return Redirect(System.Web.Security.FormsAuthentication.DefaultUrl);
                            }
                        }
                    }
                    else
                    {
                        if (isLockout)
                        {
                            ModelState.AddModelError("", "Your account has been locked for security reasons, please contact the admin.".Localize());
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username and/or password are incorrect.".Localize());
                        }

                    }
                }
                catch (DataViolationException e)
                {
                    ModelState.FillDataViolation(e.Violations);                    
                }
            }
            return View();
        }
        [HttpPost]
        public virtual ActionResult Ajax(LoginModel loginModel, int redirect)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    if (UserServices.ValidateUser(loginModel.UserName, loginModel.Password) != null)
                    {
                        System.Web.Security.FormsAuthentication.SetAuthCookie(loginModel.UserName, loginModel.RememberMe);
                        if (redirect == 0)
                        {
                            resultData.RedirectUrl = Request.UrlReferrer.ToString();
                        }
                        else
                        {
                            resultData.RedirectUrl = System.Web.Security.FormsAuthentication.DefaultUrl;
                        }

                    }
                    else
                    {
                        resultData.AddFieldError("UserName", "Username and/or password are incorrect.".Localize());
                    }
                });
            }
            return Json(data);
        }
        public virtual ActionResult SignOut(string returnUrl)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = System.Web.Security.FormsAuthentication.LoginUrl;
            }
            return Redirect(returnUrl);
        }

        public virtual ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.UserName) && string.IsNullOrEmpty(model.Email))
                {
                    ModelState.AddModelError("", "Username or Email is required.".Localize());
                }
                if (ModelState.IsValid)
                {

                    UserManager.SendResetPasswordLink(model.UserName, model.Email, (user, token) =>
                        new Uri(Request.Url, Url.Action("ResetPassword", new { UserName = user.UserName, token = token })).ToString()
                        );

                    Session["Message"] = "An email with instructions to choose a new password has been sent to you.".Localize();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }

        public virtual ActionResult ResetPassword(string userName, string token)
        {
            if (!UserManager.ValidatePasswordToken(userName, token))
            {
                ViewBag.Message = "The password token is invalid.".Localize();
            }
            return View();
        }
        [HttpPost]
        public virtual ActionResult ResetPassword(ResetPasswordByTokenModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    UserManager.ResetPasswordByToken(model.UserName, model.Token, model.NewPassword);

                    Session["Message"] = "The password was reset.".Localize();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }
    }
}
