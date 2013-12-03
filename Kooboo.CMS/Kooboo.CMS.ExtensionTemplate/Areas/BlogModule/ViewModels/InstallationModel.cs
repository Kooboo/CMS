using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels
{
    public class InstallationModel
    {
        [Required]
        [DisplayName("Connection string")]
        public string ConnectionString { get; set; }
    }
}