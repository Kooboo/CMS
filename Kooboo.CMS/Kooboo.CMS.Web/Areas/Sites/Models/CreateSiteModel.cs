#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Services;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Models;
using System.Web.Routing;
using Kooboo.Dynamic;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class CreateSiteModel
    {
        public CreateSiteModel() { }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsSiteNameAvailable", "Site", AdditionalFields = "Parent")]
        [Description("The site name")]
        public string Name { get; set; }

        [Description("The site display name")]
        public string DisplayName { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("Your front site culture, this is used to display currency,<br/>  date, number or other culture related content")]
        public string Culture { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(SitesDataSource))]
        [Description("The parent website which this new website will be based on.")]
        public string Parent { get; set; }

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


        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(SiteTemplatesDataSource))]
        public string Template { get; set; }

        [Display(Name = "Custom fields")]
        [UIHint("Dictionary")]
        public Dictionary<string, string> CustomFields { get; set; }

        [Display(Name = "Time zone")]
        [UIHint("DropDownList")]
        [DataSource(typeof(TimeZonesDataSource))]
        public string TimeZoneId { get; set; }
    }
}