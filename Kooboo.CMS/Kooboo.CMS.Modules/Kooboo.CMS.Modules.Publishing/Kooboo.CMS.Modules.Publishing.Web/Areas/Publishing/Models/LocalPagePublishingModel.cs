using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    public class LocalPagePublishingModel
    {
        [DataSource(typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources.PagesDataSource))]
        [UIHint("Page_DropDownList")]
        [DisplayName("Pages")]
        [Required(ErrorMessage="Required")]
        public string[] Pages { get; set; }       

        [DisplayName("Schedule")]
        public bool Schedule { get; set; }

        [DisplayName("Publish time")]
        public DateTime? UtcTimeToPublish { get; set; }

        [DisplayName("Unpublish time")]
        public DateTime? UtcTimeToUnpublish { get; set; }
    }
}