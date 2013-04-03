using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Web.Models;
using Kooboo.Dynamic;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    public class PreviewLinkRender : IItemColumnRender
    {

        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {

            var isStaticPage = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.IsStaticPage(Kooboo.CMS.Sites.Models.Site.Current, (Page)dataItem);

            UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);
            var href = FrontUrlHelper.Preview(urlHelper, Kooboo.CMS.Sites.Models.Site.Current, (Page)dataItem, null);

            if (!isStaticPage)
            {
                return new HtmlString("");
            }

            return new HtmlString(string.Format(@"<a href=""{0}"" target=""_blank"" class=""o-icon preview"" title=""{2}"">{1}</a>", href, "Preview".Localize(), "Preview".Localize()));
        }
    }

    public class PageNameColumnRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            var page = ((Page)dataItem).AsActual();
            string homePageIcon = page.IsDefault ? string.Format("<span class=\"o-icon home\" title=\"{0}\"></span>", "Home page".Localize()) : "";
            if (dataItem is IInheritable)
            {
                var inheritable = (IInheritable)dataItem;
                if (!inheritable.IsLocalized(Site.Current))
                {
                    return new HtmlString((value == null ? "" : value.ToString()) + homePageIcon);
                }
            }

            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            return new HtmlString(string.Format(@"<a name='PageName' href=""{0}"">{1}</a>",
                url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("FullName", page.FullName).
                Merge("ReturnUrl", viewContext.RequestContext.HttpContext.Request.RawUrl))
                , value) + homePageIcon);

        }
    }
    internal class FullNameRenderType : IItemColumnRender
    {
        #region IItemColumnRender Members

        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            var text = ((Page)dataItem).Navigation == null ? "" : ((Page)dataItem).Navigation.DisplayText;
            return new HtmlString(System.Web.HttpUtility.HtmlEncode(text));
        }

        #endregion
    }

    public class PageDataRuleRouteValueGetter : IGridActionRouteValuesGetter
    {
        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, ViewContext viewContext)
        {
            routeValueDictionary["ReturnUrl"] = (new UrlHelper(viewContext.RequestContext)).Action("Index", viewContext.RequestContext.AllRouteValues());
            routeValueDictionary["from"] = "page";
            return routeValueDictionary;
        }
    }
    public class EditDraftVisible : InheritableGridActionVisibleArbiter
    {
        public override bool IsVisible(object dataItem, ViewContext viewContext)
        {
            var visible = base.IsVisible(dataItem, viewContext);
            if (visible)
            {
                var page = (Page)dataItem;
                visible = ServiceFactory.PageManager.HasDraft(page);
            }
            return visible;
        }
    }
    public class PublishColumnRender : BooleanColumnRender
    {
        public override IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            var page = (Page)dataItem;
            var allowToPublish = ServiceFactory.UserManager.Authorize(Site.Current, viewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission);
            allowToPublish = allowToPublish && page.IsLocalized(Site.Current);
            if (allowToPublish)
            {
                string url = "";
                var published = (bool)value;

                var urlHelper = new UrlHelper(viewContext.RequestContext);
                string tip = "Click to {0}";
                string @class = "";
                if (published)
                {
                    url = urlHelper.Action("Unpublish", viewContext.RequestContext.AllRouteValues().Merge("FullName", page.FullName));
                    tip = string.Format(tip, "unpublish").Localize();
                    @class = "actionCommand";
                }
                else
                {
                    url = urlHelper.Action("Publish", viewContext.RequestContext.AllRouteValues().Merge("FullName", page.FullName));
                    tip = string.Format(tip, "publish").Localize();
                    @class = "dialog-link";
                }
                return new HtmlString(string.Format(@"<a class=""o-icon {0} {1} "" href=""{2}"" title=""{3}"">{1}</a>"
                    , GetIconClass(value)
                    , @class
                    , url
                    , tip));

            }
            else
            {
                return base.Render(dataItem, value, viewContext);
            }

        }
    }

    public class VersionGridActionVisibleArbiter : IColumnVisibleArbiter
    {
        public bool IsVisible(ViewContext viewContext)
        {
            return Site.Current.EnableVersioning.HasValue ? Site.Current.EnableVersioning.Value : true;
        }
    }

    [Grid(Checkable = true, IdProperty = "FullName", CheckVisible = typeof(InheritableCheckVisible))]
    [GridAction(ActionName = "CopyPage", DisplayName = "Copy", RouteValueProperties = "Name,SourcePage=FullName", Order = 1, Class = "o-icon copy dialog-link", CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValuesGetter = typeof(PageDataRuleRouteValueGetter))]
    [GridAction(DisplayName = "Move", ActionName = "MovePage", RouteValueProperties = "Name,fullName=FullName,fromSite=Site", Order = 2, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), Class = "o-icon move-page dialog-link", RouteValuesGetter = typeof(PageDataRuleRouteValueGetter))]
    [GridAction(ActionName = "Edit", Class = "o-icon edit", RouteValueProperties = "Name,fullName=FullName", Order = 3, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValuesGetter = typeof(PageDataRuleRouteValueGetter))]
    [GridAction(ActionName = "Draft", Class = "o-icon edit", RouteValueProperties = "Name,fullName=FullName", Order = 4, CellVisibleArbiter = typeof(EditDraftVisible), RouteValuesGetter = typeof(PageDataRuleRouteValueGetter))]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", Class = "o-icon localize", ConfirmMessage = "Are you sure you want to localize this item?",
        RouteValueProperties = "Name,fullName=FullName", Order = 5, RouteValuesGetter = typeof(PageDataRuleRouteValueGetter), Renderer = typeof(LocalizationRender))]
    [GridAction(DisplayName = "Version", ActionName = "Version", RouteValueProperties = "Name,fullName=FullName,fromSite=Site", Order = 7, ColumnVisibleArbiter = typeof(VersionGridActionVisibleArbiter), Class = "o-icon version dialog-link", RouteValuesGetter = typeof(PageDataRuleRouteValueGetter))]
    public class Page_Metadata
    {
        [GridColumn(Order = 1, ItemRenderType = typeof(PageNameColumnRender))]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsNameAvailable", "page", AdditionalFields = "SiteName,ParentPage,old_Key")]
        public string Name { get; set; }

        [GridColumn(Order = 2, HeaderText = "Display text", ItemRenderType = typeof(FullNameRenderType))]
        public string FullName { get; set; }

        [Display(Name = "Set as homepage")]
        [Required(ErrorMessage = "Required")]
        public bool IsDefault { get; set; }

        [Display(Name = "Theme")]
        [Description("Load the theme css files from the system selected theme folder")]
        [Required(ErrorMessage = "Required")]
        public bool EnableTheming { get; set; }

        [Display(Name = "Script")]
        [Description("Load JavaScript files from the site script folder")]
        [Required(ErrorMessage = "Required")]
        public bool EnableScript { get; set; }

        [UIHintAttribute("Layout")]
        [DataSource(typeof(LayoutListDataSource))]
        public string Layout { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginDataSource))]
        public List<string> Plugins { get; set; }

        [GridColumn(Order = 13)]
        public Site Site { get; set; }

        public HtmlMeta HtmlMeta { get; set; }

        public PageRoute Route { get; set; }

        public Navigation Navigation { get; set; }

        public PagePermission Permission { get; set; }

        [UIHint("DataRules")]
        public List<DataRuleSetting> DataRules { get; set; }

        [UIHint("RadioButtonList")]
        [EnumDataType(typeof(PageType))]
        [Display(Name = "Type of page")]
        [Description("The type of your page, used to mark them differently in the sitemap<br /><b>Default: </b> select this option if you do not know your page type<br /><b>Static: </b> contains only manually input static content <br /><b>Dynamic:</b> contains dynamic content from database either via the datarule or your own API query")]
        public PageType PageType { get; set; }

        [Description("Cache your rendered pages for quicker response time in the next page request")]
        [UIHint("OutputCache")]
        public CacheSettings OutputCache { get; set; }

        [GridColumn(Order = 14, HeaderText = "Preview", Class = "action", ItemRenderType = typeof(PreviewLinkRender))]
        public string VirtualPath { get; set; }

        [GridColumn(Order = 15, HeaderText = "Publish", Class = "action", ItemRenderType = typeof(PublishColumnRender))]
        [Required(ErrorMessage = "Required")]
        public bool? Published { get; set; }

        [UIHint("CustomFields")]
        public Dictionary<string, string> CustomFields { get; set; }

        [Display(Name = "Content title")]
        [Description("The title that can be used by page content, this value can be accessed via API: @Page_Context.Current.ContentTitle")]
        public string ContentTitle { get; set; }

        [Display(Name = "Search index")]
        [Description("Include this page content in the search index")]
        public bool Searchable { get; set; }

    }
    public class Navigation_Metadata
    {
        [Display(Name = "Show in menu")]
        [Description("Make this page accessiable from the menu API")]
        [Required(ErrorMessage = "Required")]
        public bool Show { get; set; }

        [Display(Name = "Display text")]
        public string DisplayText { get; set; }

        public int Order { get; set; }


        [Display(Name = "Show in Crumb Path")]
        [Required(ErrorMessage = "Required")]
        [Description("Make this page accesiable from the Crumb path API.")]
        public bool? ShowInCrumb { get; set; }
    }

    public class LinkTargetDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in Enum.GetValues(typeof(LinkTarget)))
            {
                list.Add(new SelectListItem() { Text = item.ToString(), Value = ((int)item).ToString() });
            }
            return list.EmptyItem("");
        }
    }
    public class Pageroute_Metadata
    {
        [UIHint("DropDownList")]
        [DataSource(typeof(LinkTargetDataSource))]
        public LinkTarget? LinkTarget { get; set; }

        [Description("Give your page an alternative name. This new name will be used when generating page links. Start with '/' to indicate using as absolute path.")]
        [Display(Name = "Alternative name")]
        [UIHint("Identifier")]
        [Kooboo.CMS.Web.Models.RemoteEx("IsIdentifierAvailable", "*", RouteFields = "SiteName,FullName,parentPage", AdditionalFields = "Name")]
        public string Identifier { get; set; }

        [Description("Customize the page URL to anyway you like.<br /> Use {key} to match the URL query string. <br />For example: {userkey}_othertext to replace parameter \"?userkey=\" into static URL")]
        [Display(Name = "URL path")]
        public string RoutePath { get; set; }

        [Description("Default values appended to the page URL query string")]
        [Display(Name = "Default values")]
        [UIHint("Dictionary")]
        public IDictionary<string, string> Defaults { get; set; }

        [Description("In this field you can insert an external URL in case a menu items need to link externally. e.g:http://forum.kooboo.com/ ")]
        [Display(Name = "External url")]
        public string ExternalUrl { get; set; }
    }
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
    public class LayoutListDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;
            var layoutList = Kooboo.CMS.Sites.Persistence.Providers.LayoutProvider.All(site);
            foreach (var l in layoutList)
            {
                yield return new System.Web.Mvc.SelectListItem() { Value = l.Name, Text = l.Name };
            }
        }
    }

    public class CopyPageModel
    {
        [Required(ErrorMessage = "Required")]
        [Remote("IsNameAvailable", "page", AdditionalFields = "SiteName,ParentPage")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string Name { get; set; }


        [UIHint("DropDownListTree")]
        [DataSource(typeof(PageDropDownListTreeDataSource))]
        public string ParentPage { get; set; }
        public Page SourcePage { get; set; }
        [Display(Name = "Create automatic redirect")]
        public bool CreateRedirect { get; set; }

    }
    public class PageDropDownListTreeDataSource : ISelectListDataSource
    {

        public static PageDropDownListTreeDataSource Instance()
        {
            return new PageDropDownListTreeDataSource();
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            IEnumerable<SelectListItemTree> result = new List<SelectListItemTree>() { new SelectListItemTree() { } };
            var rootPages = ServiceFactory.PageManager.All(Site.Current, null);

            var pageTree = rootPages.Select(o => CreateSelectItemTreeNode(o));

            return result.Concat(pageTree);
        }
        private SelectListItemTree CreateSelectItemTreeNode(Page parent)
        {
            var node = new SelectListItemTree();
            node.Text = parent.Name;
            node.Value = parent.FullName;

            var children = ServiceFactory.PageManager.ChildPages(Site.Current, parent.FullName, null);

            if (children != null)
            {
                node.Items = children.Select(o => CreateSelectItemTreeNode(o));
            }


            return node;
        }
    }
}