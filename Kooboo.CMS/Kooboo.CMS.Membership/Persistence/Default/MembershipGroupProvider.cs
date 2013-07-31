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
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMembershipGroupProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<MembershipGroup>))]
    public class MembershipGroupProvider : ListProviderBase<MembershipGroup>, IMembershipGroupProvider
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        MembershipPath _membershipPath = null;
        public MembershipGroupProvider(MembershipPath membershipPath)
        {
            this._membershipPath = membershipPath;
        }
        #endregion

        #region  abstract implementation
        protected override string GetDataFile(Kooboo.CMS.Membership.Models.Membership membership)
        {
            var membershipPath = this._membershipPath.GetMembershipPath(membership);

            return Path.Combine(membershipPath, "MembershipGroups.config");
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion
    }
}
