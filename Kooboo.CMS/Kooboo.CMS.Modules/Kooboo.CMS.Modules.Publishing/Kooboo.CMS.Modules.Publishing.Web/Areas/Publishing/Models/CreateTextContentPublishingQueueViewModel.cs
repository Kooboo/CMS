using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    public class CreateTextContentPublishingQueueViewModel
    {
        [DisplayName("Folder name")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(TextFoldersDataSource))]
        public string LocalFolderId { get; set; }

        public string[] TextContents { get; set; }

        public string[] ObjectTitles { get; set; }

        [DataSource(typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources.RemotePublishingMappingDataSource))]
        [UIHint("Multiple_DropDownList")]
        [DisplayName("Textfolder mappings")]
        public string[] TextFolderMappings { get; set; }

        [DisplayName("Schedule")]
        public bool Schedule { get; set; }

        [DisplayName("Publish time")]
        public DateTime? UtcTimeToPublish { get; set; }

        [DisplayName("Unpublish time")]
        public DateTime? UtcTimeToUnpublish { get; set; }
    }
}