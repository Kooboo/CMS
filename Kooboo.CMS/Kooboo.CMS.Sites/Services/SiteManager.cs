#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(SiteManager))]
    public class SiteManager : PathResourceManagerBase<Site, ISiteProvider>
    {
        #region .ctor
        public SiteManager(ISiteProvider provider) : base(provider) { }
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
        public virtual void Remove(Site site, bool includeRepository)
        {
            if (!UseSharedDB(site) && includeRepository)
            {
                RemoveSiteRepository(site);
            }

            Provider.Offline(site);

            Provider.Remove(site);

        }

        private void RemoveSiteRepository(Site site)
        {
            var repository = site.AsActual().Repository;

            if (!string.IsNullOrEmpty(repository))
            {
                Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Remove(new Kooboo.CMS.Content.Models.Repository(repository));
            }
            foreach (var child in ChildSites(site))
            {
                RemoveSiteRepository(child);
            }
        }
        #endregion

        #region SiteTrees
        public virtual IEnumerable<SiteTree> SiteTrees(string userName)
        {
            return AllRootSites().Select(it => GetSiteNode(it, userName))
                .Where(it => it != null)
                .Select(it => new SiteTree() { Root = it });
        }
        protected virtual SiteNode GetSiteNode(Site site, string userName)
        {
            if (ServiceFactory.UserManager.Authorize(site, userName))
            {
                SiteNode siteNode = new SiteNode() { Site = site.AsActual() };

                siteNode.Children = ChildSites(site)
                    .Select(it => GetSiteNode(it, userName))
                    .Where(it => it != null);

                return siteNode;
            }
            return null;
        }
        #endregion

        #region Export/Import
        private void CopyRepository(Kooboo.CMS.Content.Models.Repository sourceRepository, string name)
        {
            var repositoryManager = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager;
            var repository = repositoryManager.Get(name);

            if (repository == null)
            {
                repositoryManager.Copy(sourceRepository, name);
            }
        }
        public virtual Site Create(Site parent, string siteName, string templateFullName, Site siteSetting, string userName = null)
        {
            if (string.IsNullOrEmpty(templateFullName))
            {
                if (parent == null)
                {
                    throw new KoobooException("Parent site is required.".Localize());
                }
                parent = parent.AsActual();

                siteSetting.Parent = parent;

                // Set the same settings with parent.
                siteSetting.Theme = parent.Theme;
                siteSetting.EnableJquery = parent.EnableJquery;
                siteSetting.EnableStyleEdting = parent.EnableStyleEdting;
                siteSetting.EnableVersioning = parent.EnableVersioning;
                siteSetting.InlineEditing = parent.InlineEditing;
                siteSetting.CustomFields = parent.CustomFields;
                siteSetting.Smtp = parent.Smtp;
                siteSetting.Membership = parent.Membership;

                Add(parent, siteSetting);

                if (!string.IsNullOrEmpty(siteSetting.Repository))
                {
                    CopyRepository(parent.GetRepository(), siteSetting.Repository);
                }
                return siteSetting;
            }
            else
            {
                var template = new ItemTemplate(templateFullName);
                var itemTemplate = ServiceFactory.SiteTemplateManager.GetItemTemplate(template.Category, template.TemplateName);
                if (itemTemplate == null)
                {
                    throw new KoobooException("The template does not exists.");
                }
                Site site = null;
                using (FileStream fs = new FileStream(itemTemplate.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    site = Create(parent, siteName, fs, siteSetting);
                }
                //copy site setting...
                site.Repository = siteSetting.Repository;
                site.DisplayName = siteSetting.DisplayName;
                site.Culture = siteSetting.Culture;
                site.Domains = siteSetting.Domains;
                site.SitePath = siteSetting.SitePath;
                site.Version = siteSetting.Version;
                site.Mode = siteSetting.Mode;
                site.Membership = siteSetting.Membership;
                Update(site);

                return site;
            }
        }
        public virtual Site Import(Site parent, string siteName, string importedSiteName, Site siteSetting, string userName = null)
        {
            var template = new ItemTemplate(importedSiteName);
            var itemTemplate = ServiceFactory.ImportedSiteManager.GetItemTemplate(template.Category, template.TemplateName);
            if (itemTemplate == null)
            {
                throw new KoobooException("The imported site does not exists.");
            }
            Site site = null;
            using (FileStream fs = new FileStream(itemTemplate.TemplateFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                site = Import(parent, siteName, fs, siteSetting);
            }
            return site;
        }
        public virtual Site Import(Site parent, string siteName, Stream siteStream, Site siteSetting, string userName = null)
        {
            return Create(parent, siteName, siteStream, siteSetting);
        }

        public virtual Site Create(Site parent, string siteName, Stream siteStream, Site siteSetting, string userName = null)
        {
            var site = Provider.Create(parent, siteName, siteStream, new CreateSiteSetting() { Repository = siteSetting.Repository, Membership = siteSetting.Membership });
            site.Repository = siteSetting.Repository;
            site.Membership = siteSetting.Membership;
            site.DisplayName = "";
            site.Domains = null;
            Update(site);
            Provider.Initialize(site);
            Provider.Online(site);

            return site;
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

        public virtual Site Copy(Site sourceSite, string siteName, Site siteSetting)
        {
            MemoryStream ms = new MemoryStream();
            var exportReposiotry = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(siteSetting.Repository) == null;
            Export(sourceSite.FullName, ms, exportReposiotry, true);
            ms.Position = 0;
            return Create(sourceSite.Parent, siteName, ms, siteSetting);
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
    }
}
