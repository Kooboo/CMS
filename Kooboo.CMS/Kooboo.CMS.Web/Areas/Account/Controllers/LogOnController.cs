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

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    public class LogOnController : ControllerBase
    {
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
                var isLockout = false;
                if (ServiceFactory.UserManager.ValidateUser(loginModel.UserName, loginModel.Password, out isLockout) != null)
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
            return View();
        }
        [HttpPost]
        public virtual ActionResult Ajax(LoginModel loginModel, int redirect)
        {
            var resultData = new JsonResultEntry();
            try
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
                    ModelState.AddModelError("UserName", "Username and/or password are incorrect.".Localize());
                }

                resultData.AddModelState(ModelState);
            }
            catch (Exception e)
            {
                resultData.AddException(e);
            }
            return Json(resultData);
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

                    ServiceFactory.UserManager.SendResetPasswordLink(model.UserName, model.Email, (user, token) =>
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
            if (!ServiceFactory.UserManager.ValidatePasswordToken(userName, token))
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

                    ServiceFactory.UserManager.ResetPasswordByToken(model.UserName, model.Token, model.NewPassword);

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
