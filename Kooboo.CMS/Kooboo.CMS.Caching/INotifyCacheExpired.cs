#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

namespace Kooboo.CMS.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotifyCacheExpired
    {
        /// <summary>
        /// Notifies the specified object cache name.
        /// </summary>
        /// <param name="objectCacheName">Name of the object cache.</param>
        /// <param name="key">The key.</param>
        void Notify(string objectCacheName, string key);
    }
}
