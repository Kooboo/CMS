#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Kooboo.CMS.Sites.Membership
{
    public interface IMembershipAuthentication
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
        void SignOut();
        IPrincipal GetMember();
        MembershipUser GetMembershipUser();
        bool IsAuthenticated();
    }
}
