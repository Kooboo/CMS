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
        /// <summary>
        /// Creates the culture info.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <returns></returns>
        public static CultureInfo CreateCultureInfo(string cultureName)
        {
            var culture = new CultureInfo("en-US");
            try
            {
                culture = new CultureInfo(cultureName);
            }
            catch
            {
            }
            return culture;

        }
    }
}
