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
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Template))]
    public class Template_Metadata
    {
        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }
    }
}