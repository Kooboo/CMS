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

namespace Kooboo.CMS.eCommerce.Services.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the reset password token.
        /// </summary>
        public string ResetPasswordToken { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword { get; set; }
    }
}
