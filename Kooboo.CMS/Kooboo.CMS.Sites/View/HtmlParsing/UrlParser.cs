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
        internal static bool HasIllegalCharacters(string path, bool checkAdditional)
        {
            for (int i = 0; i < path.Length; i++)
            {
                int num2 = path[i];
                //0x22="  60=<  0x3e=> 0x7c=| 0x20= 
                if (((num2 == 0x22) || (num2 == 60)) || (((num2 == 0x3e) || (num2 == 0x7c)) || (num2 < 0x20)))
                {
                    return true;
                }
                //0x3f=? 0x2a=*
                if (checkAdditional && ((num2 == 0x3f) || (num2 == 0x2a)))
                {
                    return true;
                }
            }
            return false;
        }

        public string Parse(string parameters)
        {
            try
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

                    if (!HasIllegalCharacters(pageName, false))
                    {
                        return _pageUrlGenerator.PageUrl(pageName, routes);
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
                return null;
            }           
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
