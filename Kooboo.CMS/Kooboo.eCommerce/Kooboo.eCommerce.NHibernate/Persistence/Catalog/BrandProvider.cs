#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Persistence;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System.IO;
using System.Linq;

namespace Kooboo.eCommerce.NHibernate.Persistence.Catalog
{
    [Dependency(typeof(IBrandProvider))]
    public class BrandProvider : ProviderBase<Brand>, IBrandProvider
    {
        public IEntityFileProvider EntityFileProvider { get; private set; }

        public BrandProvider(IDbContextFactory dbContextFactory, IEntityFileProvider entityFileProvider)
            : base(dbContextFactory)
        {
            this.EntityFileProvider = entityFileProvider;
        }

        public override void Add(Brand obj)
        {
            //save entity file
            var folderName = GetFileFolderName(obj);
            if (obj.ImageFile != null)
            {
                var result = EntityFileProvider.Save(obj.ImageFile, folderName);
                obj.Image = result.VirtualPath;
            }
            base.Add(obj);
        }

        public override void Update(Brand obj)
        {
            //save entity file
            var folderName = GetFileFolderName(obj);
            if (obj.ImageFile != null)
            {
                var result = EntityFileProvider.Save(obj.ImageFile, folderName);
                obj.Image = result.VirtualPath;
            }
            base.Update(obj);
        }

        public override void Delete(Brand obj)
        {
            base.Delete(obj);
            //delete entity file
            var folderName = GetFileFolderName(obj);
            EntityFileProvider.DeleteFolder(folderName);
        }

        private string GetFileFolderName(Brand obj)
        {
            return Path.Combine("Brand", "Brands", obj.Id.ToString());
        }
    }
}
