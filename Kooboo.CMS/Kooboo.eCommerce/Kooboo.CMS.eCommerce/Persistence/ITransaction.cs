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

namespace Kooboo.CMS.eCommerce.Persistence
{
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <value>
        /// The db context.
        /// </value>
        IDbContext DbContext { get; }
        /// <summary>
        /// Commits the changes
        /// </summary>
        void Commit();
        /// <summary>
        /// Rollbacks the changes
        /// </summary>
        void Rollback();
    }
}
