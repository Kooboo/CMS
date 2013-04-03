
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class ImageScaleModel
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool PreserveAspectRatio { get; set; }
        public int? Quality { get; set; }
    }
}