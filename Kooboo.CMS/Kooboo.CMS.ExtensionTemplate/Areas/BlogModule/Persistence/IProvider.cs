
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence
{
    public interface IProvider<T>
         where T : class,IEntity
    {
        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> CreateQuery();

        /// <summary>
        /// Query by.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T QueryById(int id);

        /// <summary>
        /// Adds the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        void Add(T obj);

        /// <summary>
        /// Updates the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        void Update(T obj);

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void Delete(T obj);
    }
}
