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
using Kooboo.Common.Globalization;
using System.Net.Mail;
using Kooboo;
using Kooboo.CMS.Account.Persistence;

using Kooboo.Common.Misc;
using Kooboo.Common.Data.DataViolation;

namespace Kooboo.CMS.Account.Services
{
    public class UserManager
    {
        #region .ctor
        public IUserProvider UserProvider { get; private set; }
        public ISettingProvider SettingProvider { get; private set; }
        public PasswordProvider _passwordProvider;
        public UserManager(IUserProvider userProvider, ISettingProvider settingProvider, PasswordProvider passwordProvider)
        {
            UserProvider = userProvider;
            SettingProvider = settingProvider;
            _passwordProvider = passwordProvider;
        }
        #endregion

        #region Add
        public virtual void Add(User user)
        {
            #region Validate data
            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (UserProvider.Get(user) != null)
            {
                violations.Add(new DataViolationItem("UserName", user.UserName, "DuplicateUserName".Localize()));
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                violations.Add(new DataViolationItem("Password", user.Password, "InvalidPassword".Localize()));
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                violations.Add(new DataViolationItem("Email", user.Email, "InvalidEmail".Localize()));
            }
            if (UserProvider.FindUserByEmail(user.Email) != null)
            {
                violations.Add(new DataViolationItem("Email", user.Email, "DuplicateEmail".Localize()));
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }
            #endregion

            var salt = _passwordProvider.GenerateSalt();
            var encodedPassword = _passwordProvider.EncryptPassword(user.Password, salt);
            user.Password = encodedPassword;
            user.PasswordSalt = salt;
            UserProvider.Add(user);
        }
        #endregion

        #region Delete
        public virtual void Delete(string userName)
        {
            UserProvider.Remove(new User() { UserName = userName });
        }
        #endregion

        #region Get
        public virtual User Get(string userName)
        {
            return UserProvider.Get(new User() { UserName = userName });
        }
        #endregion

        #region All
        public virtual IEnumerable<User> All()
        {
            return UserProvider.All();
        }
        #endregion

        #region Update
        public virtual void Update(string userName, User newUser)
        {
            var old = Get(userName);
            UserProvider.Update(newUser, old);
        }
        #endregion

        #region ChangePassword
        public virtual bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (!ValidateUser(userName, oldPassword))
            {
                return false;
            }
            return ChangePassword(userName, newPassword);
        }
        public virtual bool ChangePassword(string userName, string newPassword)
        {
            var user = UserProvider.Get(new User() { UserName = userName });

            if (user == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }

            var salt = user.PasswordSalt;

            var encodedPassword = _passwordProvider.EncryptPassword(newPassword, salt);
            user.Password = encodedPassword;
            user.UtcLastPasswordChangedDate = DateTime.UtcNow;
            user.ActivateCode = null;

            Update(userName, user);

            return true;
        }
        #endregion

        #region ValidateUser

        public virtual bool ValidateUser(string userName, string password)
        {
            bool isLockedOut;

            return ValidateUser(userName, password, out isLockedOut);
        }

        public virtual bool ValidateUser(string userName, string password, out bool isLockout)
        {
            var user = UserProvider.Get(new User() { UserName = userName });
            var setting = SettingProvider.Get();
            isLockout = false;
            #region DataViolation
            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (user == null)
            {
                return false;
            }
            if (!user.IsApproved)
            {
                violations.Add(new DataViolationItem("UserName", userName, "UserNotApproved".Localize()));
            }
            if (user.IsLockedOut)
            {
                //锁定15分钟后解锁
                var time = (DateTime.UtcNow - user.UtcLastLockoutDate.Value);
                if (time.Minutes > setting.MinutesToUnlock)
                {
                    user.IsLockedOut = false;
                    user.FailedPasswordAttemptCount = 0;
                    Update(userName, user);
                }
                else
                {
                    violations.Add(new DataViolationItem("UserName", userName, "UserLockedOut".Localize()));
                }
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }
            #endregion

            var encodedPassword = _passwordProvider.EncryptPassword(password, user.PasswordSalt);
            var valid = encodedPassword == user.Password;
            if (valid == false && setting.FailedPasswordAttemptCount > 0)
            {
                user.FailedPasswordAttemptCount++;
                if (user.FailedPasswordAttemptCount >= setting.FailedPasswordAttemptCount)
                {
                    user.IsLockedOut = true;
                    user.UtcLastLockoutDate = DateTime.UtcNow;
                }
            }
            else
            {
                user.FailedPasswordAttemptCount = 0;
                user.UtcLastLoginDate = DateTime.UtcNow;
            }
            Update(user.UserName, user);
            return valid;
        }
        #endregion

        #region Forgotpassword
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
            string randomValue = UniqueIdGenerator.GetInstance().GetBase32UniqueId(16);

            user.ActivateCode = randomValue;

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

            var passwordToken = user.ActivateCode;
            if (string.IsNullOrEmpty(passwordToken) || !passwordToken.EqualsOrNullEmpty(passwordToken, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("The activate token is invalid.".Localize());
            }

            ChangePassword(userName, newPassword);
        }

        public virtual bool ValidatePasswordToken(string userName, string token)
        {
            var user = Get(userName);
            if (user == null)
            {
                return false;
            }
            var passwordToken = user.ActivateCode;
            if (string.IsNullOrEmpty(passwordToken) || !passwordToken.EqualsOrNullEmpty(token, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
