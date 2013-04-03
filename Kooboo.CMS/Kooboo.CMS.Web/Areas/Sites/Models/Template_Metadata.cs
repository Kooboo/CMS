using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class Template_Metadata
    {
        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }
    }
}