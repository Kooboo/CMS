#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Membership.OAuthClients
{
    public abstract class OpenAuthClient : IAuthClient
    {
        public virtual MembershipConnect MembershipConnect { get; set; }

        public abstract IAuthenticationClient GetOpenAuthClient();

        public virtual void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            GetOpenAuthClient().RequestAuthentication(context, returnUrl);
        }

        public virtual AuthResult VerifyAuthentication(HttpContextBase context, Uri returnUrl)
        {
            //        string str;
            //if (!this.ValidateRequestAgainstXsrfAttack(out str))
            //{
            //    return new AuthenticationResult(false, this.authenticationProvider.ProviderName, null, null, null);
            //}

            var authenticationProvider = GetOpenAuthClient();
            AuthenticationResult result = null;

            OAuth2Client oauth2Client = authenticationProvider as OAuth2Client;
            if (oauth2Client != null)
            {
                returnUrl = returnUrl.AttachQueryStringParameter("__provider__", this.MembershipConnect.Name);//.AttachQueryStringParameter("__sid__", str);
                try
                {
                    result = oauth2Client.VerifyAuthentication(context, returnUrl);
                }
                catch (HttpException exception)
                {
                    result = new AuthenticationResult(exception.GetBaseException(), oauth2Client.ProviderName);
                }
            }
            result = authenticationProvider.VerifyAuthentication(context);

            return new AuthResult(result.IsSuccessful, result.Provider, result.ProviderUserId, result.UserName, result.ExtraData, result.Error);
        }

        public abstract string ProviderName
        {
            get;
        }

        public abstract bool RequiresAppId
        {
            get;
        }
    }
}
