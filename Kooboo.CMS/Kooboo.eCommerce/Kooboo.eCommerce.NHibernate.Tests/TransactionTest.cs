#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Kooboo.CMS.eCommerce.Models.Catalog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NH=NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;

namespace Kooboo.eCommerce.NHibernate.Tests
{
    [TestClass]
    public class TransactionTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var config = new NH.Configuration().Configure();
            
            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(SessionFactory).Assembly.GetTypes());
            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            SchemaUpdate su = new SchemaUpdate(config);
            //se.Execute(false, false, true);
            su.Execute(true, true);

            var factory = config.BuildSessionFactory();

            using (var session = factory.OpenSession())
            {
                using (var tran = session.BeginTransaction())
                {
                    session.Save(new Brand { Name = "Brand 2", UtcCreationDate = DateTime.UtcNow, UtcUpdateDate = DateTime.UtcNow });
                    tran.Commit();
                }
            }

            factory.Dispose();
        }
    }
}
