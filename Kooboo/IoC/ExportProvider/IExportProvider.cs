using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public interface IExportProvider
    {      
        IEnumerable<Contract> FindExports(Type contractType);

        void Export(Type exportType, Type contractType, string contractName, int index);
    }
}
