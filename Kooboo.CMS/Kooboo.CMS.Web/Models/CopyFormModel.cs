using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Kooboo.CMS.Web.Models
{
    public class CopyFormModel
    {

        public string SourceName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "New Name")]
        [RemoteEx("CopyNameAvailabled", "*", RouteFields = "SiteName,RepositoryName")]
        public string DestName { get; set; }
    }
}