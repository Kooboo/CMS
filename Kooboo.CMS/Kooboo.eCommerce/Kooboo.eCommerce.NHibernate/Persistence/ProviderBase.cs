#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.eCommerce.Persistence;
using System;
using System.Linq;
using NHibernate.Linq;
using Kooboo.CMS.eCommerce.Models;

namespace Kooboo.eCommerce.NHibernate.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ProviderBase<T> : IProvider<T>
        where T : class,IRelationEntity
    {
        #region Properties
        protected NHibernateContextFactory DbContextFactory { get; private set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderBase{T}" /> class.
        /// </summary>
        /// <param name="dbContextFactory">The db context.</param>
        public ProviderBase(IDbContextFactory dbContextFactory)
        {
            this.DbContextFactory = (NHibernateContextFactory)dbContextFactory;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> CreateQuery()
        {
            return DbContextFactory.GetDbContext().Session.Query<T>();
        }

        /// <summary>
        /// Queries the by.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual T QueryById(int id)
        {
            return DbContextFactory.GetDbContext().Session.Get<T>(id);
            //return CreateQuery().Where(it => it.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Adds the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public virtual void Add(T obj)
        {
            DbContextFactory.GetDbContext().Session.Save(obj);
            DbContextFactory.GetDbContext().SaveChanges(this);
        }

        /// <summary>
        /// Updates the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public virtual void Update(T obj)
        {
            obj = DbContextFactory.GetDbContext().Session.Merge<T>(obj);
            DbContextFactory.GetDbContext().Session.Update(obj);
            DbContextFactory.GetDbContext().SaveChanges(this);
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public virtual void Delete(T obj)
        {
            var o = QueryById(obj.Id);
            DbContextFactory.GetDbContext().Session.Delete(o);
            DbContextFactory.GetDbContext().SaveChanges(this);
        }
        #endregion
    }
}
