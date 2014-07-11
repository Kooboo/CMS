using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Models.Options;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Misc;
using Kooboo.Common.Web.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class CopySiteModel
    {
        public string Parent { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsSiteNameAvailable", "Site", AdditionalFields = "Parent")]
        [Description("The site name")]
        public string Name { get; set; }

        [Remote("IsRepositoryAvaliable", "Site", AdditionalFields = "CreateNew")]
        [Required(ErrorMessage = "Required")]
        [Description("Create a new database or select the database where <br/> your content is stored.")]
        [UIHint("CreateOrSelect")]
        [DataSource(typeof(RepositoriesDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Description("Create a new membership or select the membership.")]
        [UIHint("CreateOrSelect")]
        [DataSource(typeof(MembershipDataSource))]
        [Display(Name = "Membership")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Membership { get; set; }


        public CreateSiteOptions ToCreateSiteOptions()
        {
            var options = new CreateSiteOptions()
            {
                RepositoryName = Repository,
                MembershipName = Membership,
            };

            return options;
        }
    }
}