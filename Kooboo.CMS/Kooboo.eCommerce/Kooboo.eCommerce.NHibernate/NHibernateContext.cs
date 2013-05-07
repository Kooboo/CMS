#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.eCommerce.Persistence;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.eCommerce.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    [Dependency(typeof(NHibernateContext), ComponentLifeStyle.InRequestScope)]
    public class NHibernateContext : IDbContext
    {
        private object _scopeObject = null;

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public ISession Session { get; private set; }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <value>
        /// The transaction.
        /// </value>
        public ITransaction Transaction { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateContext" /> class.
        /// </summary>
        public NHibernateContext()
        {
            CreateNewSession();
        }

        private void CreateNewSession()
        {
            Close();
            Session = SessionFactory.CreateSession();
            Transaction = Session.BeginTransaction();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        private void Close()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
            if (Session != null)
            {
                Session.Dispose();
                Session = null;
            }
        }

        /// <summary>
        /// Sets the scope.
        /// 设置DbContext的事务提交范围，如果有设置scopeObject的话，在调用SaveChanges时传入的scopeObject与这个值相等才会Commit。
        /// </summary>
        /// <param name="scopeObject">The scope object.</param>
        /// <returns></returns>
        public IDbContext SetScope(object scopeObject)
        {
            this._scopeObject = scopeObject;
            return this;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="scopeObject">The scope object.</param>
        public void SaveChanges(object scopeObject)
        {
            if (_scopeObject == null || _scopeObject == scopeObject)
            {
                try
                {
                    this.Transaction.Commit();
                }
                catch
                {
                    this.Transaction.Rollback();
                    throw;
                }


                CreateNewSession();
            }
        }
    }
}
