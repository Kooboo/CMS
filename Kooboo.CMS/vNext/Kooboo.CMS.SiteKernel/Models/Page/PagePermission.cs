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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public class PagePermission
    {
        public bool RequireMember { get; set; }
        public string[] AllowGroups { get; set; }
        public bool AuthorizeMenu { get; set; }
        public string UnauthorizedUrl { get; set; }

        public bool Authorize(IPrincipal principal)
        {
            var allow = true;
            if (this.RequireMember)
            {
                if (!principal.Identity.IsAuthenticated)
                {
                    allow = false;
                }
                else
                {
                    var groups = this.AllowGroups;
                    if (groups != null && groups.Length > 0 && !groups.Any<string>(new Func<string, bool>(principal.IsInRole)))
                    {
                        allow = false;
                    }
                }
            }
            return allow;
        }
    }
}
