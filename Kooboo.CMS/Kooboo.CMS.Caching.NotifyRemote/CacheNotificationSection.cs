using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace Kooboo.CMS.Caching.NotifyRemote
{
    public class CacheNotificationSection : ConfigurationSection
    {
        static string SectionName = "cache.notify";
        [ConfigurationProperty("servers", IsDefaultCollection = false)]
        public ServerItemElementCollection Servers
        {
            get
            {
                ServerItemElementCollection itemsCollection =
                (ServerItemElementCollection)base["servers"];
                return itemsCollection;
            }
        }

        public static CacheNotificationSection GetSection()
        {
            return (CacheNotificationSection)ConfigurationManager.GetSection(SectionName);
        }
    }
}
