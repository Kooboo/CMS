using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Caching
{
    public interface INotifyCacheExpired
    {
        void Notify(string objectCacheName, string key);
    }
}
