#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet;
using Kooboo.Common.ObjectContainer;

namespace Kooboo.CMS.Membership.OAuthClients
{
    public static class OAuthWebSecurity
    {
        #region RequestAuthentication
        public static void RequestAuthentication(Kooboo.CMS.Membership.Models.Membership membership, string connectName, HttpContextBase httpContext, string returnUrl)
        {

            var client = GetAuthClient(membership, connectName);

            OpenAuthSecurityManager manager = new OpenAuthSecurityManager(httpContext, client.GetOpenAuthClient(), new MembershipOpenAuthDataProvider());
            manager.RequestAuthentication(returnUrl);
            //client.RequestAuthentication(httpContext, new Uri(returnUrl));
        }

        #endregion

        #region VerifyAuthentication
        public static AuthResult VerifyAuthentication(Kooboo.CMS.Membership.Models.Membership membership, HttpContextBase httpContext, string returnUrl)
        {
            return VerifyAuthenticationCore(membership, httpContext, returnUrl);
        }

        internal static AuthResult VerifyAuthenticationCore(Kooboo.CMS.Membership.Models.Membership membership, HttpContextBase context, string returnUrl)
        {
            IAuthClient client;
            string providerName = OpenAuthSecurityManager.GetProviderName(context);
            if (string.IsNullOrEmpty(providerName))
            {
                return AuthResult.Failed;
            }
            client = GetAuthClient(membership, providerName);
            if (client == null)
            {
                throw new InvalidOperationException("Invalid membership connect.");
            }
            OpenAuthSecurityManager manager = new OpenAuthSecurityManager(context, client.GetOpenAuthClient(), new MembershipOpenAuthDataProvider());
            var result = manager.VerifyAuthentication(returnUrl);
            return new AuthResult(result);
        }
        #endregion

        #region GetAuthClient
        private static IAuthClient GetAuthClient(Kooboo.CMS.Membership.Models.Membership membership, string connectName)
        {
            var membershipConnect = new MembershipConnect() { Membership = membership, Name = connectName }.AsActual();

            if (membershipConnect == null)
            {
                throw new ArgumentException("Invalid membership connect.");
            }

            var authClient = EngineContext.Current.Resolve<IAuthClient>(membershipConnect.Name.ToLower());
            authClient.MembershipConnect = membershipConnect;

            return authClient;
        }
        #endregion
    }
}
