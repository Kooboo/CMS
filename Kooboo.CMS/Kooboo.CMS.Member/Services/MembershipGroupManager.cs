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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Services
{
    public class MembershipGroupManager : ManagerBase<MembershipGroup>
    {
        #region .ctor
        IMembershipGroupProvider _provider;
        public MembershipGroupManager(IMembershipGroupProvider provider)
            : base(provider)
        {
            this._provider = provider;
        }
        #endregion

        #region All
        public virtual IEnumerable<MembershipGroup> All(Membership membership, string filterName)
        {
            var list = _provider.All(membership);
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }
            return list;
        }
        #endregion
    }
}
