#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Content.Models;
using Kooboo.Common.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(MediaContentMetadata))]
    public class MediaContentMetadata_Metadata
    {

        public string Title
        {
            get;
            set;
        }


        [DisplayName("Alternative text")]
        public string AlternateText
        {
            get;
            set;
        }
        [UIHint("MultilineText")]
        public string Description
        {
            get;
            set;
        }
    }
}