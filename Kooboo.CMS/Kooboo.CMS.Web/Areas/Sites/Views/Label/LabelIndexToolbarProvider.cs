using Kooboo.Globalization;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Views.Page
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IToolbarProvider), Key = "LabelIndexToolbarProvider")]
    public class LabelIndexToolbarProvider : IToolbarProvider
    {
        public CMS.Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Label",
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
                DisplayText = "More..."                
            }};
        }

        public IEnumerable<ToolbarButton> GetButtons(System.Web.Routing.RequestContext requestContext)
        {
            return new ToolbarButton[]{
                //new ToolbarButton(){
                //    GroupName="More",
                //    CommandTarget = new MvcRoute(){ Action="UpgradeFromOldLabel",Controller="Label"},
                //    CommandText="Upgrade from .resx format",
                //     HtmlAttributes=new Dictionary<string,object>(){{"data-command-type","AjaxPost"}}
                //}
           };
        }
    }
}