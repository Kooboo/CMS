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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Membership
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Required")]
        public virtual string UserName { get; set; }

        public virtual string RedirectUrl { get; set; }

        public virtual string EmailSubject { get; set; }

        public virtual string EmailBody { get; set; }

        public virtual string ResetPasswordUrl { get; set; }
    }
}
