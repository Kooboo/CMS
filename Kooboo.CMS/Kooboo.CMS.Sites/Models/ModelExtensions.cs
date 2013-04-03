using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.Models
{
    public static class ModelExtensions
    {
        public static Repository GetRepository(this Site site)
        {
            site = site.AsActual();
            if (!string.IsNullOrEmpty(site.Repository))
            {
                return new Repository(site.Repository);
            }
            return null;
        }
    }
}
