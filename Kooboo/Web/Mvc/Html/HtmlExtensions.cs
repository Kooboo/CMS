using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Url;

namespace Kooboo.Web.Mvc.Html
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Normalizes a url in the form ~/path/to/resource.aspx.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ResolveUrl(this HtmlHelper html, string relativeUrl)
        {
            return UrlUtility.ResolveUrl(relativeUrl);
        }
        /// <summary>
        /// Renders a script tag referencing the javascript file. 
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="scriptFile">The script file.</param>
        /// <returns></returns>
        public static IHtmlString Script(this HtmlHelper html, string scriptUrl)
        {
            string url = ResolveUrl(html, scriptUrl);
            return new HtmlString(string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>\n", url));
        }
        /// <summary>
        /// Renders a link tag referencing the stylesheet.  
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="cssUrl">The CSS file.</param>
        /// <returns></returns>
        public static IHtmlString Stylesheet(this HtmlHelper html, string cssUrl)
        {
            string url = ResolveUrl(html, cssUrl);
            return new HtmlString(string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />\n", url));
        }
        /// <summary>
        /// Renders a link tag referencing the stylesheet.  
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="cssUrl">The CSS file.</param>
        /// <param name="media">The media.</param>
        /// <returns></returns>
        public static IHtmlString Stylesheet(this HtmlHelper html, string cssUrl, string media)
        {
            string url = ResolveUrl(html, cssUrl);
            return new HtmlString(string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" media=\"{1}\" />\n", url, media));
        }
    }
}
