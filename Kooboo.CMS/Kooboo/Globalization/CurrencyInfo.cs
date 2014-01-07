#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class CurrencyInfo
    {
        #region Fields
        static List<CurrencyInfo> _Currencies;
        #endregion

        #region Methods
        /// <summary>
        /// Gets the currencies.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CurrencyInfo> GetCurrencies()
        {
            if (_Currencies == null)
            {
                lock (typeof(CurrencyInfo))
                {
                    if (_Currencies == null)
                    {
                        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures);

                        var list = new List<CurrencyInfo>();
                        //loop through all the cultures found
                        foreach (CultureInfo culture in cultures)
                        {
                            //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                            //to the RegionInfo contructor to gain access to the information for that culture
                            try
                            {
                                RegionInfo region = new RegionInfo(culture.LCID);

                                if (list.Any(i => i.EnglishName == region.CurrencyEnglishName) == false)
                                {
                                    list.Add(new CurrencyInfo
                                    {
                                        EnglishName = region.CurrencyEnglishName,
                                        NativeName = region.CurrencyNativeName,
                                        Symbol = region.CurrencySymbol,
                                        ISOSymbol = region.ISOCurrencySymbol
                                    });
                                }
                            }
                            catch
                            {
                                //next 
                            }
                        }

                        _Currencies = list;
                    }
                }
            }

            return _Currencies;
        }

        /// <summary>
        /// Gets the currency info.
        /// </summary>
        /// <param name="englishName">Name of the english.</param>
        /// <returns></returns>
        public static CurrencyInfo GetCurrencyInfo(string englishName)
        {
            return GetCurrencies()
                .Where(i => i.EnglishName == englishName)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the currency info by ISO symbol.
        /// </summary>
        /// <param name="isoSymbol">The iso symbol.</param>
        /// <returns></returns>
        public static CurrencyInfo GetCurrencyInfoByISOSymbol(string isoSymbol)
        {
            return GetCurrencies()
              .Where(i => i.ISOSymbol == isoSymbol)
              .FirstOrDefault();
        }

        /// <summary>
        /// Gets or sets the name of the english.
        /// </summary>
        /// <value>
        /// The name of the english.
        /// </value>
        public string EnglishName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the native.
        /// </summary>
        /// <value>
        /// The name of the native.
        /// </value>
        public string NativeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        /// <value>
        /// The symbol.
        /// </value>
        public string Symbol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ISO symbol.
        /// </summary>
        /// <value>
        /// The ISO symbol.
        /// </value>
        public string ISOSymbol
        {
            get;
            set;
        }
        #endregion
    }
}
