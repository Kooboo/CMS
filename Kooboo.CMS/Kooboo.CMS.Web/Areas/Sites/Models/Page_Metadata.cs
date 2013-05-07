#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Dynamic;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Page))]
    [Grid(Checkable = true, Draggable = true, IdProperty = "UUID", GridItemType = typeof(PageGridItem))]
    [GridColumn(GridItemColumnType = typeof(Page_Draft_GridItemColumn), HeaderText = "Draft", Order = 5)]
    public class Page_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(InheritableEditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsNameAvailable", "page", AdditionalFields = "SiteName,ParentPage,old_Key")]
        public string Name { get; set; }

        public string FullName { get; set; }

        [Display(Name = "Set as homepage")]
        [Required(ErrorMessage = "Required")]
        public bool IsDefault { get; set; }

        [Display(Name = "Theme")]
        [Description("Load the theme CSS files from the system selected theme folder.")]
        [Required(ErrorMessage = "Required")]
        public bool EnableTheming { get; set; }

        [Display(Name = "Script")]
        [Description("Load JavaScript files from the site script folder.")]
        [Required(ErrorMessage = "Required")]
        public bool EnableScript { get; set; }

        [UIHintAttribute("Layout")]
        [DataSource(typeof(LayoutListDataSource))]
        public string Layout { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginsDataSource))]
        public List<string> Plugins { get; set; }

        [GridColumn(Order = 2, GridColumnType = typeof(SortableGridColumn), HeaderText = "Display text")]
        public Navigation Navigation { get; set; }

        [GridColumn(Order = 3, GridColumnType = typeof(SortableGridColumn))]
        public Site Site { get; set; }

        public HtmlMeta HtmlMeta { get; set; }

        public PageRoute Route { get; set; }


        public PagePermission Permission { get; set; }

        [UIHint("DataRules")]
        public List<DataRuleSetting> DataRules { get; set; }

        [UIHint("RadioButtonList")]
        [EnumDataType(typeof(PageType))]
        [Display(Name = "Type of page")]
        [Description("The type of your page used to mark them differently in the sitemap<br /><b>Default: </b> select this option if you do not know your page type<br /><b>Static: </b> contains only manually inputted static content <br /><b>Dynamic:</b> contains dynamic content from database either via the datarule or your own API query.")]
        public PageType PageType { get; set; }

        [Description("Cache your rendered pages for quicker response time in the next page request")]
        [UIHint("OutputCache")]
        public CacheSettings OutputCache { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool? Published { get; set; }

        [GridColumn(Order = 4, HeaderText = "Preview", GridItemColumnType = typeof(Page_Preview_GridItemColumn))]
        public string VirtualPath { get; set; }

        [UIHint("CustomFields")]
        public Dictionary<string, string> CustomFields { get; set; }

        [Display(Name = "Content title")]
        [Description("The title that can be used by this page, the value can be accessed via API: @Page_Context.Current.ContentTitle.")]
        public string ContentTitle { get; set; }

        [Display(Name = "Search index")]
        [Description("Include this page content in the search index.")]
        public bool Searchable { get; set; }
        [Display(Name = "Require HTTPS")]
        [Description("Forces an unsecured HTTP request to be re-sent over HTTPS. Similar with the RequireHttpsAttribute in ASP.NET MVC.")]
        public bool RequireHttps { get; set; }

        [Display(Name = "Cache to disk")]
        [Description("Will generate static html pages for each url serve by this page. Once the static html page eixsts, which will be returned without dynamic processing.")]
        public bool CacheToDisk { get; set; }
    }

    [MetadataFor(typeof(Navigation))]
    public class Navigation_Metadata
    {
        [Display(Name = "Show in menu")]
        [Description("Make this page accessible from the menu API.")]
        [Required(ErrorMessage = "Required")]
        public bool Show { get; set; }

        [Display(Name = "Display text")]
        public string DisplayText { get; set; }

        public int Order { get; set; }


        [Display(Name = "Show in Crumb Path")]
        [Required(ErrorMessage = "Required")]
        [Description("Make this page accessible from the Crumb path API.")]
        public bool? ShowInCrumb { get; set; }
    }

    [MetadataFor(typeof(PageRoute))]
    public class PageRoute_Metadata
    {
        [DisplayName("Link target")]
        [UIHint("DropDownList")]
        [DataSource(typeof(LinkTargetsDataSource))]
        public LinkTarget? LinkTarget { get; set; }

        [Description("Give your page an alternative name. This new name will be used when generating page links. Start with '/' to indicate using as absolute path.")]
        [Display(Name = "Alternative name")]
        [UIHint("Identifier")]
        [Kooboo.CMS.Web.Models.RemoteEx("IsIdentifierAvailable", "*", RouteFields = "SiteName,UUID,parentPage", AdditionalFields = "Name")]
        public string Identifier { get; set; }

        [Description("Customize the page URL to anyway you like.<br /> Use {key} to match the URL query string. <br />For example: {userkey}_othertext to replace parameter \"?userkey=\" into static URL")]
        [Display(Name = "URL path")]
        [RegularExpression("^([^\\?~/])([^\\?])*\\s*$", ErrorMessage = "The route URL cannot start with a '/' or '~' character and it cannot contain a '?' character.")]
        public string RoutePath { get; set; }

        [Description("Default values appended to the page URL query string")]
        [Display(Name = "Default values")]
        [UIHint("Dictionary")]
        public IDictionary<string, string> Defaults { get; set; }

        [Description("In this field you can insert an external URL in case a menu items need to link externally. e.g:http://forum.kooboo.com/. ")]
        [Display(Name = "External URL")]
        public string ExternalUrl { get; set; }
    }

    [MetadataFor(typeof(HtmlMeta))]
    public class HtmlMeta_Metadata
    {
        [Display(Name = "Html title")]
        [Description("The title tag and value that appear in the page HTML header and also used by browsers as the page title. <br />This value can be accessed via API: @Html.FrontHtml().HtmlTitle()")]
        public string HtmlTitle { get; set; }

        [Description("Used in SEO, see: http://googlewebmastercentral.blogspot.com/2009/02/specify-your-canonical.html")]
        public string Canonical { get; set; }

        public string Author { get; set; }

        public string Keywords { get; set; }

        public string Description { get; set; }

        [DisplayName("Custom meta fields")]
        [UIHintAttribute("Dictionary")]
        [Description("Custom fields for HTML meta values")]
        public IDictionary<string, string> Customs
        {
            get;
            set;
        }
    }

    [MetadataFor(typeof(CacheSettings))]
    public class CacheSettings_Metadata
    {
        [Display(Name = "Cache in memory")]
        public bool? EnableCaching { get; set; }

        [DisplayName("Expiration policy")]
        [EnumDataType(typeof(ExpirationPolicy))]
        [UIHint("DropDownList")]
        public ExpirationPolicy ExpirationPolicy { get; set; }
        [Description("The amount of time, in seconds, that a cache entry is to remain in the output cache. The default is 0, which indicates an infinite duration.")]
        public int Duration { get; set; }
    }
}