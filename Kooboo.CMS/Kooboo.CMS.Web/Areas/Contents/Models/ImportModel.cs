using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class ImportModel
    {
        public static ImportModel Default = new ImportModel();

        [Required(ErrorMessage = "Required")]
        [UIHint("File")]
        public string File { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool Override { get; set; }
    }
}