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

namespace Kooboo.eCommerce.NHibernate.Persistence.Catalog
{
    [Dependency(typeof(IProductImageProvider))]
    public class ProductImageProvider : ProviderBase<ProductImage>, IProductImageProvider
    {
        public IEntityFileProvider EntityFileProvider { get; set; }

        public ProductImageProvider(IDbContextFactory dbContextFactory,IEntityFileProvider entityFileProvider)
            : base(dbContextFactory)
        {
            this.EntityFileProvider = entityFileProvider;
        }

        public override void Add(ProductImage obj)
        {
            //save entity file
            var folderName = GetFileFolderName(obj);
            if (obj.ImageFile != null)
            {
                var result = EntityFileProvider.Save(obj.ImageFile, folderName);
                obj.ImageUrl = result.VirtualPath;
            }
            base.Add(obj);
        }
        public override void Update(ProductImage obj)
        {
            //save entity file
            var folderName = GetFileFolderName(obj);
            if (obj.ImageFile != null)
            {
                var result = EntityFileProvider.Save(obj.ImageFile, folderName);
                obj.ImageUrl = result.VirtualPath;
            }
            base.Update(obj);
        }
        public override void Delete(ProductImage obj)
        {
            base.Delete(obj);
            //delete entity file
            var folderName = GetFileFolderName(obj);
            EntityFileProvider.DeleteFolder(folderName);
        }
        private string GetFileFolderName(ProductImage obj)
        {
            return Path.Combine("ProductImage", "ProductImages", obj.Id.ToString());
        }
    }
}
