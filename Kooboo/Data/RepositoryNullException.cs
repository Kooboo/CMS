using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Data
{
    public class RepositoryNullException : Exception, IKoobooException
    {
        public RepositoryNullException(string msg)
            : base(msg)
        {
        }
        public RepositoryNullException(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
