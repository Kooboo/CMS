using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    using System.IO;
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Common.Persistence.Non_Relational;
    using Kooboo.CMS.Sites.ABTest;
    using Kooboo.CMS.Common;

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IABPageTestResultProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<ABPageTestResult>), Order = 100)]
    public class ABPageTestResultProvider : IABPageTestResultProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim(System.Threading.LockRecursionPolicy.SupportsRecursion);
        SiteDBContext _dbContext;
        public ABPageTestResultProvider(SiteDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IEnumerable<ABTest.ABPageTestResult> All(Site site)
        {
            return _dbContext.ABPageTestResults.Where(it => it.SiteName.Equals(site.FullName, StringComparison.OrdinalIgnoreCase)).ToList().Select(it => it.ToABPageTestResult());
        }

        public IEnumerable<ABTest.ABPageTestResult> All()
        {
            return _dbContext.ABPageTestResults.ToList().Select(it => it.ToABPageTestResult());
        }

        public ABTest.ABPageTestResult Get(ABTest.ABPageTestResult dummy)
        {
            return All().FirstOrDefault(it => it.UUID.Equals(dummy.UUID, StringComparison.OrdinalIgnoreCase) && it.Site == dummy.Site);
        }

        public void Add(ABTest.ABPageTestResult item)
        {
            if (null != item)
            {
                UpdateOrAdd(item, item);
            }
        }

        public void Update(ABTest.ABPageTestResult @new, ABTest.ABPageTestResult old)
        {
            UpdateOrAdd(@new, old);
        }

        public void Remove(ABTest.ABPageTestResult item)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABPageTestResults.FirstOrDefault(it => it.UUID.Equals(item.UUID, StringComparison.OrdinalIgnoreCase) && it.SiteName.Equals(item.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                _dbContext.ABPageTestResults.Remove(dummy);
                _dbContext.SaveChanges();
            }
            ((IPersistable)item).OnSaved();
        }


        private void UpdateOrAdd(ABPageTestResult item, ABPageTestResult oldItem)
        {
            ((IPersistable)item).OnSaving();
            var dummy = _dbContext.ABPageTestResults.FirstOrDefault(it => it.UUID.Equals(oldItem.UUID, StringComparison.OrdinalIgnoreCase)
                && it.SiteName.Equals(oldItem.Site.FullName, StringComparison.OrdinalIgnoreCase));
            if (null != dummy)
            {
                item.ToABPageTestResultEntity<ABPageTestResultEntity>(dummy);
            }
            else
            {
                dummy = item.ToABPageTestResultEntity<ABPageTestResultEntity>();
                _dbContext.ABPageTestResults.Add(dummy);
            }
            _dbContext.SaveChanges();
            ((IPersistable)item).OnSaved();
        }
    }
}
