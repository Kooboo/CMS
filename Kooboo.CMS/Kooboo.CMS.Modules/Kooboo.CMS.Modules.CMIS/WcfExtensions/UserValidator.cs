#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.ServiceModel;
using System.Text;

using Kooboo.Globalization;
namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class UserValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                throw new SecurityTokenException("Username and password required".Localize());

            var userManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<UserManager>();
            if (!userManager.ValidateUser(userName, password))
            {
                throw new FaultException(string.Format("Username and/or password are incorrect.".Localize(), userName));
            }
            var user = userManager.Get(userName);
            if (!user.IsAdministrator)
            {
                throw new FaultException("The user unable to access cmis service.".Localize());
            }
        }
    }
}
