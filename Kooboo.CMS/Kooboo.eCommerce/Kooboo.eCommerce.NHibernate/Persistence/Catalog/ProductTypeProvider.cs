#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using Kooboo.CMS.Form;
using Kooboo.IO;
using Kooboo.Runtime.Serialization;
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
    [Dependency(typeof(IProductTypeProvider))]
    public class ProductTypeProvider : IProductTypeProvider
    {
        #region Fields
        private static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        private string baseDataPath;
        private ICommerceDataDir commerceDataDir;
        protected virtual IEnumerable<Type> KnownTypes
        {
            get
            {
                return new Type[]{
                    typeof(ColumnValidation),
                    typeof(RequiredValidation),
                    typeof(StringLengthValidation),
                    typeof(RangeValidation),
                    typeof(RegexValidation)
                };
            }
        }
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeProvider" /> class.
        /// </summary>
        /// <param name="dataDir">The data dir.</param>
        public ProductTypeProvider(ICommerceDataDir dataDir)
        {
            this.commerceDataDir = dataDir;
            this.baseDataPath = Path.Combine(dataDir.DataPhysicalPath, "ProductTypes");
        }

        #endregion

        #region Methods
        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductType> All()
        {
            List<ProductType> list = new List<ProductType>();
            if (Directory.Exists(baseDataPath))
            {
                foreach (var item in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDataPath))
                {
                    var pt = (ProductType)DataContractSerializationHelper.Deserialize(typeof(ProductType), KnownTypes, GetSettingFilePath(item.Name));
                    list.Add(pt);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets the specified dummy.
        /// </summary>
        /// <param name="dummy">The dummy.</param>
        /// <returns></returns>
        public ProductType Get(ProductType dummy)
        {
            var filepath = GetSettingFilePath(dummy.Name);
            if (File.Exists(filepath))
            {
                locker.EnterReadLock();
                try
                {
                    var item = (ProductType)DataContractSerializationHelper.Deserialize(dummy.GetType(), KnownTypes, filepath);
                    return item;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            return default(ProductType);
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(ProductType item)
        {
            var path = GetSettingFilePath(item.Name);
            IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(path));
            locker.EnterWriteLock();
            try
            {
                Kooboo.Runtime.Serialization.DataContractSerializationHelper.Serialize(item, path);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the specified new.
        /// </summary>
        /// <param name="new">The new.</param>
        /// <param name="old">The old.</param>
        public void Update(ProductType @new, ProductType old)
        {
            Remove(old);
            Add(@new);
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Remove(ProductType item)
        {
            locker.EnterWriteLock();
            try
            {
                var itemPath = GetItemPath(item.Name);
                Kooboo.IO.IOUtility.DeleteDirectory(itemPath, true);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        #region private
        private string GetItemPath(string name)
        {
            return Path.Combine(baseDataPath, name);
        }
        private string GetSettingFilePath(string name)
        {
            return Path.Combine(GetItemPath(name), commerceDataDir.CMSDir.SettingFileName);
        }
        #endregion
        #endregion
    }
}
