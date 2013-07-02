#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Member.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Kooboo.CMS.Member.Services
{
    public class MembershipUserManager : ManagerBase<MembershipUser>
    {
        #region .ctor
        IMembershipUserProvider _provider;
        MembershipPasswordProvider _passwordProvider;
        public MembershipUserManager(IMembershipUserProvider provider, MembershipPasswordProvider passwordProvider)
            : base(provider)
        {
            _provider = provider;
            _passwordProvider = passwordProvider;
        }
        #endregion


        #region All
        public IQueryable<MembershipUser> All(Membership membership, string filterName)
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
        public virtual MembershipUser Create(Membership membership, string userName, string email, string password, bool isApproved, string culture, string timeZoneId, string passwordQuestion = null,
            string passwordAnswer = null, string[] membershipGroups = null, Dictionary<string, string> profiles = null, string comment = null)
        {
            membership = membership.AsActual();

            MembershipUser membershipUser = new MembershipUser() { Membership = membership };
            membershipUser.UserName = userName;

            if (membershipUser.AsActual() != null)
            {
                throw new ArgumentException("DuplicateUserName");
            }
            if (_provider.QueryUserByEmail(membership, email) != null)
            {
                throw new ArgumentException("DuplicateEmail");
            }

            membershipUser.Email = email;
            membershipUser.Culture = culture;
            membershipUser.TimeZoneId = timeZoneId;
            membershipUser.PasswordQuestion = passwordQuestion;
            membershipUser.PasswordAnswer = passwordAnswer;
            membershipUser.Comment = comment;
            membershipUser.IsApproved = isApproved;
            membershipUser.UtcCreationDate = DateTime.UtcNow;
            membershipUser.Profiles = profiles;
            membershipUser.MembershipGroups = membershipGroups;

            SetPassword(membership, membershipUser, password);


            _provider.Add(membershipUser);

            return _provider.Get(membershipUser);
        }

        private void SetPassword(Membership membership, MembershipUser membershipUser, string password)
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

        #region Update
        public override void Update(MembershipUser @new, MembershipUser old)
        {
            throw new NotSupportedException("Using 'Edit' to update a membership user instead of 'Update'.");
        }
        #endregion

        #region Edit
        public virtual void Edit(Membership membership, string userName, bool isApproved, bool isLockedOut, string culture, string timeZoneId, string passwordQuestion = null,
            string passwordAnswer = null, string[] membershipGroups = null, Dictionary<string, string> profiles = null, string comment = null)
        {
            membership = membership.AsActual();

            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            if (membershipUser == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }

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
        public virtual void ChangePassword(Membership membership, string userName, string newPassword)
        {
            membership = membership.AsActual();
            MembershipUser membershipUser = new MembershipUser() { Membership = membership, UserName = userName }.AsActual();

            if (membershipUser == null)
            {
                throw new ArgumentException("The member doest not exists.");
            }

            SetPassword(membership, membershipUser, newPassword);

            _provider.Update(membershipUser, membershipUser);
        }
        #endregion

    }
}
