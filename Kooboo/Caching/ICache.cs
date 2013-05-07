#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;

namespace Kooboo.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICache
    {
        #region Methods
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="callback">The callback.</param>
        void Add(string key, object value, TimeSpan slidingExpiration, CacheRemovedCallback callback = null);

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="callback">The callback.</param>
        void Add(string key, object value, DateTime absoluteExpiration, CacheRemovedCallback callback = null);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object Get(string key); 
        #endregion
    }

    #region Callback
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">The args.</param>
    public delegate void CacheRemovedCallback(CacheRemovedCallbackArgs args);

    /// <summary>
    /// 
    /// </summary>
    public class CacheRemovedCallbackArgs
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }
    } 
    #endregion
}
