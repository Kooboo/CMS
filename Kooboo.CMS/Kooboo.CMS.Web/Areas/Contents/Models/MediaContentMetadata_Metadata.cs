
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class MediaContentMetadata_Metadata
    {

        public string Title
        {
            get;
            set;
        }

        [DisplayName("Alternate text")]
        [Description("The alt text for img tag.")]
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