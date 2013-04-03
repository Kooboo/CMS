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

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    public class CreateSiteModel
    {
        public CreateSiteModel() { }
        public CreateSiteModel(Site site)
        {
            this.Name = site.Name;
            this.DisplayName = site.DisplayName;
            this.Culture = site.Culture;
            this.Theme = site.Theme;
            this.Domains = site.Domains;
            this.SitePath = site.SitePath;
            this.Mode = site.Mode;
            this.Parent = site.Parent == null ? "" : site.Parent.FullName;
            this.Repository = site.Repository;
        }

        [Required()]
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

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(ThemesDataSource))]
        [Description("Load theme CSS files from the theme folder")]
        public string Theme { get; set; }

        [Description("Your website domains")]
        [Display(Name = "Domains")]
        [UIHint("Array")]
        public string[] Domains { get; set; }

        [Description("The virtual path of your website. <br/> You may create a subsite eg. www.kooboo.com/china, <b>china</b> is the SitePath.")]
        public string SitePath { get; set; }

        [UIHintAttribute("Version")]
        [Description("Give your wesite a version number, <br/> it can be any number as you like")]
        public string Version { get; set; }

        [Description("Use <b>debug</b> on development and  use <b>release</b> on live site. <br/><b>Release</b> mode utilizes CSS/JS compression and other techniques")]
        [EnumDataType(typeof(ReleaseMode))]
        [UIHintAttribute("RadioButtonList")]// why to use UIHintAttribute ??  The EnumDataTypeAttribute inherit from DataTypeAttribute.
        public ReleaseMode Mode { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownListTree")]
        [DataSource(typeof(SiteDropDownListTreeWithEmptyOption))]
        [Description("The parent website which this new website will base on")]
        public string Parent { get; set; }

        [Remote("IsRepositoryAvaliable", "Site", AdditionalFields = "IsNew")]
        [Required(ErrorMessage = "Required")]
        [Description("Select the database where <br/> your content is stored or create a new database")]
        [UIHint("ChooseRepository")]
        [DataSource(typeof(RepositoryDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(SiteTemplateDataSource))]
        public string Template { get; set; }

        [Display(Name = "Custom fields")]
        [UIHint("Dictionary")]
        public Dictionary<string, string> CustomFields { get; set; }

        private bool? enableVersioning;
        public bool? EnableVersioning
        {
            get
            {
                if (enableVersioning == null)
                {
                    enableVersioning = true;
                }
                return enableVersioning;
            }
            set
            {
                enableVersioning = value;
            }
        }
    }
    public class SiteTemplateDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Sites.Services.ServiceFactory.SiteTemplateManager.All().Select(it => new SelectListItem()
                       {
                           Text = it.TemplateName,
                           Value = it.TemplateName
                       }).EmptyItem("   ");
        }
    }

    public class SitePackageDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return ServiceFactory.ImportedSiteManager
                .All()
                .Select(o => new SelectListItem
                {
                    Text = o.TemplateName,
                    Value = o.TemplateName
                });
        }
    }

    public class RepositoryPackageDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Content.Services.ServiceFactory.RepositoryTemplateManager
                .All()
                .Select(o => new SelectListItem
                {
                    Text = o.TemplateName,
                    Value = o.TemplateName
                });
        }
    }


    public class ImportSiteModel
    {
        [Required()]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsSiteNameAvailable", "Site", AdditionalFields = "Parent")]
        [Description("The site name")]
        public string Name { get; set; }


        [UIHint("DropDownListTree")]
        [DataSource(typeof(SiteDropDownListTreeWithEmptyOption))]
        [Description("The parent website which this new website will base on")]
        public string Parent { get; set; }

        [Remote("IsRepositoryAvaliable", "Site")]
        [Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
        [DataSource(typeof(RepositoryDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("SiteSelector")]
        [DataSource(typeof(SitePackageDatasource))]
        public string File { get; set; }


    }

    public class ImportRepositoryModel
    {
        [Remote("IsRepositoryAvaliable", "Site")]
        [Required(ErrorMessage = "Required")]
        [Description("The name of your database to be created")]
        [DataSource(typeof(RepositoryDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [UIHint("SiteSelector")]
        [DataSource(typeof(RepositoryPackageDatasource))]
        public string File { get; set; }


    }

}