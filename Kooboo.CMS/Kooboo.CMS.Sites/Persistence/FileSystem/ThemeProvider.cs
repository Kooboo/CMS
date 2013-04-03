using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.Web.Mvc.WebResourceLoader;
using System.ComponentModel.Composition;
using Kooboo.IO;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Export(typeof(IThemeProvider))]
    public class ThemeProvider : IThemeProvider
    {
        #region IThemeRepository Members
        public IQueryable<StyleFile> AllStyles(Theme theme)
        {
            return AllStylesEnumerable(theme.Site, theme.Name).AsQueryable();
        }
        public IEnumerable<StyleFile> AllStylesEnumerable(Site site, string themeName)
        {
            var theme = new Theme(site, themeName);

            var fileNames = EnumerateCssFilesWithPath(site, themeName);

            fileNames = FileOrderHelper.OrderFiles(GetOrderFile(site, themeName), fileNames);

            return fileNames.Select(it => new StyleFile(theme, it).LastVersion());

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
            foreach (var file in Directory.EnumerateFiles(dir, "*.css"))
            {
                list.Add(Path.GetFileName(file));
            }
            return list;
        }

        public ThemeRuleFile GetCssHack(Theme theme)
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
        public IQueryable<ThemeImageFile> AllImages(Theme theme)
        {
            return AllImagesEnumerable(theme).AsQueryable();
        }

        public IEnumerable<ThemeImageFile> AllImagesEnumerable(Theme theme)
        {
            List<ThemeImageFile> list = new List<ThemeImageFile>();
            theme = theme.LastVersion();
            if (theme.Exists())
            {
                ThemeImageFile dummy = new ThemeImageFile(theme, "dummy");
                var baseDir = dummy.BasePhysicalPath;
                if (Directory.Exists(baseDir))
                {
                    foreach (var file in Directory.EnumerateFiles(baseDir))
                    {
                        list.Add(new ThemeImageFile(theme, Path.GetFileName(file)));
                    }
                }
            }
            return list;
        }


        public IQueryable<Theme> All(Models.Site site)
        {
            return IInheritableHelper.All<Theme>(site).AsQueryable();
        }


        public Theme Get(Site site, string name)
        {
            var all = this.All(site);
            return all.Where(o => o.Name == name && o.Site == site).FirstOrDefault();
        }


        #endregion

        public void Add(Theme item)
        {
            IOUtility.EnsureDirectoryExists(item.PhysicalPath);
        }

        public void Update(Theme @new, Theme old)
        {

        }

        public void Remove(Theme item)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(item.PhysicalPath);
            di.Delete(true);
        }

        #region IRepository<Theme> Members


        public Theme Get(Theme dummy)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IImportRepository Members

        public void Export(IEnumerable<Theme> sources, System.IO.Stream outputStream)
        {
            ImportHelper.Export(sources, outputStream);
        }
        public void Import(Site site, string destDir, System.IO.Stream zipStream, bool @override)
        {
            ImportHelper.Import(site, destDir, zipStream, @override);
        }
        #endregion
    }
}
