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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.eCommerce.NHibernate
{
    [Dependency(typeof(IDbContextFactory))]
    public class NHibernateContextFactory : IDbContextFactory
    {
        IDbContext IDbContextFactory.GetDbContext()
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<NHibernateContext>();
        }
        public virtual NHibernateContext GetDbContext()
        {
            return (NHibernateContext)((IDbContextFactory)this).GetDbContext();
        }
    }
}
