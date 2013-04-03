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
                cachePolicy.AbsoluteExpiration = DateTime.UtcNow.AddSeconds(Duration);
            }
            else
            {
                cachePolicy.SlidingExpiration = TimeSpan.FromSeconds(Duration);
            }
            return cachePolicy;
        }
    }
}
