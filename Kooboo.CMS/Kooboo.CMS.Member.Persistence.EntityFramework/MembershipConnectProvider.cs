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
    public class MembershipConnectProvider : ProviderBase<MembershipConnect>, IMembershipConnectProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipConnectProvider(MemberDBContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region Get
        public override MembershipConnect Get(MembershipConnect dummy)
        {
            return _dbContext.Set<MembershipConnect>().Where(it => it.Membership.Name == dummy.Membership.Name && it.Name == dummy.Name)
             .FirstOrDefault();
        }
        #endregion

        #region Update
        public override void Update(MembershipConnect @new, MembershipConnect old)
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

    }
}
