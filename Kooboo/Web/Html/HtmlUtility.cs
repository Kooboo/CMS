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
using System.Text.RegularExpressions;

namespace Kooboo.Web.Html
{
    public static class HtmlUtility
    {
        public static string StylesheetLink(string href)
        {
            return StylesheetLink("Stylesheet", href);
        }

        public static string StylesheetLink(string @ref, string href)
        {
            return string.Format("<link rel=\"{0}\" href=\"{1}\" />", @ref, Url.UrlUtility.ResolveUrl(href));
        }

        public static string RemoveComment(string html)
        {
            return Regex.Replace(html, @"<!--[\w\W]*?-->", String.Empty);
        }

        public static string RemoveTag(string html)
        {
            return Regex.Replace(html, @"<[\w\W]*?>", String.Empty);
        }

        public static string ConvertToHtml(string text)
        {
            return text.Replace(Environment.NewLine, "<br />");
        }
    }
}
