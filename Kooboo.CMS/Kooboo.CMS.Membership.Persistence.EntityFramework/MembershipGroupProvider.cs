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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.EntityFramework
{
    [Dependency(typeof(IMembershipGroupProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<MembershipGroup>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipGroupProvider : ProviderBase<MembershipGroup>, IMembershipGroupProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipGroupProvider(MemberDBContext dbContext)
            : base(dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region Get
        public override MembershipGroup Get(Models.MembershipGroup dummy)
        {
            return _dbContext.Set<MembershipGroup>().Where(it => it.Membership.Name == dummy.Membership.Name && it.Name == dummy.Name).FirstOrDefault();
        }
        #endregion

        #region Update
        public override void Update(Models.MembershipGroup @new, Models.MembershipGroup old)
        {
            throw new NotImplementedException();
        }
        #endregion
        
    }
}
