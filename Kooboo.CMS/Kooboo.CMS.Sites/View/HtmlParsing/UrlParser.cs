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
using System.Web.Routing;

namespace Kooboo.CMS.Sites.View.HtmlParsing
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IParser), Key = "url")]
    public class UrlParser : IParser
    {
        IPageUrlGenerator _pageUrlGenerator;
        public UrlParser(IPageUrlGenerator pageUrlGenerator)
        {
            _pageUrlGenerator = pageUrlGenerator;
        }
        public string Tag
        {
            get { return "url"; }
        }

        public string Parse(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                RouteValueDictionary routes = new RouteValueDictionary();
                var list = parameters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                var pageName = list[0].Trim();
                foreach (var item in list.Skip(1))
                {
                    var keyValue = Split(item);
                    if (!string.IsNullOrEmpty(keyValue.Key))
                    {
                        routes[keyValue.Key] = keyValue.Value;
                    }
                }

                return _pageUrlGenerator.PageUrl(pageName, routes);
            }
            return null;
        }

        private KeyValuePair<string, string> Split(string s)
        {
            var keyValue = new KeyValuePair<string, string>();
            if (s.Contains('='))
            {
                var arr = s.Split('=');
                keyValue = new KeyValuePair<string, string>(arr[0].Trim(), arr[1].Trim());
            }
            return keyValue;
        }
    }
}
