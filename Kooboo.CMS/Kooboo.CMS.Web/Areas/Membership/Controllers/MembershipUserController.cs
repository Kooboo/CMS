using Kooboo.CMS.Common;
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Member.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Web.Areas.Membership.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Member.Persistence;
using Kooboo.CMS.Common.DataViolation;

namespace Kooboo.CMS.Web.Areas.Membership.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Member", Group = "", Name = "Membership", Order = 1)]
    public class MembershipUserController : ControllerBase
    {
        #region .ctor
        MembershipUserManager _manager = null;
        IMembershipUserProvider _membershipUserProvider = null;
        public MembershipUserController(MembershipUserManager manager, IMembershipUserProvider provider)
        {
            this._manager = manager;
            this._membershipUserProvider = provider;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string search, string sortField, string sortDir)
        {
            return View(_manager.All(Membership, search).SortBy(sortField, sortDir));
        }
        #endregion

        #region Create
        public virtual ActionResult Create()
        {
            var membershipUser = new CreateMembershipUserModel()
            {
                IsApproved = true
            };
            return View(membershipUser);
        }
        [HttpPost]
        public virtual ActionResult Create(CreateMembershipUserModel model, string @return)
        {
            JsonResultData data = new JsonResultData();
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    try
                    {
                        _manager.Create(Membership, model.UserName, model.Email, model.Password, model.IsApproved, model.Culture, model.TimeZoneId, model.PasswordQuestion, model.PasswordAnswer
                        , model.MembershipGroups, model.Profiles, model.Comment);
                        resultData.RedirectUrl = @return;
                    }
                    catch (DataViolationException dataViolationException)
                    {
                        ModelState.FillDataViolation(dataViolationException.Violations);
                    }
                });
            }
            data.AddModelState(ModelState);
            return Json(data);
        }

        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            var membershipUser = new MembershipUser() { Membership = Membership, UUID = uuid }.AsActual();
            var editModel = new EditMembershipUserModel(membershipUser);
            return View(editModel);
        }
        [HttpPost]
        public virtual ActionResult Edit(EditMembershipUserModel model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _manager.Edit(Membership, model.UserName, model.Email, model.IsApproved, model.IsLockedOut, model.Culture, model.TimeZoneId, model.PasswordQuestion, model.PasswordAnswer
                        , model.MembershipGroups, model.Profiles, model.Comment);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region IsUserNameAvailable
        public virtual ActionResult IsUserNameAvailable(string userName)
        {
            var membershipUser = new MembershipUser() { Membership = Membership, UserName = userName }.AsActual();
            if (membershipUser != null)
            {
                return Json(string.Format("{0} is duplicate.".Localize(), userName), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IsEmailAvailable
        public virtual ActionResult IsEmailAvailable(string email)
        {
            var membershipUser = _membershipUserProvider.QueryUserByEmail(Membership, email);
            if (membershipUser != null)
            {
                return Json(string.Format("{0} is duplicate.".Localize(), email), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(MembershipUser[] model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var o in model)
                    {
                        o.Membership = Membership;
                        _manager.Delete(o);
                    }
                }

                data.ReloadPage = true;
            });

            return Json(data);

        }
        #endregion

        #region ChangePassword
        public virtual ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult ChangePassword(ChangePasswordModel model, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _manager.ChangePassword(Membership, model.UUID, model.NewPassword);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion
    }
}
