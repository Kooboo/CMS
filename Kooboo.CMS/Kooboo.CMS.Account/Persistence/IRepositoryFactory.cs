#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Common.Infrastructure;
using Kooboo.CMS.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account.Persistence
{
    public interface IRepositoryFactory
    {
        T GetRepository<T>()
            where T : class;
    }
    public static class RepositoryFactory
    {
        static IRepositoryFactory factory;
        [Obsolete("修改注入方式，参考默认实现IDependencyRegistrar来动态注入对象到容器")]
        public static IRepositoryFactory Factory
        {
            get
            {
                return factory;
            }
            set
            {
                factory = value;

                //这是兼容旧的注册方式而存在的，把旧的实现方式注册的实例放到容器中。

                var defaultEngine = (DefaultEngine)EngineContext.Current;
                defaultEngine.ContainerManager.AddComponentInstance(typeof(IRoleProvider), factory.GetRepository<IRoleProvider>());
                defaultEngine.ContainerManager.AddComponentInstance(typeof(IProvider<Role>), factory.GetRepository<IRoleProvider>());

                defaultEngine.ContainerManager.AddComponentInstance(typeof(IUserProvider), factory.GetRepository<IUserProvider>());
                defaultEngine.ContainerManager.AddComponentInstance(typeof(IProvider<User>), factory.GetRepository<IUserProvider>());

                defaultEngine.ContainerManager.AddComponentInstance(typeof(ISettingProvider), factory.GetRepository<ISettingProvider>());
                defaultEngine.ContainerManager.AddComponentInstance(typeof(IProvider<Setting>), factory.GetRepository<ISettingProvider>());
            }
        }
        [Obsolete("V4中，应该是使用构造器依赖或属性依赖注入")]
        public static IRoleProvider RoleRepository
        {
            get
            {
                return EngineContext.Current.Resolve<IRoleProvider>();
            }
        }
        [Obsolete("V4中，应该是使用构造器依赖或属性依赖注入")]
        public static IUserProvider UserRepository
        {
            get
            {
                return EngineContext.Current.Resolve<IUserProvider>();
            }
        }
        [Obsolete("V4中，应该是使用构造器依赖或属性依赖注入")]
        public static ISettingProvider SettingRepository
        {
            get
            {
                return EngineContext.Current.Resolve<ISettingProvider>();
            }
        }
    }
}
