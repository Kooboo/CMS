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
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.EntityFramework
{
    [Dependency(typeof(IMembershipUserProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<MembershipUser>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipUserProvider : IMembershipUserProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipUserProvider(MemberDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion
        #region All
        public IEnumerable<MembershipUser> All()
        {
            throw new NotImplementedException();
        }

        public IQueryable<MembershipUser> All(Membership membership)
        {
            return _dbContext.MembershipUsers.Where(it => it.Membership.Name == membership.Name);
        }
        #endregion

        #region QueryUserByEmail
        public MembershipUser QueryUserByEmail(Membership membership, string email)
        {
            return _dbContext.MembershipUsers.Where(it => it.Membership.Name == membership.Name && it.Email == email)
                 .FirstOrDefault();
        }
        #endregion

        #region IMemberElementProvider
        IEnumerable<MembershipUser> IMemberElementProvider<MembershipUser>.All(Membership membership)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get
        public MembershipUser Get(MembershipUser dummy)
        {
            return _dbContext.MembershipUsers.Where(it => it.Membership.Name == dummy.Membership.Name && it.UserName == dummy.UserName)
                .FirstOrDefault();
        }
        #endregion

        #region Add
        public void Add(MembershipUser item)
        {
            _dbContext.Memberships.Attach(item.Membership);
            _dbContext.MembershipUsers.Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public void Update(MembershipUser @new, MembershipUser old)
        {
            _dbContext.SaveChanges();
        }
        #endregion

        #region Remove
        public void Remove(MembershipUser item)
        {
            _dbContext.MembershipUsers.Attach(item);
            _dbContext.MembershipUsers.Remove(item);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
