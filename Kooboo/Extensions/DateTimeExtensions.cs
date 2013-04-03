using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToRfc1123(this DateTime dateTime)
        {
            return dateTime.ToString("r");
        }
    }
}
