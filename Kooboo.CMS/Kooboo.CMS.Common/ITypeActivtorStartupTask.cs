#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common
{
    public class ITypeActivtorStartupTask : IStartupTask
    {
        public void Execute()
        {
            TypeActivator.CreateInstanceMethod = (type) =>
            {
                return Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve(type);
            };
            TypeActivator.CreateInstancesMethod = (type) =>
            {
                return Kooboo.CMS.Common.Runtime.EngineContext.Current.ResolveAll(type);
            };
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
