#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.eCommerce;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using Kooboo.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.eCommerce.NHibernate.Persistence.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    [Dependency(typeof(IProductVariantProvider))]
    public class ProductVariantProvider : IProductVariantProvider
    {
        #region Fields
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        private string dataPath;
        private ICommerceDataDir commerceDataDir;
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[] { };
            }
        }
        #endregion

        #region .ctor
        public ProductVariantProvider(ICommerceDataDir dataDir)
        {
            this.commerceDataDir = dataDir;
            this.dataPath = Path.Combine(dataDir.DataPhysicalPath, "ProductVariant");
        }
        #endregion

        #region Methods
        public IEnumerable<ProductVariant> All()
        {
            List<ProductVariant> list = new List<ProductVariant>();
            if (Directory.Exists(dataPath))
            {
                foreach (var item in IO.IOUtility.EnumerateFilesExludeHidden(dataPath))
                {
                    var pv = (ProductVariant)Serialization.Deserialize(typeof(ProductVariant), KnownTypes, GetFilePath(item.Name));
                    list.Add(pv);
                }
            }
            return list;
        }

        public IEnumerable<ProductVariant> QueryByProduct(Product product)
        {
            return new ProductVariant[0];
        }

        public ProductVariant Get(ProductVariant dummy)
        {
            var filepath = GetFilePath(dummy.UUID);
            if (File.Exists(filepath))
            {
                locker.EnterReadLock();
                try
                {
                    var item = (ProductVariant)Serialization.Deserialize(dummy.GetType(), KnownTypes, filepath);
                    return item;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            return default(ProductVariant);
        }

        public void Add(ProductVariant item)
        {
            var path = GetFilePath(item.UUID);
            IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(path));
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<ProductVariant>(item, path);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void Update(ProductVariant @new, ProductVariant old)
        {
            Remove(old);
            Add(@new);
        }

        public void Remove(ProductVariant item)
        {
            locker.EnterWriteLock();
            try
            {
                var file = GetFilePath(item.UUID);
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        #region private
        private string GetFilePath(string uuid)
        {
            return Path.Combine(dataPath, uuid, commerceDataDir.CMSDir.SettingFileName);
        }
        #endregion
        #endregion


    }
}
