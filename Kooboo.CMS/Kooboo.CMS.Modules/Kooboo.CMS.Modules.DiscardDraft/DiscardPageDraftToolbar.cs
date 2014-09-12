#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Extension.UI;
using Kooboo.CMS.Sites.Extension.UI.TopToolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Modules.DiscardDraft
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IToolbarProvider), Key = "DiscardPageDraftToolbar")]
    public class DiscardPageDraftToolbar : IToolbarProvider
    {
        public Sites.Extension.UI.MvcRoute[] ApplyTo
        {
            get
            {
                return new[]{
                    new MvcRoute()
                {
                    Area = "Sites",
                    Controller = "Page",
                    Action = "Draft"
                }
                };
            }
        }

        public IEnumerable<ToolbarButton> GetButtons(System.Web.Routing.RequestContext requestContext)
        {
            List<ToolbarButton> buttons = new List<ToolbarButton>();
            buttons.Add(new ToolbarButton()
            {
                CommandTarget = new MvcRoute() { Action = "Discard", Controller = "PageDraft", Area = "_PageDraft" },
                CommandText = "Discard",
                IconClass = "delete",
                HtmlAttributes = new Dictionary<string, object>() { { "data-ajaxform", "" }, { "data-confirm", "Are you sure you want to discard this draft?".Localize() } }
            });

            return buttons;
        }

        public IEnumerable<ToolbarGroup> GetGroups(System.Web.Routing.RequestContext requestContext)
        {
            return new ToolbarGroup[0];
        }
    }
}
