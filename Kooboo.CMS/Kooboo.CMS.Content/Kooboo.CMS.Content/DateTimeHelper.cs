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

namespace Kooboo.CMS.Content
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Parses from ticks OR datetime string.
        /// </summary>
        /// <param name="strValue">The STR value.</param>
        /// <returns></returns>
        public static DateTime Parse(string strValue)
        {
            DateTime dt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(strValue))
            {
                long ticks = 0;
                if (long.TryParse(strValue, out ticks))
                {
                    dt = new DateTime(ticks);
                }
                else
                {
                    DateTime.TryParse(strValue, out dt);
                }
            }

            return dt;
        }
        public static bool TryParse(string strValue, out DateTime dt)
        {
            dt = DateTime.Now;
            if (!string.IsNullOrEmpty(strValue))
            {
                long ticks = 0;
                if (long.TryParse(strValue, out ticks))
                {
                    dt = new DateTime(ticks);
                    return true;
                }
                else
                {
                    return DateTime.TryParse(strValue, out dt);
                }
            }
            return false;
        }
    }
}
