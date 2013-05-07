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
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.View.HtmlParsing
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IHtmlParser))]
    public class HtmlParser : IHtmlParser
    {
        public IParsers Parsers { get; set; }
        public HtmlParser(IParsers parsers)
        {
            this.Parsers = parsers;
        }
        static Regex syntaxRegex = new Regex("\\[\\[(.+):(.+)\\]\\]");
        public string Parse(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }
            StringBuilder sb = new StringBuilder(html);
            Dictionary<string, string> syntaxs = new Dictionary<string, string>();


            foreach (Match match in syntaxRegex.Matches(html))
            {
                if (match.Groups.Count > 0)
                {
                    if (!syntaxs.ContainsKey(match.Value))
                    {
                        var tagType = match.Groups[1].Value;
                        var parser = Parsers.GetParser(tagType);
                        if (parser != null)
                        {
                            string parameters = null;
                            if (match.Groups.Count > 2)
                            {
                                parameters = match.Groups[2].Value;
                            }
                            var result = parser.Parse(parameters);

                            if (result != null)
                            {
                                syntaxs[match.Value] = result;
                            }
                        }
                    }
                }
                foreach (var item in syntaxs)
                {
                    sb.Replace(item.Key, item.Value);
                }
            }
            return sb.ToString();
        }
    }
}
