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
    public class RegistrationRequest
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsApproved { get; set; }
    }
}
