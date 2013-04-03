using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Kooboo.IoC
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class OrderedExportAttribute : ExportAttribute
    {
        public OrderedExportAttribute()
            : base()
        {
            
        }

        public OrderedExportAttribute(string contractName, int index)
            : base(contractName)
        {
            this.Index = index;
        }

        public OrderedExportAttribute(Type contractType,int index)
            : base(contractType)
        {
            this.Index = index;
        }

        public OrderedExportAttribute(string contractName, Type contractType, int index)
            : base(contractName, contractType)
        {
            this.Index = index;
        }


        public int Index
        {
            get;
            set;
        }
    }
}
