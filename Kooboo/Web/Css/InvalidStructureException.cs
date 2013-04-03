using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    [Serializable]
    public class InvalidStructureException : Exception
    {
        public InvalidStructureException() { }
        public InvalidStructureException(string message) : base(message) { }
        public InvalidStructureException(string message, Exception inner) : base(message, inner) { }
        protected InvalidStructureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
