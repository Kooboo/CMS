#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using DotNetOpenAuth.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Member.Models.AuthClients
{
    public abstract class OpenAuthClient : IAuthClient
    {
        public virtual MembershipConnect MembershipConnect { get; set; }

        protected abstract IAuthenticationClient GetOpenAuthClient();

        public virtual void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            GetOpenAuthClient().RequestAuthentication(context, returnUrl);
        }

        public virtual AuthResult VerifyAuthentication(HttpContextBase context)
        {
            var result = GetOpenAuthClient().VerifyAuthentication(context);

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
