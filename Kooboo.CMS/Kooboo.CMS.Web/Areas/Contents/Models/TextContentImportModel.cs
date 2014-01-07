using Kooboo.CMS.Content.Formatter;
using Kooboo.CMS.Content.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class TextContentImportModel
    {
        [DisplayName("Data format")]
        public string Formatter { get; set; }

        [Required(ErrorMessage = "Required")]
        //[RegularExpression(".+\\.(zip)$", ErrorMessage = "Required a zip file.")]
        [UIHint("File")]
        [Description("")]
        public virtual HttpPostedFileWrapper File { get; set; }

        [Required]
        public string FolderName { get; set; }

        public ITextContentFormater TextContentExporter
        {
            get
            {
                return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<ITextContentFormater>(this.Formatter.ToLower());
            }
        }
    }
}