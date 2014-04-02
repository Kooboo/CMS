using Kooboo.Globalization;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Views.Page
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IToolbarProvider), Key = "ABRuleSettingIndexToolbarProvider")]
    public class ABRuleSettingIndexToolbarProvider : IToolbarProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "ABRuleSetting",
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
                    {"data-show-on-check","Any"}
                }
            }};
        }

        public IEnumerable<ToolbarButton> GetButtons(System.Web.Routing.RequestContext requestContext)
        {
            return new ToolbarButton[]{
                new ToolbarButton(){
                    GroupName="More",
                    CommandTarget = new MvcRoute(){ Action="Relations",Controller="ABRuleSetting",RouteValues = new Dictionary<string,object>(){ {"Title","Show A/B rule relations".Localize()}}},
                    CommandText="Relations",
                    HtmlAttributes=new Dictionary<string,object>(){{"data-show-on-check","Single"},{"data-command-type","Redirect"}}
                }
           };
        }
    }
}