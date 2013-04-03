using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public sealed class LifetimeManagers
    {
        static LifetimeManagers()
        {
            Default = () => new PerRequestLifetimeManager();
            Shared = new SharedLifetimeManager();
        }

        public static Func<ILifetimeManager> Default
        {
            get;
            set;
        }

        public static ILifetimeManager Shared
        {
            get;
            set;
        }
    }
}
