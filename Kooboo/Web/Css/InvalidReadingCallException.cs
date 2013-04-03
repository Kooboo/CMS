using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    [Serializable]
    public class InvalidReadingCallException : Exception
    {
        public InvalidReadingCallException() { }
        public InvalidReadingCallException(string message) : base(message) { }
        public InvalidReadingCallException(string message, Exception inner) : base(message, inner) { }
        protected InvalidReadingCallException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
