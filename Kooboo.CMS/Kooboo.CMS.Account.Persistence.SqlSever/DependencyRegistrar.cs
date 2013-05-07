#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Account.Persistence.SqlSever
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddComponent(typeof(IRoleProvider), typeof(RoleProvider));
            containerManager.AddComponent(typeof(IProvider<Role>), typeof(RoleProvider));

            containerManager.AddComponent(typeof(IUserProvider), typeof(UserProvider));
            containerManager.AddComponent(typeof(IProvider<User>), typeof(UserProvider));

        }

        public int Order
        {
            get { return 1; }
        }
    }
}
