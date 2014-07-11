using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Views.Page
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IToolbarProvider), Key = "PageIndexToolbarProvider")]
    public class PageIndexToolbarProvider : IToolbarProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Page",
                    Action = "Index"
                }
                };
            }
        }

        public IEnumerable<ToolbarGroup> GetGroups(System.Web.Routing.RequestContext requestContext)
        {
            return new ToolbarGroup[]{ new ToolbarGroup()
            {
                GroupName = "More",
                DisplayText = "More...",
                HtmlAttributes = new Dictionary<string, object>()
                {                   
                    {"data-show-on-check","Any"},
                    {"data-show-on-selector",".localized"}
                }
            }};
        }

        public IEnumerable<ToolbarButton> GetButtons(System.Web.Routing.RequestContext requestContext)
        {
            List<ToolbarButton> buttons = new List<ToolbarButton>();
            buttons.Add(new ToolbarButton()
            {
                GroupName = "More",
                CommandTarget = new MvcRoute() { Action = "Export", Controller = "Page" },
                CommandText = "Export",
                HtmlAttributes = new Dictionary<string, object>() { { "data-show-on-check", "Any" }, { "data-show-on-selector", ".localized" }, { "data-command-type", "Download" } }
            });
            buttons.Add(new ToolbarButton()
            {
                GroupName = "More",
                CommandTarget = new MvcRoute() { Action = "Copy", Controller = "Page" },
                CommandText = "Copy",
                HtmlAttributes = new Dictionary<string, object>() { { "data-show-on-check", "Single" }, { "data-show-on-selector", ".localized" }, { "data-command-type", "Redirect" } }
            });
            if (ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, requestContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission))
            {
                buttons.AddRange(new ToolbarButton[]{                
                new ToolbarButton(){
                    GroupName="More",
                    CommandTarget = new MvcRoute(){ Action="BatchPublish",Controller="Page"},
                    CommandText="Publish",
                    HtmlAttributes=new Dictionary<string,object>(){{"data-show-on-check","Any"},{ "data-show-on-selector",".unpublished"},{"data-command-type","AjaxPost"},
                    {"data-confirm","Are you sure you want to publish these items?".Localize()}}
                },
                new ToolbarButton(){
                    GroupName="More",
                    CommandTarget = new MvcRoute(){ Action="BatchUnpublish",Controller="Page"},
                    CommandText="Unpublish",
                    HtmlAttributes=new Dictionary<string,object>(){{"id","unpublish_bar"},{"data-show-on-check","Any"},{ "data-show-on-selector",".published"},{"data-command-type","AjaxPost"},
                    {"data-confirm","Are you sure you want to unpublish these items?".Localize()}}
                },                
                new ToolbarButton(){
                    GroupName="More",
                    CommandTarget = new MvcRoute(){ Action="Move",Controller="Page"},
                    CommandText="Move",
                    HtmlAttributes=new Dictionary<string,object>(){{"data-show-on-check","Single"},{ "data-show-on-selector",".localized"},{"data-command-type","Redirect"}}
                },
                new ToolbarButton(){
                    GroupName="More",
                    CommandTarget = new MvcRoute(){ Action="Versions",Controller="Page",RouteValues=new Dictionary<string,object>(){{"title","Show page versions".Localize()}}},
                    CommandText="Versions",
                    HtmlAttributes=new Dictionary<string,object>(){{"data-show-on-check","Single"},{ "data-show-on-selector",".localized"},{"data-command-type","Redirect"}}
                }
            });
            }

            return buttons;
        }
    }
}