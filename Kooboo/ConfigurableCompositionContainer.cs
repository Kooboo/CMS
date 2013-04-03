using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Kooboo
{
    public class ConfigurableCompositionContainer:ExportProvider
    {
        CompositionContainer CompositionContainer
        {
            get;
            set;
        }

        public ConfigurableCompositionContainer()
            : this(null, new ExportProvider[0])
        {
        }

    
        public ConfigurableCompositionContainer(params ExportProvider[] providers)
            : this(null, providers)
        {
        }

     
        public ConfigurableCompositionContainer(ComposablePartCatalog catalog, params ExportProvider[] providers)
            : this(catalog, false, providers)
        {
        }

        public ConfigurableCompositionContainer(ComposablePartCatalog catalog, bool isThreadSafe, params ExportProvider[] providers)
        {
            this.CompositionContainer = new CompositionContainer(catalog, isThreadSafe, providers);
        }



        public Lazy<T> GetExportFirstOrConfigured<T>()
        {

            return this.GetExport<T>("__DefaultOrConfigured__");
        }

        public Lazy<T> GetExportFirstOrConfigured<T>(string contractName)
        {
            return this.GetExport<T>("__DefaultOrConfigured__" + contractName);
        }
    

        protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
        {
            throw new NotImplementedException();
        }
    }
}
