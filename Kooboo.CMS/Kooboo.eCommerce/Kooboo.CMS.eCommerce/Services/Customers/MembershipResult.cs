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
    public class MembershipResult
    {
        public List<string> Errors { get; set; }

        public MembershipResult()
        {
            this.Errors = new List<string>();
        }

        public bool Success
        {
            get { return this.Errors.Count == 0; }
        }

    }
}
