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

namespace Kooboo.CMS.Sites.Member
{
    public class ActivateMemberModel
    {
        public string Member { get; set; }
        public string Code { get; set; }
        public string ActivateSuccessUrl { get; set; }
        public string ActivateFailedUrl { get; set; }
    }
}
