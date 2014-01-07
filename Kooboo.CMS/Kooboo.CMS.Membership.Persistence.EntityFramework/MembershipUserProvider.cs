#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.EntityFramework
{
    [Dependency(typeof(IMembershipUserProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<MembershipUser>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipUserProvider : ProviderBase<MembershipUser>, IMembershipUserProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipUserProvider(MemberDBContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region QueryUserByEmail
        public MembershipUser QueryUserByEmail(Kooboo.CMS.Membership.Models.Membership membership, string email)
        {
            return _dbContext.Set<MembershipUser>().Where(it => it.Membership.Name == membership.Name && it.Email == email)
                 .FirstOrDefault();
        }
        #endregion

        #region Get
        public override MembershipUser Get(MembershipUser dummy)
        {
            return _dbContext.Set<MembershipUser>().Where(it => it.Membership.Name == dummy.Membership.Name && it.UserName == dummy.UserName)
                .FirstOrDefault();
        }
        #endregion

        #region Update
        public override void Update(MembershipUser @new, MembershipUser old)
        {
            _dbContext.SaveChanges();
        }
        #endregion

    }
}
