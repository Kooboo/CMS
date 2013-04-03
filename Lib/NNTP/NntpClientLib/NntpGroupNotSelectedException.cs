using System;
using System.Runtime.Serialization;

namespace NntpClientLib
{
    [Serializable]
    public class NntpGroupNotSelectedException : NntpException
    {
        public NntpGroupNotSelectedException() { }
        public NntpGroupNotSelectedException(string message) : base(message) { }
        public NntpGroupNotSelectedException(string message, Exception inner) : base(message, inner) { }
        protected NntpGroupNotSelectedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}

