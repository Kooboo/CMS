#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kooboo.CMS.Common.DataViolation;
using Kooboo.CMS.Membership.OAuthClients;


namespace Kooboo.CMS.Membership.Services
{
    public class MembershipUserManager : ManagerBase<MembershipUser>
    {
        #region .ctor
        IOAuthMembershipProvider _oauthMembershipProvider;
        IMembershipUserProvider _provider;
        MembershipPasswordProvider _passwordProvider;
        public MembershipUserManager(IMembershipUserProvider provider, MembershipPasswordProvider passwordProvider, IOAuthMembershipProvider oauthMembershipProvider)
            : base(provider)
        {
            _provider = provider;
            _passwordProvider = passwordProvider;
            _oauthMembershipProvider = oauthMembershipProvider;
        }
        #endregion


        #region All
        public IQueryable<MembershipUser> All(Kooboo.CMS.Membership.Models.Membership membership, string filterName)
        {
            var all = _provider.All(membership);
            if (!string.IsNullOrEmpty(filterName))
            {
                all = all.Where(it => it.UserName.Contains(filterName));
            }
            return all;
        }
        #endregion

        #region Add
        public override void Add(MembershipUser obj)
        {
            throw new NotSupportedException("Using 'Create' to create a membership user instead of 'Add'.");
        }
        #endregion

        #region Create
        public virtual MembershipUser Create(Kooboo.CMS.Membership.Models.Membership membership, string userName, string email, string password, bool isApproved, string culture, string timeZoneId, string passwordQuestion = null,
            string passwordAnswer = null, string[] membershipGroups = null, Dictionary<string, string> profiles = null, string comment = null)
        {
            membership = membership.AsActual();

            MembershipUser membershipUser = new MembershipUser() { Membership = membership };
            membershipUser.UserName = userName;
            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (membershipUser.AsActual() != null)
            {
                violations.Add(new DataViolationItem("UserName", userName, "DuplicateUserName"));
            }
            if (_provider.QueryUserByEmail(membership, email) != null)
            {
                violations.Add(new DataViolationItem("Email", email, "DuplicateEmail"));
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }

            if (!string.IsNullOrEmpty(email))
            {
                membershipUser.Email = email;
            }
            membershipUser.Culture = culture;
            membershipUser.TimeZoneId = timeZoneId;
            membershipUser.PasswordQuestion = passwordQuestion;
            membershipUser.PasswordAnswer = passwordAnswer;
            membershipUser.Comment = comment;
            membershipUser.IsApproved = isApproved;
            if (!membershipUser.IsApproved)
            {
                string activateCode = UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
                membershipUser.ActivateCode = activateCode;
            }
            membershipUser.UtcCreationDate = DateTime.UtcNow;
            membershipUser.Profiles = profiles;
            membershipUser.MembershipGroups = membershipGroups;

            SetPassword(membership, membershipUser, password);


            _provider.Add(membershipUser);

            return _provider.Get(membershipUser);
        }

        private void SetPassword(Kooboo.CMS.Membership.Models.Membership membership, MembershipUser membershipUser, string password)
        {
            var salt = _passwordProvider.GenerateSalt();
            var encodedPassword = _passwordProvider.EncodePassword(membership, password, salt);
            if (encodedPassword == password)
            {
                salt = null;
            }
            membershipUser.Password = encodedPassword;
            membershipUser.PasswordSalt = salt;
        }
        #endregion

        #region CreateOrUpdateOAuthMember
        public virtual MembershipUser CreateOrUpdateOAuthMember(Kooboo.CMS.Membership.Models.Membership membership, MembershipConnect membershipConnect, AuthResult authResult, Dictionary<string, string> profiles)
        {
            membership = membership.AsActual();
            membershipConnect = membershipConnect.AsActual();

            var userName = _oauthMembershipProvider.GetUserName(authResult, membershipConnect);
            var email = _oauthMembershipProvider.GetEmail(authResult, membershipConnect);
            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();
            Dictionary<string, string> extraData = null;
            if (authResult.ExtraData != null)
            {
                extraData = new Dictionary<string, string>(authResult.ExtraData);
            }
            if (membershipUser != null)
            {
                if (membershipUser.ProviderUserId == authResult.ProviderUserId)
                {
                    membershipUser.UtcLastLoginDate = DateTime.UtcNow;
                    membershipUser.ProviderExtraData = extraData;
                    membershipUser.Profiles = profiles;
                    if (profiles != null)
                    {
                        if (membershipUser.Profiles == null)
                            membershipUser.Profiles = new Dictionary<string, string>();
                        foreach (var item in profiles)
                        {
                            membershipUser.Profiles[item.Key] = item.Value;
                        }
                    }
                    membershipUser.MembershipGroups = membershipConnect.MembershipGroups;
                    _provider.Update(membershipUser, membershipUser);
                }
            }
            else
            {
                membershipUser = new MembershipUser() { Membership = membership, UserName = userName };
                membershipUser.Email = email;
                membershipUser.IsApproved = true;
                membershipUser.UtcCreationDate = DateTime.UtcNow;
                membershipUser.UtcLastLoginDate = DateTime.UtcNow;
                membershipUser.Profiles = profiles;
                membershipUser.MembershipGroups = membershipConnect.MembershipGroups;
                membershipUser.ProviderType = authResult.Provider;
                membershipUser.ProviderUserId = authResult.ProviderUserId;
                membershipUser.ProviderExtraData = extraData;
                _provider.Add(membershipUser);
            }

            return _provider.Get(membershipUser);
        }

        #endregion

        #region Update
        public override void Update(MembershipUser @new, MembershipUser old)
        {
            _provider.Update(@new, old);
        }
        #endregion

        #region Edit
        public virtual void Edit(Kooboo.CMS.Membership.Models.Membership membership, string userName, string email, bool isApproved, bool isLockedOut, string culture, string timeZoneId, string passwordQuestion = null,
            string passwordAnswer = null, string[] membershipGroups = null, Dictionary<string, string> profiles = null, string comment = null)
        {
            membership = membership.AsActual();

            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            if (membershipUser == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }
            membershipUser.Email = email;
            membershipUser.Culture = culture;
            membershipUser.TimeZoneId = timeZoneId;
            membershipUser.PasswordQuestion = passwordQuestion;
            membershipUser.PasswordAnswer = passwordAnswer;
            membershipUser.Comment = comment;
            membershipUser.IsApproved = isApproved;
            membershipUser.IsLockedOut = isLockedOut;
            membershipUser.Profiles = profiles;
            membershipUser.MembershipGroups = membershipGroups;

            _provider.Update(membershipUser, membershipUser);

        }
        #endregion

        #region ChangePassword
        public virtual void ChangePassword(Kooboo.CMS.Membership.Models.Membership membership, string userName, string newPassword)
        {
            membership = membership.AsActual();
            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            if (membershipUser == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }

            SetPassword(membership, membershipUser, newPassword);
            membershipUser.UtcLastPasswordChangedDate = DateTime.UtcNow;

            _provider.Update(membershipUser, membershipUser);
        }

        #endregion

        #region Validate
        public virtual bool Validate(Kooboo.CMS.Membership.Models.Membership membership, string userName, string password)
        {
            var membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();
            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (membershipUser == null)
            {
                return false;
            }
            if (!membershipUser.IsApproved)
            {
                violations.Add(new DataViolationItem("UserName", userName, "The member still not actived."));
            }
            if (membershipUser.IsLockedOut)
            {
                violations.Add(new DataViolationItem("UserName", userName, "The member was locked out."));
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }
            var encodedPassword = _passwordProvider.EncodePassword(membership, password, membershipUser.PasswordSalt);
            var valid = encodedPassword == membershipUser.Password;
            if (valid == false && membership.MaxInvalidPasswordAttempts > 0)
            {
                membershipUser.InvalidPasswordAttempts++;
                if (membershipUser.InvalidPasswordAttempts >= membership.MaxInvalidPasswordAttempts)
                {
                    membershipUser.IsLockedOut = true;
                    membershipUser.UtcLastLockoutDate = DateTime.UtcNow;
                }
            }
            else
            {
                membershipUser.UtcLastLoginDate = DateTime.UtcNow;
            }
            Update(membershipUser, membershipUser);
            return valid;
        }
        #endregion

        #region Active
        public virtual bool Activate(Kooboo.CMS.Membership.Models.Membership membership, string userName, string activateCode)
        {
            var membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (membershipUser == null)
            {
                violations.Add(new DataViolationItem("UserName", userName, "The member does not exists."));
            }
            if (string.IsNullOrEmpty(activateCode))
            {
                violations.Add(new DataViolationItem("ActivateCode", userName, "Activate code is null."));
            }
            if (membershipUser.IsApproved)
            {
                return true;
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }

            var isApproved = false;

            if (!string.IsNullOrEmpty(membershipUser.ActivateCode))
            {
                isApproved = membershipUser.ActivateCode == activateCode;
            }
            membershipUser.ActivateCode = null;
            if (isApproved == true)
            {
                membershipUser.IsApproved = true;
            }
            Update(membershipUser, membershipUser);
            return isApproved;
        }
        #endregion

        #region EditMemberProfile
        public virtual void EditMemberProfile(Kooboo.CMS.Membership.Models.Membership membership, string userName, string email, string culture, string timeZoneId, string passwordQuestion = null,
         string passwordAnswer = null, Dictionary<string, string> profiles = null)
        {
            membership = membership.AsActual();

            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            if (membershipUser == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }
            if (!string.IsNullOrEmpty(email))
            {
                membershipUser.Email = email;
            }
            membershipUser.Culture = culture;
            membershipUser.TimeZoneId = timeZoneId;
            membershipUser.PasswordQuestion = passwordQuestion;
            membershipUser.PasswordAnswer = passwordAnswer;
            membershipUser.Profiles = profiles;

            _provider.Update(membershipUser, membershipUser);

        }
        #endregion

        #region ForgotPassword
        public virtual MembershipUser ForgotPassword(Kooboo.CMS.Membership.Models.Membership membership, string userName)
        {
            var membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();
            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (membershipUser == null)
            {
                violations.Add(new DataViolationItem("UserName", userName, "The member does not exists."));
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }

            var activateCode = UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
            membershipUser.ActivateCode = activateCode;

            _provider.Update(membershipUser, membershipUser);

            return membershipUser;
        }
        #endregion

        #region ResetPassword
        public virtual bool ResetPassword(Kooboo.CMS.Membership.Models.Membership membership, string userName, string activateCode, string password)
        {
            var membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            List<DataViolationItem> violations = new List<DataViolationItem>();
            if (membershipUser == null)
            {
                violations.Add(new DataViolationItem("UserName", userName, "The member does not exists."));
            }
            if (string.IsNullOrEmpty(activateCode))
            {
                violations.Add(new DataViolationItem("ActivateCode", userName, "Activate code is null."));
            }

            var valid = !string.IsNullOrEmpty(membershipUser.ActivateCode) && membershipUser.ActivateCode == activateCode;
            if (valid)
            {
                SetPassword(membership, membershipUser, password);
                membershipUser.IsLockedOut = false;
                membershipUser.IsApproved = true;
            }
            else
            {
                violations.Add(new DataViolationItem("ActivateCode", userName, "Activate code is invalid."));
            }
            if (violations.Count > 0)
            {
                throw new DataViolationException(violations);
            }


            membershipUser.ActivateCode = null;
            membershipUser.UtcLastPasswordChangedDate = DateTime.UtcNow;

            Update(membershipUser, membershipUser);

            return valid;
        }
        #endregion
    }
}
