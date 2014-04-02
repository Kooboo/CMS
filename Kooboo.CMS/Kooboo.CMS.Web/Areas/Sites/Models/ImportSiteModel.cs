#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Models.Options;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
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

        //[Remote("IsRepositoryAvaliable", "Site")]
        //[Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
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
        [UIHint("UploadOrSelect")]
        [DataSource(typeof(SitePackagesDatasource))]
        [Description("Import from uploaded template files or existing site templates(under Cms_Data\\ImportedSites).")]
        [AdditionalMetadata("data-val-filesize", "The site file exceeds the maximum size can be uploaded. Please upload the file under Cms_Data\\ImportedSites folder via FTP, then switch to the selection mode.")]
        [AdditionalMetadata("data-val-filesize-value", 4194304)]
        public string File { get; set; }

        private bool keepSiteSetting = true;
        [Description("Will keep the site setting such like 'Domains','DisplayName' without any changes.")]
        [Display(Name = "Keep settings")]
        public bool KeepSiteSetting { get { return keepSiteSetting; } set { keepSiteSetting = value; } }
    }
}
