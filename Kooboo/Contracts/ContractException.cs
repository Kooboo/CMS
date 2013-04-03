using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Contracts
{
    public class ContractException : Exception, IKoobooException
    {
        public ContractException(string msg)
            : base(msg)
        {
        }
        public ContractException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
