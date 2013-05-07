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
    [Dependency(typeof(IImageTypeProvider))]
    public class ImageTypeProvider : IImageTypeProvider
    {
        #region Properties
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
        public ImageTypeProvider(ICommerceDataDir dataDir)
        {
            this.commerceDataDir = dataDir;
            this.dataPath = Path.Combine(dataDir.DataPhysicalPath, "ImageType");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get all imagetypes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ImageType> All()
        {
            List<ImageType> list = new List<ImageType>();
            if (Directory.Exists(dataPath))
            {
                foreach (var item in IO.IOUtility.EnumerateDirectoriesExludeHidden(dataPath))
                {
                    var it = (ImageType)Serialization.Deserialize(typeof(ImageType), KnownTypes, GetFilePath(item.Name));
                    list.Add(it);
                }
            }
            return list;
        }

        public ImageType Get(ImageType dummy)
        {
            var filepath = GetFilePath(dummy.Name);
            if (File.Exists(filepath))
            {
                locker.EnterReadLock();
                try
                {
                    var item = (ImageType)Serialization.Deserialize(dummy.GetType(), KnownTypes, filepath);
                    return item;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
            return default(ImageType);
        }

        public void Add(ImageType item)
        {
            var filepath = GetFilePath(item.Name);
            IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(filepath));
            locker.EnterWriteLock();
            try
            {
                Serialization.Serialize<ImageType>(item, filepath);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public void Update(ImageType @new,ImageType old)
        {
            Remove(old);
            Add(@new);
        }

        public void Remove(ImageType item)
        {
            locker.EnterWriteLock();
            try
            {
                var filepath = GetPath(item.Name);
                Kooboo.IO.IOUtility.DeleteDirectory(filepath,true);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        #region private
        private string GetPath(string name)
        {
            return Path.Combine(dataPath, name);
        }
        private string GetFilePath(string name)
        {
            return Path.Combine(dataPath, name, commerceDataDir.CMSDir.SettingFileName);
        }
        #endregion

        #endregion
    }
}
