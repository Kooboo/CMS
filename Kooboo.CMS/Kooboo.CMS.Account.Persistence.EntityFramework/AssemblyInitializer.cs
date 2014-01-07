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
using System.IO;
using Kooboo.CMS.Account.Persistence.SqlSever;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.CMS.Account.Persistence.SqlSever.AssemblyInitializer), "Initialize")]
namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public static class AssemblyInitializer
    {
        public static void Initialize()
        {
            ApplicationInitialization.RegisterInitializerMethod(delegate()
            {
                Kooboo.CMS.Account.Persistence.RepositoryFactory.Factory = new Kooboo.CMS.Account.Persistence.SqlSever.RepositoryFactory();
            }, 0);
        }
    }
}
