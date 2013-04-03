using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Form.Html
{
    public static class HtmlCodeHelper
    {
        /// <summary>
        /// abc"cde => abc""cde, 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EscapeQuote(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            return s.Replace("\"", "\"\"");
        }
        public static string RazorHtmlEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlEncode(s).Replace("@", "@@");
        }
        public static string RazorHtmlAttributeEncode(this string s)
        {
            return System.Web.HttpUtility.HtmlAttributeEncode(s).Replace("@", "@@");
        }

        public static HtmlString RenderColumnValue(this object v)
        {
            if (v == null)
            {
                return new HtmlString("");
            }

            if (v is string)
            {
                var s = v.ToString();

                if (s.StartsWith("~/") || s.StartsWith("http://"))
                {
                    var url = UrlUtility.ResolveUrl(s);
                    var extension = Path.GetExtension(s).ToLower();
                    if (extension == ".gif" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".ico")
                    {
                        return new HtmlString(string.Format("<img src='{0}' width='100' height='100'/>", url));
                    }
                    else
                    {
                        return new HtmlString(string.Format("<a href='{0}'>{0}</a>", url));
                    }
                }
                return new HtmlString(s);

            }
            else
            {
                return new HtmlString(v.ToString());
            }
        }
    }
}
