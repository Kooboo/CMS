#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Persistence;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Models.Options;
using Kooboo.Common.Globalization;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kooboo.CMS.Common;
using Kooboo.Common.Web;
using Kooboo.Common.IO;
using Kooboo.CMS.SiteKernel.Models;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ISiteProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<Site>))]
    public class SiteProvider : ISiteProvider
    {
        #region Static   
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        #endregion

        #region .ctor
        IBaseDir baseDir;
        IMembershipProvider _membershipProvider;
        ISiteExportableProvider[] _exportableProivders;
        RepositoryManager _repositoryManager;
        public SiteProvider(IBaseDir baseDir, IMembershipProvider membershipProvider, ISiteExportableProvider[] exportableProivders, RepositoryManager repositoryManager)
        {
            this.baseDir = baseDir;
            this._membershipProvider = membershipProvider;
            this._exportableProivders = exportableProivders;
            _repositoryManager = repositoryManager;
        }
        #endregion

        #region IProvider<Site> Members

        [Obsolete("The method do not supported in SiteProvider.")]
        public IQueryable<Models.Site> All(Site site)
        {
            throw new NotSupportedException("The method do not supported in SiteProvider.");
        }

        public virtual void Add(Models.Site item)
        {
            Save(item);
        }

        private static void Save(Models.Site item)
        {
            locker.EnterWriteLock();
            try
            {
                IOUtility.EnsureDirectoryExists(item.PhysicalPath);
                Serialization.Serialize(item, item.DataFile);
                ((IPersistable)item).OnSaved();
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        public virtual void Update(Models.Site @new, Models.Site old)
        {
            Save(@new);
        }

        public virtual void Remove(Models.Site item)
        {
            locker.EnterWriteLock();
            try
            {
                IOUtility.DeleteDirectory(item.PhysicalPath, true);
                ClearSiteData(item);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        private void ClearSiteData(Site site)
        {
            //try
            //{
            //    _elementRepositoryFactory.CreateRepository(site).Clear();
            //}
            //catch
            //{
            //}
            try
            {
                Kooboo.CMS.Sites.Persistence.Providers.PageProvider.Clear(site);
            }
            catch
            {
            }
            try
            {
                Kooboo.CMS.Sites.Persistence.Providers.HtmlBlockProvider.Clear(site);
            }
            catch
            {
            }
        }

        public virtual Site Get(Site dummyObject)
        {
            if (!dummyObject.Exists())
            {
                return null;
            }
            locker.EnterReadLock();
            try
            {
                var site = Serialization.DeserializeSettings<Site>(dummyObject.DataFile);
                ((IPersistable)site).Init(dummyObject);
                return site;
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        #endregion

        #region Query Members
        private class DomainMappingComparer : IComparer<DomainMapping>
        {
            public int Compare(DomainMapping x, DomainMapping y)
            {
                if (x.FullDomain.EqualsOrNullEmpty(y.FullDomain, StringComparison.InvariantCultureIgnoreCase) && x.UserAgent.EqualsOrNullEmpty(y.UserAgent, StringComparison.InvariantCultureIgnoreCase))
                {
                    return 0;
                }
                //取反，从长到短排序
                var result = -(x.FullDomain.Length.CompareTo(y.FullDomain.Length));
                if (result == 0)
                {
                    result = x.FullDomain.CompareTo(y.FullDomain);
                    if (result == 0)
                    {
                        //有useragent的优先匹配
                        result = -(x.UserAgent.CompareTo(y.UserAgent));
                    }
                }
                return result;
            }
        }
        public virtual IEnumerable<DomainMapping> GetDomainTable()
        {
            SortedSet<DomainMapping> domainList = new SortedSet<DomainMapping>(new DomainMappingComparer());
            //SortedDictionary<string, Site> table = new SortedDictionary<string, Site>(new StringLengthComparer());
            foreach (var site in AllSites())
            {
                var siteObject = Get(site);
                foreach (var domain in siteObject.FullDomains)
                {
                    domainList.Add(new DomainMapping(domain, siteObject.UserAgent, siteObject));
                }
            }

            return domainList;// new SortedDictionary<string, Site>(table, new StringLengthComparer());

        }
        public virtual Site GetSiteByHostNameNPath(string hostName, string requestPath)
        {
            var domainTable = GetDomainTable();
            var fullPath = hostName + "/" + requestPath.Trim('/') + "/";
            return domainTable.Where(it => fullPath.StartsWith(it.FullDomain, StringComparison.OrdinalIgnoreCase))
                .Select(it => it.SiteObject).FirstOrDefault();
        }

        //private Site GetSiteByPredicate(Func<Site, bool> predicate)
        //{
        //    foreach (var site in Providers.SiteProvider.AllSites())
        //    {
        //        if (site.Exists())
        //        {
        //            var detail = site.AsActual();
        //            if (predicate(detail))
        //            {
        //                return detail;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public Site GetSiteByHostName(string hostName)
        //{
        //    return GetSiteByPredicate(it => (it.Domains != null && it.Domains.Length > 0) &&
        //        it.Domains.Contains(hostName, StringComparer.OrdinalIgnoreCase) && string.IsNullOrEmpty(it.SitePath));
        //}

        IEnumerable<Site> ISiteElementProvider<Site>.All(Site site)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Site> All()
        {
            return AllRootSites();
        }
        public virtual IEnumerable<Site> AllSites()
        {
            return CascadedChildSites(null);
        }
        private IEnumerable<Site> CascadedChildSites(Site site)
        {
            var childSites = ChildSites(site);
            foreach (var child in childSites.AsEnumerable())
            {
                childSites = childSites.Concat(CascadedChildSites(child));
            }
            return childSites;
        }

        public virtual IEnumerable<Site> ChildSites(Site site)
        {
            List<Site> list = new List<Site>();
            //if the site is null, get the root sites.
            string baseDir = Site.RootBasePhysicalPath;
            if (site != null)
            {
                baseDir = site.ChildSitesBasePhysicalPath;
            }
            if (Directory.Exists(baseDir))
            {
                foreach (var dir in IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    if (File.Exists(Path.Combine(dir.FullName, PathResource.SettingFileName)))
                    {
                        list.Add(new Site(site, dir.Name));
                    }
                }
            }
            return list;
        }

        public virtual IEnumerable<Site> AllRootSites()
        {
            return ChildSites(null);
        }

        #endregion

        #region Online/Offline

        public virtual void Offline(Site site)
        {
            var offlineFile = GetOfflineFile(site);
            if (!File.Exists(offlineFile))
            {
                File.WriteAllText(offlineFile, "The site is offline, please remove this file to take it online.".Localize());
            }
        }
        public virtual void Online(Site site)
        {
            var offlineFile = GetOfflineFile(site);
            if (File.Exists(offlineFile))
            {
                File.Delete(offlineFile);
            }
        }
        public virtual bool IsOnline(Site site)
        {
            var offlineFile = GetOfflineFile(site);
            return !File.Exists(offlineFile);
        }
        private string GetOfflineFile(Site site)
        {
            return Path.Combine(site.PhysicalPath, Offline_File);
        }

        #endregion

        #region Create
        /// <summary>
        /// 1. Extract the site files.
        /// 2. Create and initialize the repository if the repository doest not exsits.
        /// </summary>
        /// <param name="parentSite"></param>
        /// <param name="siteName"></param>
        /// <param name="packageStream"></param>
        /// <returns></returns>
        public virtual Site Create(Site parentSite, string siteName, System.IO.Stream packageStream, CreateSiteOptions options)
        {
            Site site = new Site(parentSite, siteName);
            if (site.Exists())
            {
                throw new Exception("The site already exists.".Localize());
            }
            try
            {
                if (packageStream == null)
                {
                    CreateSubSite(parentSite, siteName, options);
                }
                else
                {
                    CreateSiteUsingPackage(site, packageStream, options);
                }
            }
            catch (Exception ex)
            {
                Kooboo.Common.Logging.Logger.Error(ex.Message, ex);

                Kooboo.Common.IO.IOUtility.DeleteDirectory(site.PhysicalPath, true);

                throw;
            }

            return site;
        }

        private void CreateSubSite(Site parentSite, string siteName, CreateSiteOptions options)
        {
            if (parentSite == null)
            {
                throw new Exception("Parent site is required when create a sub site.".Localize());
            }
            parentSite = Get(parentSite);

            var newSite = new Site(parentSite, siteName);
            newSite.Repository = options.RepositoryName;
            newSite.Culture = options.Culture;
            newSite.TimeZoneId = options.TimeZoneId;

            // Set the same settings with parent.
            newSite.Theme = parentSite.Theme;
            newSite.EnableJquery = parentSite.EnableJquery;
            newSite.EnableStyleEdting = parentSite.EnableStyleEdting;
            newSite.EnableVersioning = parentSite.EnableVersioning;
            newSite.InlineEditing = parentSite.InlineEditing;
            newSite.CustomFields = parentSite.CustomFields;
            newSite.Smtp = parentSite.Smtp;
            newSite.Membership = parentSite.Membership;


            Add(newSite);

            if (!string.IsNullOrEmpty(options.RepositoryName))
            {
                CopyRepository(parentSite.GetRepository(), options.RepositoryName);
            }
        }

        private void CopyRepository(Kooboo.CMS.Content.Models.Repository sourceRepository, string name)
        {
            var repositoryManager = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager;
            var repository = repositoryManager.Get(name);

            if (repository == null)
            {
                repositoryManager.Copy(sourceRepository, name);
            }
        }

        private void CreateSiteUsingPackage(Site site, System.IO.Stream packageStream, CreateSiteOptions options)
        {
            using (ZipFile zipFile = ZipFile.Read(packageStream))
            {
                var action = ExtractExistingFileAction.OverwriteSilently;
                zipFile.ExtractAll(site.PhysicalPath, action);

                site = Get(site);

                CreateRepository(site, options);
                CreateMembership(site, options);

                if (site.Parent == null)
                {
                    UpdateFileLink(site);
                }
            }



            Initialize(site);
            Online(site);

            if (!string.IsNullOrEmpty(options.Culture))
            {
                site.Culture = options.Culture;
            }
            if (!string.IsNullOrEmpty(options.TimeZoneId))
            {
                site.TimeZoneId = options.TimeZoneId;
            }
            Save(site);
        }

        private void UpdateFileLink(Site site)
        {
            string sitesBaseVirtualPath = "/" + baseDir.Cms_DataPathName + "/Sites";
            string siteFilePathPattern = sitesBaseVirtualPath + "/[^/]+/";
            string siteFileReplacement = sitesBaseVirtualPath + "/" + (site.Name ?? "") + "/";

            foreach (var file in Kooboo.Common.IO.IOUtility.EnumerateFiles(site.PhysicalPath, new[] { "*.cshtml", "*.html", "*.xml" }, SearchOption.AllDirectories))
            {
                ReplaceFile(file, siteFilePathPattern, siteFileReplacement);
            }
        }
        private void ReplaceFile(string filePath, string pattern, string replacement)
        {
            string fileBody = IOUtility.ReadAsString(filePath);
            fileBody = Regex.Replace(fileBody, pattern, replacement, RegexOptions.IgnoreCase);
            IOUtility.SaveStringToFile(filePath, fileBody);
        }
        private void CreateRepository(Site site, CreateSiteOptions options)
        {
            var repositoryName = Get(site).Repository;
            //如果用户没有指定新的内容名称，直接使用站点原来使用的名称。（有可能是导入的情况）
            if (!string.IsNullOrEmpty(options.RepositoryName))
            {
                //如果有父站点，新的子站点的内容名称使用 指定的名称+"_"+子站点的名称
                if (site.Parent != null)
                {
                    repositoryName = options.RepositoryName + "_" + site.Name;
                }
                else
                {
                    repositoryName = options.RepositoryName;
                }
            }
            site.Repository = repositoryName;
            if (!string.IsNullOrEmpty(repositoryName))
            {
                var repository = new Repository(repositoryName).AsActual();
                if (repository == null)
                {
                    var repositoryFile = GetSiteRelatedFile(site, new[] { ContentDatabaseFileName, site.Repository });
                    if (!string.IsNullOrEmpty(repositoryFile) && File.Exists(repositoryFile))
                    {
                        using (FileStream fs = new FileStream(repositoryFile, FileMode.Open, FileAccess.Read))
                        {
                            _repositoryManager.Create(repositoryName, fs);
                        }
                        try
                        {
                            File.Delete(repositoryFile);
                        }
                        catch (Exception e)
                        {
                            Kooboo.Common.Logging.Logger.Error(e.Message, e);
                        }
                        site.Repository = repositoryName;
                    }
                    else if (site.Parent != null && !string.IsNullOrEmpty(site.Repository))
                    {
                        site.Repository = Get(site.Parent).Repository;
                    }
                }
            }
            //Save the repository change.
            Save(site);
            foreach (var childSite in ChildSites(site))
            {
                CreateRepository(childSite, options);
            }
        }
        private void CreateMembership(Site site, CreateSiteOptions options)
        {
            site = Get(site);

            var membershipName = site.Membership;
            //如果用户没有指定新的Membership名称，直接使用站点原来使用的名称。（有可能是导入的情况）
            if (!string.IsNullOrEmpty(options.MembershipName))
            {
                //如果有父站点，新的子站点的Membership名称使用 指定的名称+"_"+子站点的名称
                if (site.Parent != null)
                {
                    membershipName = options.MembershipName + "_" + site.Name;
                }
                else
                {
                    membershipName = options.MembershipName;
                }
            }
            site.Membership = membershipName;
            if (!string.IsNullOrEmpty(membershipName))
            {
                var membership = new Kooboo.CMS.Membership.Models.Membership(membershipName).AsActual();
                if (membership == null)
                {
                    var membershipFile = GetSiteRelatedFile(site, MembershipFileName);
                    if (!string.IsNullOrEmpty(membershipFile) && File.Exists(membershipFile))
                    {
                        using (FileStream fs = new FileStream(membershipFile, FileMode.Open, FileAccess.Read))
                        {
                            _membershipProvider.Import(membershipName, fs);
                        }
                        try
                        {
                            File.Delete(membershipFile);
                        }
                        catch (Exception e)
                        {
                            Kooboo.Common.Logging.Logger.Error(e.Message, e);
                        }
                    }
                    else if (site.Parent != null && !string.IsNullOrEmpty(site.Membership))
                    {
                        site.Membership = Get(site.Parent).Membership;
                    }
                    else
                    {
                        site.Membership = null;
                    }
                }
            }
            Save(site);
            foreach (var childSite in ChildSites(site))
            {
                CreateMembership(childSite, options);
            }
        }

        //private string GetChildSiteRepositoryName(Site childSite, string newRepositoryName)
        //{
        //    childSite = childSite.AsActual();
        //    if (Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(childSite.Repository) == null)
        //    {
        //        return childSite.Repository;
        //    }
        //    else
        //    {
        //        return newRepositoryName + "_" + childSite.Name;
        //    }
        //}

        public virtual void Initialize(Site site)
        {
            if (_exportableProivders != null)
            {
                foreach (var exportProvider in _exportableProivders)
                {
                    exportProvider.InitializeToDB(site);
                }
            }

            foreach (var sub in ChildSites(site))
            {
                Initialize(Get(sub));
            }
        }

        #endregion

        #region Export

        private void ExportSiteElements(Site site, bool includeSubSites)
        {
            if (_exportableProivders != null)
            {
                foreach (var exportProvider in _exportableProivders)
                {
                    exportProvider.ExportToDisk(site);
                }
            }

            if (includeSubSites)
            {
                foreach (var sub in ChildSites(site))
                {
                    ExportSiteElements(Get(sub), includeSubSites);
                }
            }
        }
        /// <summary>
        /// 1. export repository as a zip file.
        /// 2. offline the site.
        /// 3. zip the site directory as a zip file.
        /// 4. online the site.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="outputStream"></param>
        public virtual void Export(Site site, System.IO.Stream outputStream, bool includeDatabase, bool includeSubSites)
        {
            ISiteProvider siteProvider = Providers.SiteProvider;

            //export the data to disk.
            ExportSiteElements(site, includeSubSites);

            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                //zipFile.ZipError += new EventHandler<ZipErrorEventArgs>(zipFile_ZipError);
                var ignores = "name != *\\~versions\\*.* and name != *\\.svn\\*.* and name != *\\_svn\\*.*";
                var dirs = Directory.EnumerateDirectories(site.PhysicalPath);
                if (!includeSubSites)
                {
                    dirs = dirs.Where(it => !Path.GetFileName(it).EqualsOrNullEmpty("Sites", StringComparison.OrdinalIgnoreCase));
                }
                foreach (var dir in dirs)
                {
                    zipFile.AddSelectedFiles(ignores, dir, Path.GetFileName(dir), true);
                }
                zipFile.AddFiles(Directory.EnumerateFiles(site.PhysicalPath), "");

                if (includeDatabase)
                {
                    ExportSiteRepository(site, site, zipFile, includeSubSites);
                    ExportSiteMembership(site, site, zipFile, includeSubSites);
                }

                zipFile.ZipErrorAction = ZipErrorAction.Skip;

                zipFile.Save(outputStream);
            }
        }

        private void ExportSiteRepository(Site rootSiteExported, Site site, ZipFile zipFile, bool includeSubSites)
        {
            site = Get(site);

            if (!string.IsNullOrEmpty(site.Repository))
            {
                if (site.Parent == null || (site.Parent != null
                    && !Get(site.Parent).Repository.EqualsOrNullEmpty(site.Repository, StringComparison.CurrentCultureIgnoreCase)))
                {
                    MemoryStream ms = new MemoryStream();

                    Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Export(site.Repository, ms);

                    ms.Position = 0;

                    var entryName = GetSiteRelatedEntryName(rootSiteExported, site, ContentDatabaseFileName);

                    if (zipFile.ContainsEntry(entryName))
                    {
                        zipFile.RemoveEntry(entryName);
                    }
                    zipFile.AddEntry(entryName, ms);
                }
            }
            if (includeSubSites)
            {
                foreach (var childSite in ChildSites(site))
                {
                    ExportSiteRepository(rootSiteExported, childSite, zipFile, includeSubSites);
                }
            }
        }

        private void ExportSiteMembership(Site rootSiteExported, Site site, ZipFile zipFile, bool includeSubSites)
        {
            site = Get(site);

            if (!string.IsNullOrEmpty(site.Membership))
            {
                if (site.Parent == null || (site.Parent != null
                    && !Get(site.Parent).Membership.EqualsOrNullEmpty(site.Membership, StringComparison.CurrentCultureIgnoreCase)))
                {
                    MemoryStream ms = new MemoryStream();

                    _membershipProvider.Export(site.GetMembership(), ms);

                    if (ms.Length > 0)
                    {
                        ms.Position = 0;

                        var entryName = GetSiteRelatedEntryName(rootSiteExported, site, MembershipFileName);

                        if (zipFile.ContainsEntry(entryName))
                        {
                            zipFile.RemoveEntry(entryName);
                        }
                        zipFile.AddEntry(entryName, ms);
                    }

                }
            }
            if (includeSubSites)
            {
                foreach (var childSite in ChildSites(site))
                {
                    ExportSiteMembership(rootSiteExported, childSite, zipFile, includeSubSites);
                }
            }
        }
        private static string GetSiteRelatedEntryName(Site rootSiteExported, Site site, string entryName)
        {
            entryName = entryName + ".zip";
            if (site.Parent != null)
            {
                //单独导出子站点的时候，目录需要移除的是导出站点路径
                var removeLength = rootSiteExported.VirtualPath.Length + 1;
                if (site.VirtualPath.Length > removeLength)
                {
                    entryName = UrlUtility.Combine(site.VirtualPath.Remove(0, removeLength), entryName);
                }
            }
            return entryName;
        }
        private static string GetSiteRelatedFile(Site site, params string[] entryNames)
        {
            foreach (var name in entryNames)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var entryName = name + ".zip";
                    entryName = Path.Combine(site.PhysicalPath, entryName);
                    if (File.Exists(entryName))
                    {
                        return entryName;
                    }
                }
            }
            return null;
        }
        #endregion

        #region The ISiteProvider must to be refactored. It can not inherit from ISiteElementProvider.
        public void InitializeToDB(Site site)
        {
            //throw new NotImplementedException();
        }

        public void ExportToDisk(Site site)
        {
            //throw new NotImplementedException();
        }
        #endregion
    }
}
