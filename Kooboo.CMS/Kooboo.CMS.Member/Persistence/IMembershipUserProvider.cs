#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence
{
    public interface IMembershipUserProvider : IMemberElementProvider<MembershipUser>
    {
        IQueryable<MembershipUser> All(Membership membership);
        MembershipUser QueryUserByEmail(Membership membership, string email);
    }
}
