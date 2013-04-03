using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Reflection
{
    public class MemberException : Exception, IKoobooException
    {
        public MemberException(string msg)
            : base(msg)
        {
        }
        public MemberException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
