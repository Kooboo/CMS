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
using System.Collections;
using Kooboo.Common.ObjectContainer;

namespace Kooboo.CMS.Account.Services
{
    public static class ServiceFactory
    {

        public static RoleManager RoleManager
        {
            get
            {
                return EngineContext.Current.Resolve<RoleManager>();
            }
            set
            {
                EngineContext.Current.ContainerManager.AddComponentInstance<RoleManager>(value);
            }
        }

        public static UserManager UserManager
        {
            get
            {
                return EngineContext.Current.Resolve<UserManager>();
            }
            set
            {
               EngineContext.Current.ContainerManager.AddComponentInstance<UserManager>(value);
            }
        }
        public static T GetService<T>()
            where T : class
        {
            return EngineContext.Current.Resolve<T>();
        }
    }
}
