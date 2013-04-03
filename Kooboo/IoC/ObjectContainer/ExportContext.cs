using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public class ExportContext
    {
        internal ExportContext()
        {
        }

        Type Type
        {
            get;
            set;
        }

        public ExportContext Export<T>()
        {
            this.Type = typeof(T);
            return this;
        }

        public void To<TContract>(string contractName = null, int index=int.MaxValue)
        {
            this.To(typeof(TContract), contractName, index);
        }

        public ExportContext Export(Type type)
        {
            this.Type = type;
            return this;
        }

        public void To(Type contractType, string contractName = null, int index = int.MaxValue)
        {
            if (this.Type != null)
            {
                ExportProviders.Default.Export(this.Type, contractType, contractName, index);
            }
        }
    }


}
