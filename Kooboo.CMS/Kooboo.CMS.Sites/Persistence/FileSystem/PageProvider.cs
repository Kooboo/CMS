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
using Kooboo.IO;
using System.ComponentModel.Composition;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System.Threading;
namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPageProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Page>))]
    public class PageProvider : InheritableProviderBase<Page>, IPageProvider, ILocalizableProvider<Page>
    {
        class PageFileStorage : DirectoryObjectFileStorage<Page>
        {
            public PageFileStorage(string baseFolder, ReaderWriterLockSlim @lock, Func<DirectoryInfo, Page> initialize)
                : base(baseFolder, @lock, KnownTypes, initialize)
            {
            }
            protected override string GetItemPath(Page o)
            {
                return Path.Combine(new[] { _baseFolder }.Concat(o.PageNamePaths).ToArray());
            }
        }
        #region Versioning
        public class PageVersionLogger : FileVersionLogger<Page>
        {
            static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
            public override void LogVersion(Page o)
            {
                locker.EnterWriteLock();
                try
                {
                    VersionPath versionPath = new VersionPath(o, NextVersionId(o));
                    IOUtility.EnsureDirectoryExists(versionPath.PhysicalPath);
                    var versionDataFile = Path.Combine(versionPath.PhysicalPath, o.DataFileName);
                    PageProvider provider = new PageProvider();
                    Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(o, versionDataFile, KnownTypes);
                }
                finally
                {
                    locker.ExitWriteLock();
                }
            }

            public override Page GetVersion(Page o, int version)
            {
                VersionPath versionPath = new VersionPath(o, version);
                var versionDataFile = Path.Combine(versionPath.PhysicalPath, o.DataFileName);
                Page page = null;
                if (File.Exists(versionDataFile))
                {
                    PageProvider provider = new PageProvider();
                    page = (Page)Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize(typeof(Page), KnownTypes, versionDataFile);
                    ((IPersistable)page).Init(o);
                }
                return page;
            }

            public override void Revert(Page o, int version, string userName)
            {
                var versionData = GetVersion(o, version);
                if (versionData != null)
                {
                    versionData.UserName = userName;
                    versionData.LastUpdateDate = DateTime.UtcNow;
                    Providers.PageProvider.Update(versionData, o);
                    LogVersion(versionData);
                }
            }
        }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim _lock = new System.Threading.ReaderWriterLockSlim();
        //protected override System.Threading.ReaderWriterLockSlim GetLocker()
        //{
        //    return locker;
        //}
        #endregion

        #region KnownTypes
        static IEnumerable<Type> KnownTypes = new Type[]{
                typeof(PagePosition),
                typeof(ViewPosition),
                typeof(ModulePosition),
                typeof(HtmlPosition),
                typeof(ContentPosition),
                typeof(HtmlBlockPosition),
                typeof(ProxyPosition),
                typeof(DataRuleBase),
                typeof(FolderDataRule),
                typeof(SchemaDataRule),
                typeof(CategoryDataRule),
                typeof(HttpDataRule)};

        #endregion

        #region IPageRepository Members

        public IEnumerable<Models.Page> ChildPages(Models.Page parentPage)
        {
            var fileStorage = new PageFileStorage(GetItemPath(parentPage), _lock, (dir) =>
            {
                return new Page(parentPage, dir.Name);
            });

            var items = fileStorage.GetList().ToArray();

            return items;

        }
        #endregion

        #region Localize & Move
        public void Localize(Page sourcePage, Site targetSite)
        {
            var sourceSite = sourcePage.Site;
            if (sourceSite != targetSite)
            {
                var namePaths = sourcePage.PageNamePaths.ToArray();
                var destPage = new Page(targetSite, namePaths);
                var destPath = destPage.PhysicalPath;
                ILocalizableHelper.CopyFiles(sourcePage.PhysicalPath, destPath);
            }
        }
        public void Move(Site site, string pageFullName, string newParent)
        {
            _lock.EnterWriteLock();

            try
            {
                var page = PageHelper.Parse(site, pageFullName);
                Page parentPage = null;
                if (!string.IsNullOrEmpty(newParent))
                {
                    parentPage = PageHelper.Parse(site, newParent);
                    if (parentPage == page.Parent || parentPage == page)
                    {
                        throw new KoobooException(string.Format("The page is under '{0}' already".Localize(), newParent));
                    }
                }
                Page newPage = null;
                if (parentPage != null)
                {
                    newPage = new Page(parentPage, page.Name);
                }
                else
                {
                    newPage = new Page(site, page.Name);
                }
                if (newPage.Exists())
                {
                    throw new KoobooException(string.Format("The page '{0}' already exists in '{1}'".Localize(), page.Name, parentPage.FriendlyName));
                }
                Directory.Move(page.PhysicalPath, newPage.PhysicalPath);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        #endregion

        #region IPageRepository Relation Members

        private IEnumerable<Page> AllPagesNested(Site site)
        {
            return Providers.PageProvider.All(site).SelectMany(it => AllPagesNested(it));
        }
        private IEnumerable<Page> AllPagesNested(Page parent)
        {
            var childPages = Providers.PageProvider.ChildPages(parent);
            return new[] { parent }.Concat(childPages.SelectMany(it => AllPagesNested(it)));
        }
        public IEnumerable<Page> ByLayout(Layout layout)
        {
            return AllPagesNested(layout.Site)
                .Select(it => it.AsActual())
                .Where(it => it.Layout.EqualsOrNullEmpty(layout.Name, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
        }

        public IEnumerable<Page> ByView(Models.View view)
        {
            return AllPagesNested(view.Site).Select(it => it.AsActual())
                .Where(it => it.PagePositions != null &&
                    it.PagePositions.Any(p => p is ViewPosition && ((ViewPosition)p).ViewName.EqualsOrNullEmpty(view.Name, StringComparison.CurrentCultureIgnoreCase)))
                    .ToArray();
        }

        public IEnumerable<Page> ByModule(Site site, string moduleName)
        {
            return AllPagesNested(site).Select(it => it.AsActual())
                 .Where(it => it.PagePositions != null &&
                     it.PagePositions.Any(p => p is ModulePosition && ((ModulePosition)p).ModuleName.EqualsOrNullEmpty(moduleName, StringComparison.CurrentCultureIgnoreCase)))
                     .ToArray();
        }
        public IEnumerable<Page> ByHtmlBlock(HtmlBlock htmlBlock)
        {
            return AllPagesNested(htmlBlock.Site).Select(it => it.AsActual())
               .Where(it => it.PagePositions != null &&
                   it.PagePositions.Any(p => p is HtmlBlockPosition && ((HtmlBlockPosition)p).BlockName.EqualsOrNullEmpty(htmlBlock.Name, StringComparison.CurrentCultureIgnoreCase)))
                   .ToArray();
        }
        #endregion

        #region Copy
        public Page Copy(Site site, string sourcePageFullName, string newPageFullName)
        {
            _lock.EnterWriteLock();

            try
            {
                var page = PageHelper.Parse(site, sourcePageFullName);
                var newPage = PageHelper.Parse(site, newPageFullName);

                IOUtility.CopyDirectory(page.PhysicalPath, newPage.PhysicalPath);

                return newPage;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #endregion

        #region Draft
        public Page GetDraft(Page page)
        {
            var draftDataFile = GetDraftDataFile(page);
            Page draft = null;
            if (File.Exists(draftDataFile))
            {
                draft = (Page)Kooboo.Runtime.Serialization.DataContractSerializationHelper.Deserialize(typeof(Page), KnownTypes, draftDataFile);
                ((IPersistable)draft).Init(page);
            }
            return draft;
        }
        private static string GetDraftDataFile(Page page)
        {
            return page.DataFile + ".draft";
        }
        public void SaveAsDraft(Page page)
        {
            Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(page, GetDraftDataFile(page), KnownTypes);
        }


        public void RemoveDraft(Page page)
        {
            var draftDataFile = GetDraftDataFile(page);
            _lock.EnterWriteLock();
            try
            {
                if (File.Exists(draftDataFile))
                {
                    File.Delete(draftDataFile);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        #endregion

        #region Export

        public void Import(Site site, Page parent, Stream zipStream, bool @override)
        {
            _lock.EnterWriteLock();
            try
            {
                var destDir = "";
                if (parent != null)
                {
                    destDir = parent.PhysicalPath;
                }
                else
                {
                    destDir = new Page(site, "Dummy").BasePhysicalPath;
                }
                ImportHelper.Import(site, destDir, zipStream, @override);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        #endregion

        private string GetBasePath(Site site)
        {
            return Path.Combine(site.PhysicalPath, "Pages");
        }
        private string GetItemPath(Page page)
        {
            return Path.Combine(GetBasePath(page.Site), page.Name);
        }
        protected override IFileStorage<Page> GetFileStorage(Site site)
        {
            return new PageFileStorage(GetBasePath(site), _lock, (dir) =>
            {
                return new Page(site, dir.Name);
            });
        }


        #region InitializePages

        public void Clear(Site site)
        {
            // no need to implement.
        }
        #endregion
    }
}
