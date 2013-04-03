using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites
{
    public class SiteRepositoryNotExists : KoobooException
    {
        public SiteRepositoryNotExists()
            : base("The site repository doest not exists.")
        {

        }
    }
}
