#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(HtmlMeta))]
    public class HtmlMeta_Metadata
    {
        [Display(Name = "Html title")]
        [Description("The title tag and value that appear in the page HTML header and also used by browsers as the page title. <br />This value can be accessed via API: @Html.FrontHtml().HtmlTitle()")]
        public string HtmlTitle { get; set; }

        [Description("Used in SEO, see: http://googlewebmastercentral.blogspot.com/2009/02/specify-your-canonical.html")]
        public string Canonical { get; set; }

        public string Author { get; set; }

        public string Keywords { get; set; }
        [UIHintAttribute("MultilineText")]
        public string Description { get; set; }

        [DisplayName("Custom meta fields")]
        [UIHintAttribute("Dictionary")]
        [Description("Custom fields for HTML meta values")]
        public IDictionary<string, string> Customs
        {
            get;
            set;
        }
    }
}
