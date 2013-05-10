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
using System.Web;
using System.IO;
using Kooboo.Web.Url;
using Kooboo.Globalization;
using Kooboo.CMS.Common;
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

        public static IHtmlString RenderColumnValue(this object v)
        {
            if (v == null)
            {
                return new HtmlString("-");
            }
            if (v is bool)
            {
                if (((bool)v) == true)
                {
                    return new HtmlString("YES".Localize());
                }
                else
                {
                    return new HtmlString("-");
                }
            }
            if (v is string)
            {
                var s = v.ToString();

                if (s.StartsWith("~/") || s.StartsWith("/") || s.StartsWith("http://"))
                {
                    List<IHtmlString> htmlStrings = new List<IHtmlString>();
                    foreach (var item in s.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        htmlStrings.Add(RenderFile(item));
                    }

                    return new AggregateHtmlString(htmlStrings);

                }
                return new HtmlString(System.Web.HttpUtility.HtmlEncode(s.Trim()));

            }
            else
            {
                return new HtmlString(v.ToString().Trim());
            }
        }

        private static IHtmlString RenderFile(string item)
        {
            var url = UrlUtility.ResolveUrl(item);
            try
            {
                var extension = Path.GetExtension(item).ToLower();
                if (extension == ".gif" || extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".ico")
                {
                    return new HtmlString(string.Format("<img src='{0}' width='100' height='100'/>", url));
                }
                else
                {
                    return new HtmlString(string.Format("<a href='{0}'>{0}</a>", url));
                }
            }
            catch
            {
                return new HtmlString(item.Trim());
            }
           
        }
    }
}
