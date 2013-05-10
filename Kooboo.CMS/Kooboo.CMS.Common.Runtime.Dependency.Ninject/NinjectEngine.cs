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
using System.Configuration;
using System.Linq;
using Ninject;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Common.Runtime.Dependency.Ninject
{
    public class NinjectEngine : IEngine
    {
        #region Ctor

        public NinjectEngine()
            : this(new WebAppTypeFinder() { AssemblySkipLoadingPattern = "Kooboo,|System\\." })
        {

        }
        public NinjectEngine(ITypeFinder typeFinder)
            : this(typeFinder, new ContainerManager())
        { }
        public NinjectEngine(ITypeFinder typeFinder, ContainerManager containerManager)
        {
            if (typeFinder == null)
            {
                throw new ArgumentNullException("typeFinder");
            }
            this.TypeFinder = typeFinder;
            this.ContainerManager = containerManager;
            InitializeContainer();
        }

        #endregion

        #region Utilities

        private void RunStartupTasks()
        {
            var startUpTaskTypes = this.TypeFinder.FindClassesOfType<IStartupTask>();
            var startUpTasks = new List<IStartupTask>();
            foreach (var startUpTaskType in startUpTaskTypes)
                startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
            //sort
            startUpTasks = startUpTasks.AsQueryable().OrderBy(st => st.Order).ToList();
            foreach (var startUpTask in startUpTasks)
                startUpTask.Execute();
        }

        private void InitializeContainer()
        {
            //register attributed dependency
            var attrDependency = new DependencyAttributeRegistrator(this.TypeFinder, this.ContainerManager);
            attrDependency.RegisterServices();

            //
            var drTypes = this.TypeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(this.ContainerManager, this.TypeFinder);

        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize components and plugins in the environment.
        /// </summary>
        /// <param name="config">Config</param>
        public void Initialize()
        {
            RunStartupTasks();
        }

        #region Resolve
        public T Resolve<T>() where T : class
        {
            return ContainerManager.Resolve<T>();
        }

        public T Resolve<T>(string name) where T : class
        {
            return ContainerManager.Resolve<T>(name);
        }

        public object Resolve(Type type, string name)
        {
            return ContainerManager.Resolve(type, name);
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }
        #endregion

        #region TryResolve
        public T TryResolve<T>() where T : class
        {
            return ContainerManager.TryResolve<T>();
        }

        public T TryResolve<T>(string name) where T : class
        {
            return ContainerManager.TryResolve<T>(name);
        }

        public object TryResolve(Type type, string name)
        {
            return ContainerManager.TryResolve(type, name);
        }

        public object TryResolve(Type type)
        {
            return ContainerManager.TryResolve(type);
        }
        #endregion

        public IEnumerable<object> ResolveAll(Type serviceType)
        {
            return ContainerManager.ResolveAll(serviceType);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion

        #region Properties

        public IContainerManager ContainerManager
        {
            get;
            private set;
        }
        public ITypeFinder TypeFinder { get; private set; }
        #endregion
    }
}
