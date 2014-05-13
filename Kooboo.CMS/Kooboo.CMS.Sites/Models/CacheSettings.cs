#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace Kooboo.CMS.Sites.Models
{
    public enum ExpirationPolicy
    {
        AbsoluteExpiration,
        SlidingExpiration
    }
    public class CacheSettings
    {
        private bool? _enable;
        public bool? EnableCaching
        {
            get
            {
                if (!_enable.HasValue)
                {
                    _enable = false;
                }
                return _enable;
            }
            set
            {
                _enable = value;
            }
        }

        public ExpirationPolicy ExpirationPolicy { get; set; }
        /// <summary>
        /// The time, in seconds, that the page or view is cached. 
        /// </summary>
        public int Duration { get; set; }

        public CacheItemPolicy ToCachePolicy()
        {
            CacheItemPolicy cachePolicy = new CacheItemPolicy();
            if (ExpirationPolicy == Models.ExpirationPolicy.AbsoluteExpiration)
            {
                cachePolicy.AbsoluteExpiration = Duration == 0 ? DateTime.MaxValue : DateTime.UtcNow.AddSeconds(Duration);
            }
            else
            {
                cachePolicy.SlidingExpiration = Duration == 0 ? TimeSpan.FromDays(1) : TimeSpan.FromSeconds(Duration);
            }
            return cachePolicy;
        }
    }
}
