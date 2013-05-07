#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ITimeZoneHelper))]
    public class TimeZoneHelper : ITimeZoneHelper
    {
        public virtual TimeZoneInfo GetCurrentTimeZone()
        {
            if (Site.Current != null && !string.IsNullOrEmpty(Site.Current.TimeZoneId))
            {
                return FindTimeZoneById(Site.Current.TimeZoneId);
            }
            else
            {
                return TimeZoneInfo.Local;
            }
        }

        public virtual TimeZoneInfo FindTimeZoneById(string id)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        public virtual IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        public virtual DateTime ConvertToUtcTime(DateTime dt)
        {
            dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
            var currentTimeZoneInfo = GetCurrentTimeZone();
            return TimeZoneInfo.ConvertTimeToUtc(dt, currentTimeZoneInfo);
        }

        public virtual DateTime ParseDateTime(string s)
        {
            return DateTime.Parse(s);
        }
        public virtual bool TryParseDateTime(string s, out DateTime dt)
        {
            return DateTime.TryParse(s, out dt);
        }
        public virtual DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            var currentTimeZoneInfo = GetCurrentTimeZone();

            var local = TimeZoneInfo.ConvertTime(dt, sourceTimeZone, currentTimeZoneInfo);

            return DateTime.SpecifyKind(local, DateTimeKind.Local);
        }
    }
}
