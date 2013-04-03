using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Services
{
    public class FriendlyException : Exception
    {
        public FriendlyException(string msg) : base(msg) { }
        public FriendlyException(string msg, Exception inner) : base(msg, inner) { }
    }
}
