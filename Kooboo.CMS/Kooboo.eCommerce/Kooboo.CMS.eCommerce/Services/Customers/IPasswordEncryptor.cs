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
    public interface IPasswordEncryptor
    {
        /// <summary>
        /// Generates the salt.
        /// </summary>
        /// <returns></returns>
        string GenerateSalt();
        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="salt">The salt.</param>
        /// <returns></returns>
        string EncryptPassword(string pass, string salt);
    }
}
