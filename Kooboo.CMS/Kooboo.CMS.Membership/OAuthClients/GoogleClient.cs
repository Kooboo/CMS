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
using System.Text;

namespace Kooboo.CMS.Membership.OAuthClients
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IAuthClient), Key = "google")]
    public class GoogleClient : OpenAuthClient, IAuthClient
    {
        public override DotNetOpenAuth.AspNet.IAuthenticationClient GetOpenAuthClient()
        {
            System.Diagnostics.Contracts.Contract.Requires(MembershipConnect != null);

            return new DotNetOpenAuth.AspNet.Clients.GoogleOpenIdClient();
        }

        public override string ProviderName
        {
            get
            {
                return "Google";
            }
        }

        public override bool RequiresAppId
        {
            get
            {
                return false;
            }
        }
    }
}
