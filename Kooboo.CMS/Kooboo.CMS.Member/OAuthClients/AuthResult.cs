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
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Member.OAuthClients
{
    public class AuthResult
    {
        #region .ctor
        static AuthResult()
        {
            Failed = new AuthResult(false);

        }
        public AuthResult(bool isSuccessful)
            : this(isSuccessful, null, null, null, null, null)
        {
        }

        public AuthResult(bool isSuccessful, string provider, string providerUserId, string userName, IDictionary<string, string> extraData, Exception error)
        {
            this.IsSuccessful = isSuccessful;
            this.Provider = provider;
            this.ProviderUserId = providerUserId;
            this.UserName = userName;
            this.ExtraData = extraData;
            this.Error = error;
        }

        public AuthResult(AuthenticationResult result)
        {
            this.IsSuccessful = result.IsSuccessful;
            this.Provider = result.Provider;
            this.ProviderUserId = result.ProviderUserId;
            this.UserName = result.UserName;
            this.ExtraData = result.ExtraData;
            this.Error = result.Error;
        }
        #endregion

        #region Properties

        public Exception Error { get; private set; }
        public IDictionary<string, string> ExtraData { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Provider { get; private set; }
        public string ProviderUserId { get; private set; }
        public string UserName { get; private set; }

        public static AuthResult Failed { get; private set; }
        #endregion        
    }
}
