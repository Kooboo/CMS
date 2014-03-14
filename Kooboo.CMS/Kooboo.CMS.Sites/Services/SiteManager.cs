#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Models.Options;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(SiteManager))]
    public class SiteManager : PathResourceManagerBase<Site, ISiteProvider>
    {
        #region .ctor
        RepositoryManager _repositoryManager;
        public SiteManager(ISiteProvider provider, RepositoryManager repositoryManager)
            : base(provider)
        {
            _repositoryManager = repositoryManager;
            if (!(provider is Kooboo.CMS.Sites.Persistence.Caching.SiteProvider))
            {
                throw new Exception("Expect caching provider");
            }
        }
        #endregion

        #region All
        public override IEnumerable<Site> All(Site site, string filterName)
        {
            throw new NotImplementedException("Use ChildSites.");
        }
        public virtual IEnumerable<Site> All()
        {
            return Provider.AllSites();
        }
        public virtual IEnumerable<Site> AllRootSites()
        {
            return Provider.AllRootSites();
        }
        #endregion

        #region ChildSites
        public virtual IEnumerable<Site> ChildSites(Site site)
        {
            var sites = Provider.ChildSites(site);

            foreach (var c in sites)
            {
                yield return Provider.Get(c);
            }
        }
        #endregion

        #region Get
        public override Site Get(Site site, string name)
        {
            return Provider.Get(site);
        }
        public virtual Site GetSite(IEnumerable<string> namePath)
        {
            if (namePath == null || namePath.Count() == 0)
            {
                throw new ArgumentNullException("namePath");
            }
            Site dummySite = new Site(namePath);
            return Provider.Get(dummySite);
        }
        #endregion

        #region Add
        public override void Add(Site parent, Site site)
        {
            site.Parent = parent;

            var query = Provider.Get(site);
            if (query != null)
            {
                throw new ItemAlreadyExistsException();
            }

            Provider.Add(site);
        }
        #endregion

        #region Update
        public virtual void Update(Site site)
        {
            Provider.Update(site, site);
        }
        #endregion

        #region Remove

        public override void Remove(Site site, Site o)
        {
            throw new NotSupportedException("Please instead of using Remove(Site site).");
        }
        public virtual void Delete(Site site, DeleteSiteOptions options)
        {
            Provider.Offline(site);

            List<string> associatedRepositories = new List<string>();

            GetAllRepositoriesAssociated(site, ref associatedRepositories);

            Provider.Remove(site);

            if (options.DeleteAssociatedRepository)
            {
                RemoveUnusedRepository(associatedRepositories.Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
            }
        }
        private void GetAllRepositoriesAssociated(Site site, ref List<string> repositories)
        {
            var repository = site.AsActual().Repository;
            if (!string.IsNullOrEmpty(repository))
            {
                repositories.Add(repository);
            }

            foreach (var item in ChildSites(site))
            {
                GetAllRepositoriesAssociated(item, ref repositories);
            }
        }
        private void RemoveUnusedRepository(IEnumerable<string> associatedRepositories)
        {
            var allSiteRepositories = All().Select(it => it.AsActual()).Where(it => !string.IsNullOrEmpty(it.Repository)).Select(it => it.Repository).ToArray();

            var unusedRepositories = associatedRepositories.Where(it => !allSiteRepositories.Any(inuse => inuse.EqualsOrNullEmpty(it, StringComparison.OrdinalIgnoreCase)));

            foreach (var item in unusedRepositories)
            {
                var repository = new Repository(item);
                _repositoryManager.Remove(repository);
            }
        }
        #endregion

        #region SiteTrees
        public virtual IEnumerable<SiteTree> SiteTrees(string userName = null)
        {
            return AllRootSites().Select(it => GetSiteNode(it, userName))
                .Where(it => it != null)
                .Select(it => new SiteTree() { Root = it });
        }
        protected virtual SiteNode GetSiteNode(Site site, string userName)
        {
            var visible = string.IsNullOrEmpty(userName) || ServiceFactory.UserManager.Authorize(site, userName);

            SiteNode siteNode = new SiteNode() { Site = site.AsActual() };

            siteNode.Children = ChildSites(site)
                .Select(it => GetSiteNode(it, userName))
                .Where(it => it != null);

            if (visible == true || siteNode.Children.Count() > 0)
            {
                return siteNode;
            }
            else
            {
                return null;
            }


        }
        #endregion

        #region Export/Import
        //[Obsolete]
        //private void CopyRepository(Kooboo.CMS.Content.Models.Repository sourceRepository, string name)
        //{
        //    var repositoryManager = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager;
        //    var repository = repositoryManager.Get(name);

        //    if (repository == null)
        //    {
        //        repositoryManager.Copy(sourceRepository, name);
        //    }
        //}
        //[Obsolete]
        //public virtual Site Create(Site parent, string siteName, string templateFullName, Site siteSetting, string userName = null)
        //{
        //    if (string.IsNullOrEmpty(templateFullName))
        //    {
        //        if (parent == null)
        //        {
        //            throw new KoobooException("Parent site is required.".Localize());
        //        }
        //        parent = parent.AsActual();

        //        siteSetting.Parent = parent;

        //        // Set the same settings with parent.
        //        siteSetting.Theme = parent.Theme;
        //        siteSetting.EnableJquery = parent.EnableJquery;
        //        siteSetting.EnableStyleEdting = parent.EnableStyleEdting;
        //        siteSetting.EnableVersioning = parent.EnableVersioning;
        //        siteSetting.InlineEditing = parent.InlineEditing;
        //        siteSetting.CustomFields = parent.CustomFields;
        //        siteSetting.Smtp = parent.Smtp;
        //        siteSetting.Membership = parent.Membership;

        //        Add(parent, siteSetting);

        //        if (!string.IsNullOrEmpty(siteSetting.Repository))
        //        {
        //            CopyRepository(parent.GetRepository(), siteSetting.Repository);
        //        }
        //        return siteSetting;
        //    }
        //    else
        //    {
        //        var template = new ItemTemplate(templateFullName);
        //        var itemTemplate = ServiceFactory.SiteTemplateManager.GetItemTemplate(template.Category, template.TemplateName);
        //        if (itemTemplate == null)
        //        {
        //            throw new KoobooException("The template does not exists.");
        //        }
        //        Site site = null;
        //        using (FileStream fs = new FileStream(itemTemplate.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //        {
        //            site = Create(parent, siteName, fs, siteSetting);
        //        }
        //        //copy site setting...
        //        site.Repository = siteSetting.Repository;
        //        site.DisplayName = siteSetting.DisplayName;
        //        site.Culture = siteSetting.Culture;
        //        site.Domains = siteSetting.Domains;
        //        site.SitePath = siteSetting.SitePath;
        //        site.Version = siteSetting.Version;
        //        site.Mode = siteSetting.Mode;
        //        site.Membership = siteSetting.Membership;
        //        Update(site);

        //        return site;
        //    }
        //}
        //[Obsolete]
        //public virtual Site Import(Site parent, string siteName, string importedSiteName, Site siteSetting, string userName = null, bool keepSiteSetting = false)
        //{
        //    var template = new ItemTemplate(importedSiteName);
        //    var itemTemplate = ServiceFactory.ImportedSiteManager.GetItemTemplate(template.Category, template.TemplateName);
        //    if (itemTemplate == null)
        //    {
        //        throw new KoobooException("The imported site does not exists.");
        //    }
        //    Site site = null;
        //    using (FileStream fs = new FileStream(itemTemplate.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
        //    {
        //        site = Import(parent, siteName, fs, siteSetting, userName, keepSiteSetting);
        //    }
        //    return site;
        //}
        //[Obsolete]
        //public virtual Site Import(Site parent, string siteName, Stream siteStream, Site siteSetting, string userName = null, bool keepSiteSetting = false)
        //{
        //    return Create(parent, siteName, siteStream, siteSetting, userName, keepSiteSetting);
        //}
        //[Obsolete]
        //public virtual Site Create(Site parent, string siteName, Stream siteStream, Site siteSetting, string userName = null, bool keepSiteSetting = false)
        //{
        //    var site = Provider.Create(parent, siteName, siteStream, new CreateSiteOptions() { ContentDatabaseName = siteSetting.Repository, MembershipName = siteSetting.Membership });
        //    site.Repository = siteSetting.Repository;
        //    site.Membership = siteSetting.Membership;
        //    if (keepSiteSetting == false)
        //    {
        //        site.DisplayName = "";
        //        site.Domains = null;
        //    }
        //    Provider.Initialize(site);
        //    Provider.Online(site);
        //    Update(site);
        //    return site;
        //}

        /// <summary>
        /// Create sub site
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="siteName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual Site Create(Site parent, string siteName, CreateSiteOptions options)
        {
            return Provider.Create(parent, siteName, null, options);
        }
        /// <summary>
        /// Create site using package file path
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="siteName"></param>
        /// <param name="packageFilePath"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual Site Create(Site parent, string siteName, string packageFilePath, CreateSiteOptions options)
        {
            using (FileStream fs = new FileStream(packageFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Create(parent, siteName, fs, options);
            }
        }
        /// <summary>
        /// Create site using package stream
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="siteName"></param>
        /// <param name="stream">the package stream can be null. if it is null, it is going to create sub site.</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual Site Create(Site parent, string siteName, Stream stream, CreateSiteOptions options)
        {
            return Provider.Create(parent, siteName, stream, options);
        }

        public virtual void Export(string fullSiteName, Stream outputStream, bool includeDatabase, bool includeSubSites)
        {
            var site = SiteHelper.Parse(fullSiteName);
            if (site.AsActual() == null)
            {
                throw new Exception("The site does not exists.".Localize());
            }
            Provider.Export(site, outputStream, includeDatabase, includeSubSites);
        }

        public virtual void Offline(string siteName)
        {
            Provider.Offline(SiteHelper.Parse(siteName));
        }
        public virtual void Online(string siteName)
        {
            Provider.Online(SiteHelper.Parse(siteName));
        }
        public virtual bool IsOnline(string siteName)
        {
            return Provider.IsOnline(SiteHelper.Parse(siteName));
        }
        #endregion

        #region Copy

        public virtual Site Copy(Site sourceSite, string siteName, CreateSiteOptions options)
        {
            MemoryStream ms = new MemoryStream();
            var exportReposiotry = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(sourceSite.Repository) == null;
            Export(sourceSite.FullName, ms, exportReposiotry, true);
            ms.Position = 0;
            return Create(sourceSite.Parent, siteName, ms, options);
        }
        #endregion

        #region UseSharedDB

        /// <summary>
        /// Determine if the site uses a shared DB.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <returns></returns>
        public virtual bool UseSharedDB(Site site)
        {
            string repository = site.AsActual().Repository;
            return IfTheRepositoryUsedByOtherSites(site, repository);
        }

        public virtual bool IfTheRepositoryUsedByOtherSites(Site site, string repository)
        {
            foreach (var item in All())
            {
                if (item != site && item.AsActual().Repository.EqualsOrNullEmpty(repository, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ResovleSite
        public virtual Site ResovleSite(HttpRequestBase httpRequest, string hostName, string requestPath)
        {
            var domainTable = Provider.GetDomainTable();
            var fullPath = hostName + "/" + requestPath.Trim('/') + "/";
            var useragent = httpRequest.UserAgent;

            return domainTable
                .Where(it => string.IsNullOrEmpty(it.UserAgent) || Regex.IsMatch(useragent, it.UserAgent, RegexOptions.IgnoreCase))
                .Where(it => fullPath.StartsWith(it.FullDomain, StringComparison.OrdinalIgnoreCase))
                .Select(it => it.SiteObject).FirstOrDefault();
        }

        #endregion
    }
}
