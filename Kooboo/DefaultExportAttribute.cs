using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Kooboo
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class DefaultExportAttribute : ExportAttribute
    {
        public DefaultExportAttribute()
        {
            this.IsDefault = false;
        }

        public bool IsDefault
        {
            get;
            set;
        }

    }
}
