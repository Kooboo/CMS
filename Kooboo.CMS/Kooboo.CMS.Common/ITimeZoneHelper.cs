#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common
{
    public interface ITimeZoneHelper
    {
        TimeZoneInfo GetCurrentTimeZone();

        TimeZoneInfo FindTimeZoneById(string id);

        IEnumerable<TimeZoneInfo> GetTimeZones();

        DateTime ConvertToUtcTime(DateTime dt);

        DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone);
     
    }

    public static class TimeZoneHelper
    {
        public static TimeZoneInfo GetCurrentTimeZone()
        {
            return EngineContext.Current.Resolve<ITimeZoneHelper>().GetCurrentTimeZone();
        }

        public static TimeZoneInfo FindTimeZoneById(string id)
        {
            return EngineContext.Current.Resolve<ITimeZoneHelper>().FindTimeZoneById(id);
        }

        public static IEnumerable<TimeZoneInfo> GetTimeZones()
        {
            return EngineContext.Current.Resolve<ITimeZoneHelper>().GetTimeZones();
        }

        public static DateTime ConvertToUtcTime(DateTime dt)
        {
            return EngineContext.Current.Resolve<ITimeZoneHelper>().ConvertToUtcTime(dt);
        }

        public static DateTime ConvertToLocalTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            return EngineContext.Current.Resolve<ITimeZoneHelper>().ConvertToLocalTime(dt,sourceTimeZone);
        }

       
    }
}
