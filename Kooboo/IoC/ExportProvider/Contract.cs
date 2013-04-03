using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public class Contract
    {
        public Contract()
        {
            this.Index = int.MaxValue;
        }

        /// <summary>
        /// the impl type
        /// </summary>
        public Type ExportType
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }
    }
}
