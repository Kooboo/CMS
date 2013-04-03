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

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ThemesDataSource : ISelectListDataSource
    {
        #region IDropDownListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var siteName = requestContext.GetRequestValue("siteName");
            if (string.IsNullOrEmpty(siteName))
            {
                siteName = requestContext.GetRequestValue("parent");
            }
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = SiteHelper.Parse(siteName);
                foreach (var item in ServiceFactory.ThemeManager.All(site, ""))
                {
                    yield return new SelectListItem() { Text = item.Name, Value = item.Name };
                }
            }
        }

        #endregion
    }
    public class SiteDropDownListTree : ISelectListDataSource
    {
        public static IEnumerable<SelectListItemTree> BuildTree(RequestContext requestContext)
        {
            var sites = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.SiteTrees();
            //yield return new SelectListItemTree() { Selected = true };
            foreach (var s in sites)
            {
                if (ServiceFactory.UserManager.Authorize(s.Root.Site, requestContext.HttpContext.User.Identity.Name))
                {
                    yield return CreateItem(s.Root, requestContext);
                }
            }
        }
        private static SelectListItemTree CreateItem(SiteNode siteNode, RequestContext requestContext)
        {
            if (siteNode == null)
            {
                return new SelectListItemTree();
            }

            var selectListItem = new SelectListItemTree();

            var site = Kooboo.CMS.Sites.Models.IPersistableExtensions.AsActual(siteNode.Site);
            selectListItem.Text = string.IsNullOrEmpty(site.DisplayName) ? site.Name : site.DisplayName;
            selectListItem.Value = site.FullName;

            string parent = requestContext.GetRequestValue("parent");

            selectListItem.Selected = site.FullName.EqualsOrNullEmpty(parent, StringComparison.CurrentCultureIgnoreCase);

            var children = siteNode.Children;
            var items = new List<SelectListItemTree>();
            foreach (var c in children)
            {
                items.Add(CreateItem(c, requestContext));
            }
            selectListItem.Items = items;
            return selectListItem;
        }

        #region ISelectListDataSource Members

        public virtual IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            return BuildTree(requestContext);
        }

        #endregion
    }

    public class SiteDropDownListTreeWithEmptyOption : SiteDropDownListTree
    {
        public override IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            return new[] { new SelectListItemTree() { Text = "", Value = "" } }.Concat(base.GetSelectListItems(requestContext, filter));
        }
    }
    public class RepositoryDataSource : ISelectListDataSource
    {

        #region ISelectListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            IEnumerable<Repository> repositories = new Repository[] { Repository.Current };
            if (ServiceFactory.UserManager.IsAdministrator(requestContext.HttpContext.User.Identity.Name))
            {
                repositories = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.All();
            }


            return repositories.Select(it => it.AsActual()).Select(it => new SelectListItem() { Text = string.IsNullOrEmpty(it.DisplayName) ? it.Name : it.DisplayName, Value = it.Name }).EmptyItem("");
        }

        #endregion
    }
      
    public class Security_Metadata
    {
        [Description(" Turn on/off the content submission API such as ContentService and SendEmail API")]
        [DisplayName("Turn on submission api")]
        public bool TurnOnSubmissionAPI { get; set; }

        [Description("The encrypt key for SecurityHelper.Encrypt/Decrypt.")]
        [DisplayName("Encrypt key")]
        [StringLength(8, MinimumLength = 8)]
        public string EncryptKey { get; set; }
    }

    public class Site_Metadata
    {
        public Site_Metadata() { }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Description("The name of your website")]
        public string Name { get; set; }

        [Description("The name of your website how it would be displayed on this CMS")]
        public string DisplayName { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        [Description("The culture shown on your website. <br /> This is used to display currency, date, number and other culture related content")]
        public string Culture { get; set; }


        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(ThemesDataSource))]
        [Description("Load theme CSS files from the theme folder")]
        public string Theme { get; set; }

        [Description("Your website domains, without \"http://\"")]
        [Display(Name = "Domains")]
        [UIHint("Array")]
        public string[] Domains { get; set; }

        [Description("The virtual path of your website. You may create a subsite eg. www.kooboo.com/china, <b>china</b> is the SitePath.")]
        public string SitePath { get; set; }

        // [RegularExpression(Kooboo.RegexPatterns.Version)]
        //[UIHintAttribute("Version")]
        [Description("Give your wesite a version number, it can be any number as you like")]
        public string Version { get; set; }

        [Description("Use <b>debug</b> on development and use <b>release</b> on live site. <b>Release</b> mode utilizes CSS/JS compression and other techniques")]
        [EnumDataType(typeof(ReleaseMode))]
        [UIHintAttribute("RadioButtonList")]
        [Required(ErrorMessage = "Required")]
        public ReleaseMode Mode { get; set; }

        [UIHint("DropDownListTree")]
        [DataSource(typeof(SiteDropDownListTree))]
        [Description("The parent website which this new website will base on")]
        public Site Parent { get; set; }

        [Remote("IsRepositoryAvaliable", "Site", AdditionalFields = "IsNew")]
        [Required(ErrorMessage = "Required")]
        [Description("Select the database where <br/> your content is stored or create a new database")]
        [UIHint("ChooseRepository")]
        [DataSource(typeof(RepositoryDataSource))]
        [Display(Name = "Content database")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "Only alphameric and numeric are allowed in the field name")]
        public string Repository { get; set; }


        [Display(Name = "Enable inline editing")]
        [Description("Enables you to edit content on the front end of your website")]
        public bool? InlineEditing { get; set; }

        [Display(Name = "Show start sitemap")]
        [Description("Shows the sitemap as the start page in the menu")]
        public bool? ShowSitemap { get; set; }


        [Display(Name = "JQuery")]
        [Description("Load JQuery and JQuery validation files")]
        [Required(ErrorMessage = "Required")]
        public bool EnableJquery { get; set; }

        [Display(Name = "Custom fields")]
        [UIHint("Dictionary")]
        [Description(@"Custom field values that can be accessed in front end site via API: @Site.Current.CustomFields[""key""]")]
        public Dictionary<string, string> CustomFields { get; set; }

        [Display(Name = "Enable versioning")]
        [Description("Enable version control on Layout, View and Page")]
        public bool? EnableVersioning { get; set; }

        [Display(Name = "Enable style edting")]
        [Description("Enable the user can edit the style files on the front-end.")]
        public bool? EnableStyleEdting { get; set; }
        [Display(Name = "Resource domain")]
        [Description("Used to generate resource links different with site domain. e.g: Generate CDN links for scripts and styles.")]
        public string ResourceDomain { get; set; }
    }
}