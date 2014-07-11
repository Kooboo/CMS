#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency.Ninject.InRequestScope;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
[assembly: System.Web.PreApplicationStartMethod(typeof(Kooboo.Common.ObjectContainer.Dependency.Ninject.NinjectWebCommonStartupTask), "Start")]
namespace Kooboo.Common.ObjectContainer.Dependency.Ninject
{
    public class NinjectWebCommonStartupTask
    {
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
        }
    }
}
