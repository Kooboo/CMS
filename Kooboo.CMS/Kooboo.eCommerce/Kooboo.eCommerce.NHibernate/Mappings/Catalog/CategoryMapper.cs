#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Catalog;
using Kooboo.CMS.eCommerce.Models.Catalog;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using ByCode = NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kooboo.eCommerce.NHibernate.Mappings.Catalog
{
    #region Custom collection
    public class PersistentStorageCategoryBag : PersistentGenericBag<Category>
    {
        public PersistentStorageCategoryBag(ISessionImplementor session)
            : base(session)
        {
        }

        public PersistentStorageCategoryBag(ISessionImplementor session, ICollection<Category> original)
            : base(session, original)
        {
        }

        public override ICollection GetOrphans(object snapshot, string entityName)
        {
            var orphans = base.GetOrphans(snapshot, entityName)
                .Cast<Category>()
                .Where(b => ReferenceEquals(null, b.Parent))
                .ToArray();

            return orphans;
        }
    }

    public class StorageCategoryBag : IUserCollectionType
    {
        public object Instantiate(int anticipatedSize)
        {
            return new List<Category>();
        }

        public IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister)
        {
            return new PersistentStorageCategoryBag(session);
        }

        public IPersistentCollection Wrap(ISessionImplementor session, object collection)
        {
            return new PersistentStorageCategoryBag(session, (IList<Category>)collection);
        }

        public IEnumerable GetElements(object collection)
        {
            return (IEnumerable)collection;
        }

        public bool Contains(object collection, object entity)
        {
            return ((IList<Category>)collection).Contains((Category)entity);
        }

        public object IndexOf(object collection, object entity)
        {
            return ((IList<Category>)collection).IndexOf((Category)entity);
        }

        public object ReplaceElements(object original, object target, ICollectionPersister persister, object owner, IDictionary copyCache, ISessionImplementor session)
        {
            var result = (IList<Category>)target;
            result.Clear();

            foreach (var box in (IEnumerable)original)
                result.Add((Category)box);

            return result;
        }
    }
    #endregion
    /// <summary>
    /// Mapping Category
    /// </summary>
    public class CategoryMapper : ClassMapping<Category>
    {
        /// <summary>
        /// Mapping
        /// </summary>
        public CategoryMapper()
        {
            Id(category => category.Id, map => map.Generator(ByCode.Generators.Identity));
            Property(category => category.Name, map => map.Length(255));
            Property(category => category.Description);
            Property(category => category.Image, map => map.Length(255));
            Property(category => category.PageSize);
            Property(category => category.Published);
            Property(category => category.Deleted);
            Property(category => category.DisplayOrder);
            Property(category => category.UtcCreationDate);
            Property(category => category.UtcUpdateDate);
            Property(category => category.Site, map => map.Length(255));
            Property(category => category.FullName, map => map.Length(255));

            ManyToOne(category => category.Parent, map =>
            {
                map.Column("ParentId");                
                map.Cascade(ByCode.Cascade.None);
            });

            Bag(category => category.Children, map =>
            {
                map.Key(it => it.Column("ParentId"));
                map.Inverse(true);
                map.Cascade(ByCode.Cascade.DeleteOrphans | ByCode.Cascade.All);
                //map.Lazy(ByCode.CollectionLazy.Lazy);
                //map.Type<StorageCategoryBag>();
            }, collecion =>
            {
                collecion.OneToMany();
            });
        }
    }
}
