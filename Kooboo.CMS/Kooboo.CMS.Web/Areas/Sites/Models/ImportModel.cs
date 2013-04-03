using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using System.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ImportModel
    {
        public static ImportModel Default = new ImportModel()
            {
                Action = "Import"
            };

        public string Action { get; set; }
        //public string Controller { get; set; }
        //public RouteValueDictionary RouteValues { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("File")]
        public string File { get; set; }

        [Description("Will update the exists items.")]
        public bool Override { get; set; }
    }
}