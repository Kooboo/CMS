using Kooboo.CMS.Sites.Models;
using Kooboo.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ProxyPosition))]
    public class ProxyPosition_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [DisplayName("Site base domain or URL")]
        public string Host { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayName("Start page")]
        public string RequestPath { get; set; }

        [DisplayName("Non proxy")]
        [Description("Keep original base URL, without proxy redirect.")]
        public bool NoProxy { get; set; }
    }
}