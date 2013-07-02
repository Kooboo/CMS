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
using System.Web;

namespace Kooboo.CMS.Member.Models.AuthClients
{
    public interface IAuthClient
    {
        /// <summary>
        ///  The name Of OAuth authentication providers like Facebook,
        //     Twitter, LinkedIn, Windows Live and OpenID authentication providers like
        //     Google and Yahoo.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        string ProviderName { get; }

        /// <summary>
        /// Requires to set the AppId and appSecret
        /// </summary>
        bool RequiresAppId { get; }

        MembershipConnect MembershipConnect { get; set; }

        void RequestAuthentication(HttpContextBase context, Uri returnUrl);

        AuthResult VerifyAuthentication(HttpContextBase context);
    }
}
