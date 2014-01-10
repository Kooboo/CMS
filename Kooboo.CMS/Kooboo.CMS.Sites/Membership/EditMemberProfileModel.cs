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

namespace Kooboo.CMS.Sites.Membership
{
    public class EditMemberProfileModel
    {
        public virtual string Email { get; set; }
        public virtual string PasswordQuestion { get; set; }

        public virtual string PasswordAnswer { get; set; }

        public virtual string Culture { get; set; }

        public virtual string TimeZoneId { get; set; }

        public virtual Dictionary<string, string> Profiles { get; set; }

        public virtual string RedirectUrl { get; set; }
    }
}
