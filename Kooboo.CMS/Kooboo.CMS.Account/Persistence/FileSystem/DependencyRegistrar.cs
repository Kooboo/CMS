#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Infrastructure.DependencyManagement;
using Kooboo.CMS.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence.FileSystem
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerManager containerManager, Common.Infrastructure.ITypeFinder typeFinder)
        {
            containerManager.AddComponentInstance(typeof(IRoleProvider), typeof(RoleProvider));
            containerManager.AddComponentInstance(typeof(IProvider<Role>), typeof(RoleProvider));

            containerManager.AddComponentInstance(typeof(IUserProvider), typeof(UserProvider));
            containerManager.AddComponentInstance(typeof(IProvider<User>), typeof(UserProvider));

            containerManager.AddComponentInstance(typeof(ISettingProvider), typeof(SettingProvider));
            containerManager.AddComponentInstance(typeof(IProvider<Setting>), typeof(SettingProvider));
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
