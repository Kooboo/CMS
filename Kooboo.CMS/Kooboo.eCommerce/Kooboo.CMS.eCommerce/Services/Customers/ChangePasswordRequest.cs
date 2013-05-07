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
    public class ChangePasswordRequest
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        public string OldPassword { get; set; }
    }
}
