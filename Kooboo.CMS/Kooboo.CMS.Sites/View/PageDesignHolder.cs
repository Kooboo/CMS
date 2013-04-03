using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace Kooboo.CMS.Sites.View
{
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Sites.Services;

    public class PageDesignHolder : PageDesignHtml
    {
        public PageDesignHolder(FrontHtmlHelper frontHtml, string layoutPositionId)
        {
            //this.TagName = "ul";
            //this.Attribute.Add("style", "list-style-type: none;");

            this.ClassName = "pagedesign-holder";

            this.Parameter.Add("LayoutPositionId", layoutPositionId);

            var children = frontHtml.PageContext.PageRequestContext.Page.PagePositions
                .Where(o => o.LayoutPositionId.Equals(layoutPositionId, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(o => o.Order).ToList();

            var site = frontHtml.PageContext.PageRequestContext.Site;
            foreach (var c in children)
            {
                var pos = Parse(c, site);
                if (pos != null)
                    this.Children.Add(pos);
            }
        }

        public static IHtmlString Parse(PagePosition position, Site site)
        {
            if (position == null) { return null; }

            var view = position as ViewPosition;
            if (view != null) { return new PageDesignViewContent(view); }

            var module = position as ModulePosition;
            if (module != null) { return new PageDesignModuleContent(module); }

            var html = position as HtmlPosition;
            if (html != null) { return new PageDesignHtmlContent(html); }

            var folder = position as ContentPosition;
            if (folder != null) { return new PageDesignFolderContent(folder); }

            var htmlBlock = position as HtmlBlockPosition;
            if (htmlBlock != null)
            {
                var entry = htmlBlock.GetHtmlBlock(site);
                if (entry != null)
                {
                    return new PageDesignHtmlBlockContent(htmlBlock, entry.Body);
                }
            }

            return new HtmlString("");
        }
    }
}
