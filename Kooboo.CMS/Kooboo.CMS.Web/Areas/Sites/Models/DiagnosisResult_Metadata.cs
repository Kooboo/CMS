using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Management;
using Kooboo.CMS.Sites.Services;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class DiagnosisResult_Metadata
    {
        public WebApplicationInformation WebApplicationInformation { get; set; }
        [Display(Name = "Content provider")]
        public string ContentProvider { get; set; }
        public DiagnosisItem[] DiagnosisItems { get; set; }
    }

    public class WebApplicationInformation_Metadata
    {
        [Display(Name = "Application virtual path")]
        public string ApplicationVirtualPath { get; set; }
        [Display(Name = "Machine name")]
        public string MachineName { get; set; }
        [Display(Name = "Trust level")]
        public string TrustLevel { get; set; }
    }
}
