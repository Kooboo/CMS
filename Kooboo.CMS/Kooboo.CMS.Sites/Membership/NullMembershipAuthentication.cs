#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Kooboo.CMS.Sites.Membership
{
    public class NullMembershipAuthentication : IMembershipAuthentication
    {
        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {

        }

        public void SignOut()
        {

        }

        public System.Security.Principal.IPrincipal GetMember()
        {
            return new GenericPrincipal(new GenericIdentity(""), new string[0]);
        }

        public bool IsAuthenticated()
        {
            return GetMember().Identity.IsAuthenticated;
        }


        public CMS.Membership.Models.MembershipUser GetMembershipUser()
        {
            return null;
        }
    }
}
