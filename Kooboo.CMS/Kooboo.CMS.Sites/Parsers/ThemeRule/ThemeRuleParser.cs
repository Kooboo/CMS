using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Parsers.ThemeRule
{
    public static class ThemeRuleParser
    {
        public static IThemeRuleParser Parser = new RegularCssHackFileParser();
        public static IEnumerable<ThemeFile> Parse(Theme theme, out string themeRuleBody, string baseUri=null)
        {
            theme = theme.LastVersion();
            IEnumerable<ThemeFile> themeFiles = Persistence.Providers.ThemeProvider.AllStyles(theme);
            ThemeRuleFile cssHackFile = Persistence.Providers.ThemeProvider.GetCssHack(theme);
            if (cssHackFile == null || !cssHackFile.Exists())
            {
                themeRuleBody = "";
                return themeFiles;
            }

            var themeRuleFiles = Parser.Parse(cssHackFile.Read(), (fileVirtualPath) => UrlUtility.ToHttpAbsolute(baseUri, new ThemeFile(theme, fileVirtualPath).LastVersion().VirtualPath), out themeRuleBody);

            return themeFiles.Where(it => !themeRuleFiles.Any(cf => cf.EqualsOrNullEmpty(it.FileName, StringComparison.CurrentCultureIgnoreCase)));
        }
    }
}
