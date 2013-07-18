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
    [Dependency(typeof(IMembershipConnectProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<MembershipConnect>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipConnectProvider : IMembershipConnectProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipConnectProvider(MemberDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region All
        public IEnumerable<MembershipConnect> All(Membership membership)
        {
            return _dbContext.MembershipConnects.Where(it => it.Membership.Name == membership.Name);
        }

        public IEnumerable<MembershipConnect> All()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get
        public MembershipConnect Get(MembershipConnect dummy)
        {
            return _dbContext.MembershipConnects.Where(it => it.Membership.Name == dummy.Membership.Name && it.Name == dummy.Name)
             .FirstOrDefault();
        }
        #endregion

        #region Add
        public void Add(MembershipConnect item)
        {
            _dbContext.Memberships.Attach(item.Membership);
            _dbContext.MembershipConnects.Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public void Update(MembershipConnect @new, MembershipConnect old)
        {
            var entity = Get(@new);
            entity.DisplayName = @new.DisplayName;
            entity.AppId = @new.AppId;
            entity.AppSecret = @new.AppSecret;
            entity.Options = @new.Options;
            entity.MembershipGroups = @new.MembershipGroups;
            entity.Enabled = @new.Enabled;
            _dbContext.SaveChanges();
        }
        #endregion

        #region Remove
        public void Remove(MembershipConnect item)
        {
            _dbContext.MembershipConnects.Attach(item);
            _dbContext.MembershipConnects.Remove(item);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
