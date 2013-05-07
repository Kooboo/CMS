#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region Usings
using System.Configuration;
#endregion


namespace Kooboo.CMS.Caching.NotifyRemote
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheNotificationSection : ConfigurationSection
    {
        #region Consts
        const string SECTION_NAME = "cache.notify";
        #endregion

        #region Properties
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
        #endregion

        #region Methods
        public static CacheNotificationSection GetSection()
        {
            return (CacheNotificationSection)ConfigurationManager.GetSection(SECTION_NAME);
        }
        #endregion
    }
}
