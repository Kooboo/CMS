using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System.IO;
using Kooboo.CMS.Sites.Parsers.ThemeRule;
using Ionic.Zip;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Account.Models;
using Kooboo.Web.Mvc.Menu;

namespace Kooboo.CMS.Sites.Services
{
    public class ModuleManager
    {
        #region Module Script & theme
        private ModuleEntryPath GetThemesBasePath(string moduleName)
        {
            return new ModuleEntryPath(moduleName, "Themes");
        }
        private ModuleEntryPath GetThemePath(string moduleName, string themeName)
        {
            return new ModuleEntryPath(GetThemesBasePath(moduleName), themeName);
        }
        public virtual IEnumerable<string> AllThemes(string moduleName)
        {
            var themesPath = GetThemesBasePath(moduleName).PhysicalPath;
            if (Directory.Exists(themesPath))
            {
                foreach (var dir in Directory.EnumerateDirectories(themesPath))
                {
                    yield return Path.GetFileName(dir);
                }
            }
        }
        public virtual IEnumerable<ModuleEntryPath> AllScripts(string moduleName)
        {
            ModuleEntryPath scriptsPath = new ModuleEntryPath(moduleName, "Scripts");
            if (Directory.Exists(scriptsPath.PhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(scriptsPath.PhysicalPath, "*.js"))
                {
                    yield return new ModuleEntryPath(scriptsPath, Path.GetFileName(file));
                }
            }
        }
        public virtual IEnumerable<ModuleEntryPath> AllThemeFiles(string moduleName, string themeName, out string themeRuleBody)
        {
            ModuleEntryPath themePath = GetThemePath(moduleName, themeName);
            List<ModuleEntryPath> themeFiles = new List<ModuleEntryPath>();
            themeRuleBody = "";
            if (Directory.Exists(themePath.PhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(themePath.PhysicalPath, "*.css"))
                {
                    themeFiles.Add(new ModuleEntryPath(themePath, Path.GetFileName(file)));
                }

                string themeBaseUrl = Kooboo.Web.Url.UrlUtility.ResolveUrl(themePath.VirtualPath);

                var themeRuleFiles = ThemeRuleParser.Parser.Parse(ThemeRuleBody(moduleName, themeName),
                    (fileVirtualPath) => Kooboo.Web.Url.UrlUtility.Combine(themeBaseUrl, fileVirtualPath), out themeRuleBody);

                return themeFiles.Where(it => !themeRuleFiles.Any(cf => cf.EqualsOrNullEmpty(it.EntryName, StringComparison.CurrentCultureIgnoreCase)));
            }
            return new ModuleEntryPath[0];

        }
        private string ThemeRuleBody(string moduleName, string themeName)
        {
            ModuleEntryPath themePath = GetThemePath(moduleName, themeName);
            ModuleEntryPath themeRuleFile = new ModuleEntryPath(themePath, "Theme.rule");
            if (File.Exists(themeRuleFile.PhysicalPath))
            {
                return File.ReadAllText(themeRuleFile.PhysicalPath);
            }
            return string.Empty;
        }
        #endregion

        #region Management

        //public bool Verify(string moduleName)
        //{
        //    if (!File.Exists(RouteTables.GetRoutesFilePath(moduleName).PhysicalPath))
        //    {
        //        return false;
        //    }
        //    if (!File.Exists(ModuleInfo.GetModuleInfoPath(moduleName).PhysicalPath))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public bool Verify(ZipFile zipFile)
        //{
        //    var moduleConfigEntry = zipFile[RouteTables.RouteFile];
        //    var routesConfigEntry = zipFile[ModuleInfo.ModuleInfoFileName];
        //    if (moduleConfigEntry == null || routesConfigEntry == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public void Upload(string moduleName, Stream zipStream, bool @override)
        //{
        //    if (!@override && All().Any(it => it.EqualsOrNullEmpty(moduleName, StringComparison.CurrentCultureIgnoreCase)))
        //    {
        //        throw new KoobooException(string.Format("The module '{0}' already exists.", moduleName));
        //    }

        //    ModulePath modulePath = new ModulePath(moduleName);
        //    using (ZipFile zipFile = ZipFile.Read(zipStream))
        //    {
        //        if (!Verify(zipFile))
        //        {
        //            throw new KoobooException("The module is invalid.");
        //        }
        //        var webconfigEntry = zipFile["web.config"];
        //        if (webconfigEntry != null)
        //        {
        //            zipFile.RemoveEntry(webconfigEntry);
        //        }

        //        zipFile.ExtractAll(modulePath.PhysicalPath, ExtractExistingFileAction.OverwriteSilently);
        //    }
        //}

        public virtual IEnumerable<string> All()
        {
            var baseDirectory = ModulePath.BaseDirectory;
            if (Directory.Exists(baseDirectory))
            {
                foreach (var dir in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDirectory))
                {
                    var moduleName = dir.Name;

                    var moduleConfigFile = new ModuleEntryPath(moduleName, ModuleInfo.ModuleInfoFileName);

                    if (File.Exists(moduleConfigFile.PhysicalPath))
                    {
                        yield return moduleName;
                    }
                }
            }
        }

        public virtual IEnumerable<ModuleInfo> AllModuleInfo()
        {
            foreach (var name in All())
            {
                yield return Get(name);
            }
        }

        public virtual ModuleInfo Get(string moduleName)
        {
            return ModuleInfo.Get(moduleName);
        }

        public virtual ModuleInfo Install(string moduleName, Stream moduleStream, ref StringBuilder log)
        {
            return ModuleInstaller.Install(moduleName, moduleStream, ref log);
        }

        public virtual void Uninstall(string moduleName)
        {
            ModuleUninstaller.Uninstall(moduleName);
        }

        #endregion

        #region Site&Module relation
        private static class ModuleData
        {
            static System.Threading.ReaderWriterLockSlim sitesModuleRelationLocker = new System.Threading.ReaderWriterLockSlim();
            public static List<string> GetSitesInModule(string moduleName)
            {
                var filePath = GetSitesModuleRelationDataFile(moduleName);
                if (!File.Exists(filePath))
                {
                    return new List<string>();
                }
                sitesModuleRelationLocker.EnterReadLock();
                try
                {
                    var list = Serialization.DeserializeSettings<List<string>>(filePath);
                    return list;
                }
                finally
                {
                    sitesModuleRelationLocker.ExitReadLock();
                }
            }
            public static void SaveSitesInModule(string moduleName, List<string> sites)
            {
                var filePath = GetSitesModuleRelationDataFile(moduleName);
                sitesModuleRelationLocker.EnterWriteLock();
                try
                {
                    Serialization.Serialize(sites, filePath);
                }
                finally
                {
                    sitesModuleRelationLocker.ExitWriteLock();
                }
            }
            private static string GetSitesModuleRelationDataFile(string moduleName)
            {
                ModuleEntryPath entryPath = new ModuleEntryPath(moduleName, "Sites.config");
                return entryPath.PhysicalPath;
            }
        }
        public virtual void AddSiteToModule(string moduleName, string siteName)
        {
            var list = ModuleData.GetSitesInModule(moduleName);
            if (!list.Contains(siteName, StringComparer.OrdinalIgnoreCase))
            {
                list.Add(siteName);
                ModuleData.SaveSitesInModule(moduleName, list);
            }
        }
        public virtual void RemoveSiteFromModule(string moduleName, string siteName)
        {
            var list = ModuleData.GetSitesInModule(moduleName);
            list.RemoveAll(s => s.EqualsOrNullEmpty(siteName, StringComparison.OrdinalIgnoreCase));
            ModuleData.SaveSitesInModule(moduleName, list);
        }
        public virtual IEnumerable<string> AllSitesInModule(string moduleName)
        {
            return ModuleData.GetSitesInModule(moduleName);
        }
        public virtual bool SiteIsInModule(string moduleName, string siteName)
        {
            return ModuleData.GetSitesInModule(moduleName).Contains(siteName, StringComparer.OrdinalIgnoreCase);
        }
        public virtual IEnumerable<string> AllModulesForSite(string siteName)
        {
            foreach (var moduleName in All())
            {
                if (SiteIsInModule(moduleName, siteName))
                {
                    yield return moduleName;
                }
            }
        }
        #endregion

        #region Module Permissions
        public IEnumerable<Permission> GetModulePermissions()
        {
            var modules = All();
            List<Permission> permissions = new List<Permission>();
            foreach (var moduleName in modules)
            {
                var menuTemplate = MenuFactory.GetMenuTemplate(moduleName);
                if (menuTemplate!=null)
                {
                    foreach (var item in menuTemplate.ItemContainers)
                    {
                        if (item is MenuItemTemplate)
                        {
                            FetchPermissions(moduleName, (MenuItemTemplate)item, permissions);
                        }
                    }
                }
               
            }
            return permissions;
        }
        private void FetchPermissions(string moduleName, MenuItemTemplate menuItemTemplate, List<Permission> permissions)
        {
            if (menuItemTemplate.ReadOnlyProperties != null)
            {
                var permissionGroup = menuItemTemplate.ReadOnlyProperties["permissionGroup"];
                var permissionName = menuItemTemplate.ReadOnlyProperties["PermissionName"];
                if (!string.IsNullOrEmpty(permissionName))
                {
                    permissions.Add(new Permission() { AreaName = moduleName, Group = permissionGroup, Name = permissionName, DisplayName = permissionName });
                }
            }
            foreach (var item in menuItemTemplate.ItemContainers)
            {
                if (item is MenuItemTemplate)
                {
                    FetchPermissions(moduleName, (MenuItemTemplate)item, permissions);
                }
            }
        }
        #endregion
    }
}
