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
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Customers
{
    public class Membership
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public virtual string Password { get; set; }

        /// <summary>
        /// Gets or sets the password salt.
        /// </summary>
        /// <value>
        /// The password salt.
        /// </value>
        public virtual string PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the reset password token.
        /// </summary>
        public virtual string ResetPasswordToken { get; set; }

        public virtual bool IsApproved { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is lockout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is lockout; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLockedOut { get; set; }

        /// <summary>
        /// Gets or sets the failed password attempt count.
        /// </summary>
        /// <value>
        /// The failed password attempt count.
        /// </value>
        public virtual int FailedPasswordAttemptCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last IP address
        /// </summary>
        public virtual string LastIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        /// <value>
        /// The UTC creation date.
        /// </value>
        public virtual DateTime UtcCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC last login date.
        /// </summary>
        /// <value>
        /// The UTC last login date.
        /// </value>
        public virtual DateTime? UtcLastLoginDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC last activity date.
        /// </summary>
        /// <value>
        /// The UTC last activity date.
        /// </value>
        public virtual DateTime UtcLastActivityDate { get; set; }
    }
}
