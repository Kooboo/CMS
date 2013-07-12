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

    public class ImportSiteModel
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsSiteNameAvailable", "Site", AdditionalFields = "Parent")]
        [Description("The site name")]
        public string Name { get; set; }


        [UIHint("DropDownList")]
        [DataSource(typeof(SitesDataSource))]
        [Description("The parent website which this new website will be based on.")]
        public string Parent { get; set; }

        [Remote("IsRepositoryAvaliable", "Site")]
        [Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
        [DataSource(typeof(RepositoriesDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("UploadOrSelect")]
        [DataSource(typeof(SitePackagesDatasource))]
        [Description("Import from uploaded template files or existing site templates(under Cms_Data\\ImportedSites).")]
        [AdditionalMetadata("data-val-filesize", "The site file exceeds the maximum size can be uploaded. Please upload the file under Cms_Data\\ImportedSites folder via FTP, then switch to the selection mode.")]
        [AdditionalMetadata("data-val-filesize-value", 4194304)]
        public string File { get; set; }

    }

    public class ImportRepositoryModel
    {
        [Display(Name = "Delete the old content database")]
        [Description("The old content database which is not used by other sites will be deleted.")]
        public bool DeleteTheOldRepository
        {
            get;
            set;
        }
        [Remote("IsRepositoryAvaliable", "Site")]
        [Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("UploadOrSelect")]
        [DataSource(typeof(RepositoryPackagesDatasource))]
        public string File { get; set; }


    }

}