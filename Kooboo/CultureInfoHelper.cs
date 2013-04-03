using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Kooboo
{
    /// <summary>
    /// 
    /// </summary>
    public class CultureInfoHelper
    {
        public static CultureInfo CreateCultureInfo(string cultureName)
        {
            var culture = new CultureInfo("en-US");
            try
            {
                culture = new CultureInfo(cultureName);
            }
            catch (Exception e)
            {
            }
            return culture;

        }
    }
}
