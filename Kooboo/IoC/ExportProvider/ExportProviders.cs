using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public static class ExportProviders
    {
        static ExportProviders()
        {
            Providers = new List<IExportProvider>();
            Providers.Add(new ExportProvider());
        }

        public static IExportProvider Default
        {
            get
            {
                return Providers[0];
            }
            set
            {
                Providers[0] = value;
            }
        }

        public static List<IExportProvider> Providers
        {
            get;
            set;
        }
    }
}
