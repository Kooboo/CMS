#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class LoadMetaExtensions
    {
        #region Title & meta

        /// <summary>
        /// HTMLs the title.
        /// </summary>
        /// <returns></returns>
        public static IHtmlString HtmlTitle(this IFrontHtmlHelper frontHtml)
        {
            return HtmlTitle(frontHtml, null);
        }
        /// <summary>
        /// HTMLs the title.
        /// </summary>
        /// <param name="defaultTitle">The HTML title.</param>
        /// <returns></returns>
        public static IHtmlString HtmlTitle(this IFrontHtmlHelper frontHtml, string defaultTitle)
        {
            var title = string.IsNullOrEmpty(frontHtml.Page_Context.HtmlMeta.HtmlTitle) ? defaultTitle : frontHtml.Page_Context.HtmlMeta.HtmlTitle;
            if (!string.IsNullOrEmpty(title))
            {
                return new HtmlString(string.Format("<title>{0}</title>", title.StripAllTags()));
            }
            return new HtmlString("");
        }
        public static IHtmlString Meta(this IFrontHtmlHelper frontHtml)
        {
            AggregateHtmlString htmlStrings = new AggregateHtmlString();
            var htmlMeta = frontHtml.Page_Context.HtmlMeta;
            if (htmlMeta != null)
            {
                if (!string.IsNullOrEmpty(htmlMeta.Canonical))
                {
                    htmlStrings.Add(new HtmlString(string.Format("<link rel=\"canonical\" href=\"{0}\"/>", htmlMeta.Canonical.StripAllTags())));
                }
                if (!string.IsNullOrEmpty(htmlMeta.Author))
                {
                    htmlStrings.Add(BuildMeta("author", htmlMeta.Author));
                }

                if (!string.IsNullOrEmpty(htmlMeta.Description))
                {
                    htmlStrings.Add(BuildMeta("description", htmlMeta.Description));
                }

                if (!string.IsNullOrEmpty(htmlMeta.Keywords))
                {
                    htmlStrings.Add(BuildMeta("keywords", htmlMeta.Keywords));
                }
                if (htmlMeta.Customs != null)
                {
                    foreach (var item in htmlMeta.Customs)
                    {
                        htmlStrings.Add(BuildMeta(item.Key, item.Value));
                    }
                }
            }
            return htmlStrings;
        }
        private static IHtmlString BuildMeta(string name, string value)
        {
            return new HtmlString(string.Format("<meta name=\"{0}\" content=\"{1}\" />", name, value.StripAllTags()));
        }
        #endregion
    }
}
