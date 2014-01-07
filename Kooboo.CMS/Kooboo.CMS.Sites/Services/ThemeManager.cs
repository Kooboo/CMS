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
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Services
{
    public class ThemeBaseDirectory : DirectoryResource
    {
        public ThemeBaseDirectory(Site site)
            : base(site, Theme.PATH_NAME)
        {

        }
        protected ThemeBaseDirectory()
            : base()
        { }
        protected ThemeBaseDirectory(string physicalPath)
            : base(physicalPath)
        {
        }
        protected ThemeBaseDirectory(Site site, string name)
            : base(site, name)
        {
        }
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[0]; }
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            return relativePaths;
        }
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(FileManager), Key = "themes")]
    public class ThemeManager : FileManager
    {
        public override string Type
        {
            get { return "Themes"; }
        }
        protected override Models.DirectoryResource GetRootDir(Site site)
        {
            return new ThemeBaseDirectory(site);
        }
        #region Theme styles

        public virtual IEnumerable<StyleFile> AllStyles(Theme theme)
        {
            return AllStylesEnumerable(theme.Site, theme.Name).AsQueryable();
        }
        private string GetOrderFile(Site site, string themeName)
        {
            while (site != null)
            {
                var orderFile = FileOrderHelper.GetOrderFile(new Theme(site, themeName).PhysicalPath);
                if (File.Exists(orderFile))
                {
                    return orderFile;
                }
                site = site.Parent;
            }
            return null;
        }
        public virtual IEnumerable<StyleFile> AllStylesEnumerable(Site site, string themeName)
        {
            var theme = new Theme(site, themeName);

            var fileNames = EnumerateCssFilesWithPath(site, themeName);

            fileNames = FileOrderHelper.OrderFiles(GetOrderFile(site, themeName), fileNames);

            return fileNames.Select(it => new StyleFile(theme, it).LastVersion());

        }
        private IEnumerable<string> EnumerateCssFilesWithPath(Site site, string themeName)
        {
            List<string> results = new List<string>();

            while (site != null)
            {
                Theme theme = new Theme(site, themeName);
                var baseDir = theme.PhysicalPath;
                if (Directory.Exists(baseDir))
                {
                    var tempResults = EnumerateCssFiles(baseDir);
                    if (results.Count == 0)
                    {
                        results.AddRange(tempResults);
                    }
                    else
                    {
                        foreach (var item in tempResults)
                        {
                            if (!results.Any(it => Path.GetFileName(it).Equals(Path.GetFileName(item), StringComparison.InvariantCultureIgnoreCase)))
                            {
                                results.Add(item);
                            }
                        }
                    }
                }
                site = site.Parent;
            }
            return results;
        }
        private IEnumerable<string> EnumerateCssFiles(string dir)
        {
            List<string> list = new List<string>();
            foreach (var file in Kooboo.IO.IOUtility.EnumerateFilesByExtensions(dir, GetCssExtensions().ToArray()))
            {
                list.Add(Path.GetFileName(file));
            }
            return list;
        }

        private IEnumerable<string> GetCssExtensions()
        {
            List<string> list = new List<string>();
            list.Add(".css");
            var dynamicClientResource = Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll<Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource.IDynamicClientResource>().Where(it => it.ResourceType == Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource.ResourceType.Stylesheet);
            foreach (var item in dynamicClientResource)
            {
                list.AddRange(item.SupportedFileExtensions);
            }
            return list;
        }

        public virtual ThemeRuleFile GetCssHack(Theme theme)
        {
            return GetCssHack(theme.Site, theme.Name);
        }
        private ThemeRuleFile GetCssHack(Site site, string themeName)
        {
            while (site != null)
            {
                var theme = new Theme(site, themeName);
                var themeRuleFile = new ThemeRuleFile(theme);
                if (themeRuleFile.Exists())
                {
                    return themeRuleFile;
                }
                site = site.Parent;
            }
            return null;
        }

        #endregion

        public virtual IEnumerable<string> AllThemes(Site site)
        {
            List<string> list = new List<string>();
            while (site != null)
            {
                AllThemesBySite(site, ref list);
                site = site.Parent;
            }

            return list;
        }
        private void AllThemesBySite(Site site, ref List<string> list)
        {
            foreach (var dir in GetDirectories(site, ""))
            {
                if (!list.Contains(dir.Name))
                {
                    list.Add(dir.Name);
                }
            }
        }
    }
}
