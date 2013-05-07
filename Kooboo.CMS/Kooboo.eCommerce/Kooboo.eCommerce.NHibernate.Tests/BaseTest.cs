#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NH=NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Kooboo.eCommerce.NHibernate.Tests
{
    [TestClass]
    public class BaseTest
    {
        public ISessionFactory MySessionFactory
        {
            get
            {
                return SessionFactory.CreateSessionFactory();
            }
        }
        private NH.Configuration _configuration;

        public BaseTest()
        {
            _configuration = SessionFactory.GetConfiguration();
            SchemaUpdate su = new SchemaUpdate(_configuration);
            //se.Execute(false, false, true);
            su.Execute(true, true);
        }
    }
}
