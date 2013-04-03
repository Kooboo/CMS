using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Html
{
    [Serializable]
    public class InvalidHtmlException : Exception
    {
        public InvalidHtmlException() { }
        public InvalidHtmlException(string message) : base(message) { }
        public InvalidHtmlException(string message, Exception inner) : base(message, inner) { }
        protected InvalidHtmlException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
