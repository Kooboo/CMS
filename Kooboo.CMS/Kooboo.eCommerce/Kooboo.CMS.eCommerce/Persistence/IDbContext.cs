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
    /// <summary>
    /// 
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Sets the scope.
        /// 设置DbContext的事务提交范围，如果有设置scopeObject的话，在调用SaveChanges时传入的scopeObject与这个值相等才会Commit。
        /// </summary>
        /// <param name="scopeObject">The scope object.</param>
        IDbContext SetScope(object scopeObject);

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="scopeObject">The scope object.</param>
        void SaveChanges(object scopeObject);

        ///// <summary>
        ///// Creates the transaction.
        ///// </summary>
        ///// <returns></returns>
        //ITransaction CreateTransaction();
    }
}
