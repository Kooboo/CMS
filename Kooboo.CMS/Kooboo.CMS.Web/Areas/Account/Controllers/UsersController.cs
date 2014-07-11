#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Account.Services;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web;
using Kooboo.CMS.Web.Areas.Account.Models;
using Kooboo.CMS.Web.Authorizations;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Globalization;
using Kooboo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Web;
namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 99)]
    public class UsersController : ControllerBase
    {
        #region .ctor
        public UserManager UserManager { get; private set; }
        public UsersController(UserManager userManager)
        {
            UserManager = userManager;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string search, int? page, int? pageSize, string sortField, string sortDir)
        {
            var users = UserManager.All();
            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(it => it.UserName.Contains(search));
            }
            users = users.AsQueryable().SortBy(sortField, sortDir);

            return View(users.ToPagedList<User>(page ?? 1, pageSize ?? 50));
        }
        #endregion

        #region Create

        public virtual ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(CreateUserModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    UserManager.Add(model.ToUser());
                    data.RedirectUrl = @return;
                });
            }
            return Json(data);

        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            return View(new EditUserModel(UserManager.Get(uuid)));
        }

        [HttpPost]
        public virtual ActionResult Edit(EditUserModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var userToUpdate = UserManager.Get(model.UserName);
                    UserManager.Update(model.UserName, model.ToUser(userToUpdate));
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }

        #endregion

        #region Delete
        public virtual ActionResult Delete(User[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var user in model)
                {
                    UserManager.Delete(user.UserName);
                }
                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string userName)
        {
            if (UserManager.Get(userName) != null)
            {
                return Json(string.Format("{0} is not available.".Localize(), userName), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ResetPassword
        public virtual ActionResult ResetPassword(string userName)
        {
            return View(new ResetPasswordModel() { UserName = userName });
        }
        [HttpPost]
        public virtual ActionResult ResetPassword(ResetPasswordModel password, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    UserManager.ChangePassword(password.UserName, password.NewPassword);
                    data.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region By user


        [RequiredLogOn(Order = 1, Exclusive = true)]
        public virtual ActionResult ChangePassword()
        {
            return View();
        }
        [RequiredLogOn(Order = 1, Exclusive = true)]
        [HttpPost]
        public virtual ActionResult ChangePassword(ChangePasswordModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    resultData.Success = UserManager.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    if (!resultData.Success)
                    {
                        resultData.AddMessage("Invalid old password.".Localize());
                    }
                    else
                    {
                        resultData.RedirectUrl = @return;
                    }
                });
            }
            return Json(data);
        }

        [RequiredLogOn(Order = 1, Exclusive = true)]
        public virtual ActionResult ChangeLanguage()
        {
            var user = UserManager.Get(User.Identity.Name);
            return View(new ChangeLanguageModel() { UICulture = user.UICulture });
        }

        [RequiredLogOn(Order = 1, Exclusive = true)]
        [HttpPost]
        public virtual ActionResult ChangeLanguage(ChangeLanguageModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var user = UserManager.Get(User.Identity.Name);
                    user.UICulture = model.UICulture;
                    UserManager.Update(User.Identity.Name, user);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }

        [RequiredLogOn(Order = 1, Exclusive = true)]
        [HttpPost]
        public virtual ActionResult SetDefaultPage(string defaultUrl, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var user = UserManager.Get(User.Identity.Name);
                    user.DefaultPage = defaultUrl;
                    UserManager.Update(User.Identity.Name, user);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion
    }
}
