using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Models.Paths
{
    public static class PathFactory
    {
        public static IPath<T> GetPath<T>()
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IPath<T>>();
        }

        public static IPath<T> GetPath<T>(T o)
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IPath<T>>(new Kooboo.CMS.Common.Runtime.Parameter("entity", o));
        }
    }
}
