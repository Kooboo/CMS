#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Persistence.Relational
{
    public interface IProvider<T>        
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
