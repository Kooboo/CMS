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
        public string Host { get; set; }

        [Required(ErrorMessage = "Required")]
        public string RequestPath { get; set; }

        [DisplayName("No proxy")]
        [Description("No proxy for the content.")]
        public bool NoProxy { get; set; }
    }
}