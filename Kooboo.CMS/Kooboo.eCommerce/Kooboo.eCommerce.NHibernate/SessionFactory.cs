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
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg;
using NHibernate;
using NHibernate.Linq;
using NH = NHibernate.Cfg;

namespace Kooboo.eCommerce.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public static class SessionFactory
    {
        static ModelMapper mapper = new ModelMapper();
        static NH.Configuration configuraion = new NH.Configuration();
        static SessionFactory()
        {
            mapper.AddMappings(typeof(SessionFactory).Assembly.GetTypes());

            configuraion.Configure();

            configuraion.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

        }

        /// <summary>
        /// Get Configuration
        /// </summary>
        /// <returns></returns>
        public static NH.Configuration GetConfiguration()
        {
            return configuraion;
        }
        /// <summary>
        /// Creates the session factory.
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory CreateSessionFactory()
        {
            return configuraion.BuildSessionFactory();
        }
        /// <summary>
        /// Creates the session.
        /// </summary>
        /// <returns></returns>
        public static ISession CreateSession()
        {
            return CreateSessionFactory().OpenSession();
        }
        
    }
}
