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

namespace Kooboo.CMS.Sites.Models.Options
{
    public class CreateSiteOptions
    {
        public string RepositoryName { get; set; }
        public string MembershipName { get; set; }
        public string Culture { get; set; }
        public string TimeZoneId { get; set; }
        public string UserName { get; set; }
    }
}
