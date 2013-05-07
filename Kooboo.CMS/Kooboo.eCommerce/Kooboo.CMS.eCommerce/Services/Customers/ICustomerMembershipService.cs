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

namespace Kooboo.CMS.eCommerce.Services.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomerMembershipService
    {
        /// <summary>
        /// Validates the specified username or email.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        MembershipResult ValidateByUserName(string username, string password);

        /// <summary>
        /// Validates the name of the by user.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        MembershipResult ValidateByEmail(string email, string password);

        /// <summary>
        /// Registers the specified registration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        MembershipResult Register(RegistrationRequest request);

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        MembershipResult ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// Generates the reset password token.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        GenerateResetPasswordTokenResult GenerateResetPasswordToken(string username);

        /// <summary>
        /// Resets the passowrd result.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        MembershipResult ResetPassowrd(ResetPasswordRequest request);
    }
}
