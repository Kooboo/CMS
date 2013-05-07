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
using Kooboo.CMS.Account.Models;
using Kooboo.Globalization;
using System.Net.Mail;
using Kooboo.Extensions;
using Kooboo;
using Kooboo.CMS.Account.Persistence;
using Kooboo.Collections;

namespace Kooboo.CMS.Account.Services
{
    public class UserManager
    {
        public IUserProvider UserProvider { get; private set; }
        public ISettingProvider SettingProvider { get; private set; }
        public UserManager(IUserProvider userProvider, ISettingProvider settingProvider)
        {
            UserProvider = userProvider;
            SettingProvider = settingProvider;
        }
        public virtual void Add(User user)
        {
            user.Email = user.Email.ToLower();
            UserProvider.Add(user);
        }
        public virtual void Delete(string userName)
        {
            UserProvider.Remove(new User() { UserName = userName });
        }
        public virtual User Get(string userName)
        {
            return UserProvider.Get(new User() { UserName = userName });
        }
        public virtual IEnumerable<User> All()
        {
            return UserProvider.All();
        }
        public virtual void Update(string userName, User newUser)
        {
            var old = Get(userName);
            UserProvider.Update(newUser, old);
        }

        public virtual bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return UserProvider.ChangePassword(userName, oldPassword, newPassword);
        }
        public virtual bool ChangePassword(string userName, string newPassword)
        {
            return UserProvider.ChangePassword(userName, newPassword);
        }
        public virtual bool ValidateUser(string userName, string password)
        {
            return UserProvider.ValidateUser(userName, password);
        }

        public virtual User ValidateUser(string userName, string password, out bool isLockout)
        {
            isLockout = false;
            var setting = SettingProvider.Get();
            var cmsUser = Get(userName);
            if (cmsUser == null)
            {
                return null;
            }
            isLockout = cmsUser.IsLockedOut;
            if (cmsUser.IsLockedOut)
            {
                //锁定15分钟后解锁
                var time = (DateTime.UtcNow - cmsUser.LastLockoutDate);
                if (time.Minutes > setting.MinutesToUnlock)
                {
                    cmsUser.IsLockedOut = false;
                    cmsUser.FailedPasswordAttemptCount = 0;
                    Update(userName, cmsUser);
                }
                else
                {
                    isLockout = true;
                    return null;
                }
            }
            var connectUser = Kooboo.Connect.UserServices.ValidateUser(userName, password);
            if (connectUser == null && cmsUser != null)
            {
                cmsUser.FailedPasswordAttemptCount += 1;
                if (setting.EnableLockout && cmsUser.FailedPasswordAttemptCount >= setting.FailedPasswordAttemptCount)
                {
                    cmsUser.IsLockedOut = true;
                    cmsUser.LastLockoutDate = DateTime.UtcNow;
                    isLockout = true;
                }
                Update(userName, cmsUser);

                return null;
            }
            else
            {
                //登录成功后清除重试次数
                cmsUser.FailedPasswordAttemptCount = 0;
                Update(userName, cmsUser);
            }
            return cmsUser;
        }

        public virtual void SendResetPasswordLink(string userName, string email, Func<User, string, string> resetLinkGenerator)
        {
            User user;
            if (!string.IsNullOrEmpty(userName))
            {
                user = Get(userName);
            }
            else
            {
                user = All().Where(it => it.Email == email).FirstOrDefault();
            }
            if (user == null)
            {
                throw new Exception("The user does not exists.".Localize());
            }
            string randomValue = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(16);

            user.CustomFields = user.CustomFields ?? new DynamicDictionary();

            user.CustomFields["PasswordToken"] = randomValue;

            Update(user.UserName, user);

            string emailSubject = "Your password".Localize();
            string emailBody = "<b>{0}</b> <br/><br/> To change your password, click on the following link:<br/> <br/> <a href='{1}'>{1}</a> <br/>".Localize();

            SmtpClient smtp = new SmtpClient();
            MailMessage mail = new MailMessage();
            mail.To.Add(user.Email);
            mail.Subject = emailSubject;
            mail.Body = string.Format(emailBody, user.UserName, resetLinkGenerator(user, randomValue));
            mail.IsBodyHtml = true;
            smtp.Send(mail);
        }

        public virtual void ResetPasswordByToken(string userName, string token, string newPassword)
        {

            var user = Get(userName);
            if (user == null)
            {
                throw new Exception("The user does not exists.".Localize());
            }
            user.CustomFields = user.CustomFields ?? new DynamicDictionary();
            var passwordToken = user.CustomFields["PasswordToken"] == null ? "" : user.CustomFields["PasswordToken"].ToString();
            if (string.IsNullOrEmpty(passwordToken) || !passwordToken.EqualsOrNullEmpty(passwordToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("The password token is invalid.".Localize());
            }

            ChangePassword(userName, newPassword);

            user.CustomFields["PasswordToken"] = null;

            Update(user.UserName, user);

        }

        public virtual bool ValidatePasswordToken(string userName, string token)
        {
            var user = Get(userName);
            if (user == null)
            {
                return false;
            }
            user.CustomFields = user.CustomFields ?? new DynamicDictionary();
            var passwordToken = user.CustomFields["PasswordToken"] == null ? "" : user.CustomFields["PasswordToken"].ToString();
            if (string.IsNullOrEmpty(passwordToken) || !passwordToken.EqualsOrNullEmpty(token, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

    }
}
