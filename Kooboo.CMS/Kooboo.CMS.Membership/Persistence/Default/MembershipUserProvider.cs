#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMembershipUserProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<MembershipUser>))]
    public class MembershipUserProvider : ListProviderBase<MembershipUser>, IMembershipUserProvider
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        MembershipPath _membershipPath = null;
        public MembershipUserProvider(MembershipPath membershipPath)
        {
            this._membershipPath = membershipPath;
        }
        #endregion

        #region  abstract implementation
        protected override string GetDataFile(Kooboo.CMS.Membership.Models.Membership membership)
        {
            var membershipPath = this._membershipPath.GetMembershipPath(membership);

            return Path.Combine(membershipPath, "MembershipUsers.config");
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion

        #region QueryUserByEmail
        public MembershipUser QueryUserByEmail(Kooboo.CMS.Membership.Models.Membership membership, string email)
        {
            return this.All(membership).Where(it => it.Email.EqualsOrNullEmpty(email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
        #endregion

        #region All
        public new IQueryable<MembershipUser> All(Kooboo.CMS.Membership.Models.Membership membership)
        {
            return base.All(membership).AsQueryable();
        }
        #endregion
    }
}
