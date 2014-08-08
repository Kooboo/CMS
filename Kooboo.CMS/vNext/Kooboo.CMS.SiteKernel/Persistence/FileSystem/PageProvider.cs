#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence.FileSystem.Storage;
using Kooboo.Common.Data.IsolatedStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(IPageProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.DependencyAttribute(typeof(IProvider<Page>))]
    public class PageProvider : FileProviderBase<Page>, IPageProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        protected override IFileStorage<Page> GetFileStorage(Site site)
        {
            IIsolatedStorage isolatedStorage = site.GetIsolatedStorage();

            var directoryStorage = new DirectoryObjectFileStorage<Page>(isolatedStorage, "Pages", locker, new Type[0], (name) =>
            {
                return new Page(site, name);
            });
            return directoryStorage;
        }
        public IEnumerable<Page> RootPages(Site site)
        {
            return RootItems(site);
        }
        public IEnumerable<Page> All(Site site)
        {
            List<Page> allPages = new List<Page>();

            foreach (var page in RootPages(site))
            {
                AddPageAndItsChildPages(page, ref allPages);
            }
            return allPages;
        }
        private void AddPageAndItsChildPages(Page page, ref List<Page> pages)
        {
            pages.Add(page);
            foreach (var item in ChildPages(page))
            {
                AddPageAndItsChildPages(item, ref pages);
            }
        }
        public IEnumerable<Page> ChildPages(Page parentPage)
        {
            return GetFileStorage(parentPage.Site).GetList(parentPage.FullName);
        }

        public Page GetDraft(Page page)
        {
            throw new NotImplementedException();
        }

        public void SaveAsDraft(Page page)
        {
            throw new NotImplementedException();
        }

        public void RemoveDraft(Page page)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Page> All()
        {
            throw new NotImplementedException();
        }

        public void Import(Page data, System.IO.Stream zipData, IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream Export(IEnumerable<Page> data, IDictionary<string, object> options)
        {
            throw new NotImplementedException();
        }





    }
}
