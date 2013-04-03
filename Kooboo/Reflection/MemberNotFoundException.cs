using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Reflection
{
    public class MemberNotFoundException : Exception, IKoobooException
    {
        public MemberNotFoundException(string msg)
            : base(msg)
        {
        }
        public MemberNotFoundException(string msg, Exception inner)
            : base(msg, inner)
        {
        }

        public string PropertyName { get; set; }
        public Type Type { get; set; }
    }
}
