#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Services
{
    public class MembershipManager : ManagerBase<Kooboo.CMS.Membership.Models.Membership>
    {
        #region .ctor
        IMembershipProvider _membershipProvider;
        public MembershipManager(IMembershipProvider membershipProvider)
            : base(membershipProvider)
        {
            this._membershipProvider = membershipProvider;
        }
        #endregion

        #region All
        public virtual IEnumerable<Kooboo.CMS.Membership.Models.Membership> All(string filterName)
        {
            var list = _membershipProvider.All();
            if (!string.IsNullOrEmpty(filterName))
            {
                list = list.Where(it => it.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }
            return list;
        }
        #endregion
    }
}
