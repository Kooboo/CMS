#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Security))]
    public class Security_Metadata
    {
        [Description("The encrypt key for SecurityHelper.Encrypt/Decrypt.")]
        [DisplayName("Encrypt key")]
        [StringLength(8, MinimumLength = 8)]
        public string EncryptKey { get; set; }

        [Description(" Turn on/off the content submission API such as ContentService and SendEmail API. The 'ContentService' was obsolete in 4.1 verion, it is recommended to trun off for security reason.")]
        [DisplayName("Turn on submission api")]
        public bool TurnOnSubmissionAPI { get; set; }

    }
}
