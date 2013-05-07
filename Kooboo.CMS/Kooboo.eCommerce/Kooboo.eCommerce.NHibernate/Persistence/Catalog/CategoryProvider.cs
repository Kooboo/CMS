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
using NHibernate.Linq;
using Kooboo.CMS.Common.Persistence.Relational;

namespace Kooboo.eCommerce.NHibernate.Persistence.Catalog
{
    [Dependency(typeof(ICategoryProvider))]
    [Dependency(typeof(IProvider<Category>))]
    public class CategoryProvider : ProviderBase<Category>, ICategoryProvider
    {
        public IEntityFileProvider EntityFileProvider { get; private set; }

        public CategoryProvider(IDbContextFactory dbContextFactory, IEntityFileProvider entityFileProvider)
            : base(dbContextFactory)
        {
            this.EntityFileProvider = entityFileProvider;
        }

        public override void Add(Category obj)
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

        public override void Update(Category obj)
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

        public override void Delete(Category obj)
        {
            base.Delete(obj);
            //delete entity file
            var folderName = GetFileFolderName(obj);
            EntityFileProvider.DeleteFolder(folderName);
        }

        public System.Collections.Generic.IEnumerable<Category> QueryChildren(Category parent)
        {
            return CreateQuery().Where(it => it.Parent == parent).AsEnumerable();
        }

        #region private
        private string GetFileFolderName(Category obj)
        {
            return Path.Combine("Category", "Categories", obj.Id.ToString());
        }
        #endregion
    }
}
