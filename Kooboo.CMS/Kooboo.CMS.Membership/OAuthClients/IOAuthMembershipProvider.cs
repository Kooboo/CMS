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
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Membership.OAuthClients
{
    public interface IOAuthMembershipProvider
    {
        string GetUserName(AuthResult authResult, MembershipConnect membershipConnect);
        string GetEmail(AuthResult authResult, MembershipConnect membershipConnect);
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IOAuthMembershipProvider))]
    public class OAuthMembershipProvider : IOAuthMembershipProvider
    {
        public string GetUserName(AuthResult authResult, MembershipConnect membershipConnect)
        {
            string userName = authResult.UserName;
            if (!IsEmail(userName) && !string.IsNullOrEmpty(membershipConnect.UsernameFormat))
            {
                userName = string.Format(membershipConnect.UsernameFormat, userName);
            }
            return userName;
        }
        private static bool IsEmail(string str)
        {
            return Regex.IsMatch(str, RegexPatterns.EmailAddress);
        }
        public string GetEmail(AuthResult authResult, MembershipConnect membershipConnect)
        {
            string email = "";

            if (IsEmail(authResult.UserName))
            {
                email = authResult.UserName;
            }
            else
            {
                email = "";
                if (authResult.ExtraData != null)
                {
                    var extraData = new Dictionary<string, string>(authResult.ExtraData, StringComparer.OrdinalIgnoreCase);
                    if (extraData.ContainsKey("email"))
                    {
                        email = extraData["email"];
                    }
                }
            }
            return email;
        }
    }
}
