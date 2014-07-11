using Kooboo.Common.Globalization;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Views.DataSource
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IToolbarProvider), Key = "Kooboo.CMS.Web.Areas.Sites.Views.DataSource.IndexToolbarProvider")]
    public class IndexToolbarProvider : IToolbarProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "DataSource",
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
                CommandTarget = new MvcRoute() { Action = "Embedded", Controller = "DataSource", RouteValues = new Dictionary<string, object>() { } },
                CommandText = "Embedded relation",
                HtmlAttributes = new Dictionary<string, object>() { { "data-show-on-check", "Single" }, { "data-show-on-selector", ".localized" }, { "data-command-type", "Redirect" } }
            });

            buttons.Add(new ToolbarButton()
            {
                GroupName = "More",
                CommandTarget = new MvcRoute() { Action = "Relations", Controller = "DataSource", RouteValues = new Dictionary<string, object>() { { "title", "Show data source relations".Localize() } } },
                CommandText = "Relations",
                HtmlAttributes = new Dictionary<string, object>() { { "data-show-on-check", "Single" }, { "data-show-on-selector", ".localized" }, { "data-command-type", "Redirect" } }
            });


            return buttons;
        }
    }
}