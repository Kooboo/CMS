using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    [Serializable]
    public class KoobooException : Exception, IKoobooException
    {
        public KoobooException()
            : base()
        {

        }
        public KoobooException(string msg)
            : base(msg)
        {

        }
        public KoobooException(string msg, Exception exception)
            : base(msg, exception)
        { }
    }
}
