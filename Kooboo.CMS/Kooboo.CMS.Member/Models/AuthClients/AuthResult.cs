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

namespace Kooboo.CMS.Member.Models.AuthClients
{
    public class AuthResult
    {
        public AuthResult(bool isSuccessful, string provider, string providerUserId, string userName, IDictionary<string, string> extraData, Exception error)
        {
            this.IsSuccessful = isSuccessful;
            this.Provider = provider;
            this.ProviderUserId = providerUserId;
            this.UserName = userName;
            this.ExtraData = extraData;
            this.Error = error;
        }

        public Exception Error { get; private set; }
        public IDictionary<string, string> ExtraData { get; private set; }
        public bool IsSuccessful { get; private set; }
        public string Provider { get; private set; }
        public string ProviderUserId { get; private set; }
        public string UserName { get; private set; }
    }
}
