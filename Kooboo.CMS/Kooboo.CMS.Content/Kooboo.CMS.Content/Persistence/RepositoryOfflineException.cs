using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence
{
    public class RepositoryOfflineException : KoobooException
    {
        public RepositoryOfflineException()
            : base("The repository is offline.")
        {

        }
    }
}
