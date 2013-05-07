using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public enum ExportType
    {
        [Description("Site with database")]
        SiteWithDatabase = 0,
        [Description("Only site")]
        OnlySite = 1,
        [Description("Only database")]
        OnlitDatabase = 2
    }
    public class ExportSiteModel
    {
        public string SiteName { get; set; }
        [UIHint("DropDownList")]
        [EnumDataType(typeof(ExportType))]
        [DisplayName("Export type")]
        public ExportType ExportType { get; set; }
        [DisplayName("Include sub sites")]
        public bool IncludeSubSites { get; set; }
    }
}