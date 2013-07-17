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
    [Dependency(typeof(IMembershipGroupProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<MembershipGroup>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipGroupProvider : IMembershipGroupProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipGroupProvider(MemberDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region All
        public IEnumerable<Models.MembershipGroup> All(Models.Membership membership)
        {
            return _dbContext.MembershipGroups.Where(it => it.Membership.Name == membership.Name);
        }

        public IEnumerable<Models.MembershipGroup> All()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get
        public Models.MembershipGroup Get(Models.MembershipGroup dummy)
        {
            return _dbContext.MembershipGroups.Where(it => it.Membership.Name == dummy.Membership.Name && it.Name == dummy.Name).FirstOrDefault();
        }
        #endregion

        #region Add
        public void Add(Models.MembershipGroup item)
        {
            _dbContext.Memberships.Attach(item.Membership);
            _dbContext.MembershipGroups.Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public void Update(Models.MembershipGroup @new, Models.MembershipGroup old)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public void Remove(Models.MembershipGroup item)
        {
            _dbContext.MembershipGroups.Attach(item);
            _dbContext.MembershipGroups.Remove(item);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
