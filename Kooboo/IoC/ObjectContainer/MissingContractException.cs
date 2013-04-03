using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    [Serializable]
    public class MissingContractException : ApplicationException
    {

        public MissingContractException() { }

        public MissingContractException(Type type)
            : this("Can not find any exports for \"" + type.ToString() + "\".")
        {

        }

        public MissingContractException(string message) : base(message) { }
        public MissingContractException(string message, Exception inner) : base(message, inner) { }
        protected MissingContractException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

   
}
