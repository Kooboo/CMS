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

namespace Kooboo.eCommerce.NHibernate.Tests.Persistence
{
    public class NoDI_NHibernateContextFactory : NHibernateContextFactory
    {
        static NHibernateContext dbContext = new NHibernateContext();
        public override NHibernateContext GetDbContext()
        {
            return dbContext;
        }
    }
}
