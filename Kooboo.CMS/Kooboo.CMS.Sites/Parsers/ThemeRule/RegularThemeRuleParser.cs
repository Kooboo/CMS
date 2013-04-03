using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel.Composition;

namespace Kooboo.CMS.Sites.Parsers.ThemeRule
{
    /// <summary>
    /// Default implementation 
    /// </summary>
    public class RegularCssHackFileParser : IThemeRuleParser
    {
        static Regex regex = new Regex(@"href=""(?<VirualPath>[^""]+)""", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        #region ICssHackFileParser Members

        public IEnumerable<string> Parse(string cssHackBody, Func<string, string> replaceUrl, out string replacedCssHackBody)
        {            
            List<string> virtualPaths = new List<string>();
            MatchEvaluator virtualPathEvaluator = new MatchEvaluator(delegate(Match m)
            {
                var virtualPath = m.Groups["VirualPath"].Value;
                virtualPaths.Add(virtualPath);
                return string.Format(@"href=""{0}""", replaceUrl(virtualPath));
            });
            replacedCssHackBody = regex.Replace(cssHackBody, virtualPathEvaluator);

            return virtualPaths;
        }

        #endregion
    }
}
