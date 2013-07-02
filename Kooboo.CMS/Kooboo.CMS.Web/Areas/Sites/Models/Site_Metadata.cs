#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Dynamic;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [MetadataFor(typeof(Site))]
    public class Site_Metadata
    {
        public Site_Metadata() { }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsSiteNameAvailable", "Site", AdditionalFields = "Parent")]
        [Description("The name of your website.")]
        public string Name { get; set; }

        [Description("The name of your website how it would be displayed on this CMS")]
        public string DisplayName { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture shown on your website. <br /> This is used to display currency, date, number and other culture related content.")]
        public string Culture { get; set; }


        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(ThemesDataSource))]
        [Description("Load theme CSS files from the theme folder")]
        public string Theme { get; set; }

        [Description("Your website domains, without \"http://\"")]
        [Display(Name = "Domains")]
        [UIHint("Array")]
        public string[] Domains { get; set; }

        [DisplayName("Site path")]
        [Description("The virtual path of your website. You may create a subsite eg. www.kooboo.com/china, <b>china</b> is the Site path.")]
        public string SitePath { get; set; }

        // [RegularExpression(Kooboo.RegexPatterns.Version)]
        //[UIHintAttribute("Version")]
        [Description("Give your website a version number, it can be any number as you like.")]
        public string Version { get; set; }

        [Description("Use <b>debug</b> on development and use <b>release</b> on live site. <b>Release</b> mode utilizes CSS/JS compression and other techniques.")]
        [EnumDataType(typeof(ReleaseMode))]
        [UIHintAttribute("RadioButtonList")]
        [Required(ErrorMessage = "Required")]
        public ReleaseMode Mode { get; set; }

        [Required(ErrorMessage = "Required")]
        [Description("Select the database where <br/> your content is stored or create a new database.")]
        [UIHint("DropDownList")]
        [DataSource(typeof(RepositoriesDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }


        [Description("Select a membership data source for the site.")]
        [UIHint("DropDownList")]
        [DataSource(typeof(MembershipDataSource))]
        public string Membership { get; set; }

        [Display(Name = "Enable inline editing")]
        [Description("Enables you to edit content from the front end of your website.")]
        public bool? InlineEditing { get; set; }

        [Display(Name = "Show start sitemap")]
        [Description("Shows the sitemap as the start page in the menu.")]
        public bool? ShowSitemap { get; set; }


        [Display(Name = "JQuery")]
        [Description("Load JQuery and JQuery validation files.")]
        [Required(ErrorMessage = "Required")]
        public bool EnableJquery { get; set; }

        [Display(Name = "Custom fields")]
        [UIHint("Dictionary")]
        [Description(@"Custom field values that can be accessed in front end site via API: @Site.Current.CustomFields[""key""].")]
        public Dictionary<string, string> CustomFields { get; set; }

        [Display(Name = "Enable versioning")]
        [Description("Enable version control on Layout, View and Page.")]
        public bool? EnableVersioning { get; set; }

        [Display(Name = "Enable style edting")]
        [Description("Enable the user can edit the style files on the front-end.")]
        public bool? EnableStyleEdting { get; set; }
        [Display(Name = "Resource domain")]
        [Description("Used to generate resource links different with site domain. e.g: Generate CDN links for scripts and styles.")]
        public string ResourceDomain { get; set; }

        [Display(Name = "Time zone")]
        [UIHint("DropDownList")]
        [DataSource(typeof(TimeZonesDataSource))]
        public string TimeZoneId { get; set; }
    }
}