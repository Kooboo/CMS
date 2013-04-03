using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Kooboo.Globalization
{
    public class CurrencyInfo
    {
        static List<CurrencyInfo> _Currencies;
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

        public static CurrencyInfo GetCurrencyInfo(string englishName)
        {
            return GetCurrencies()
                .Where(i => i.EnglishName == englishName)
                .FirstOrDefault();
        }

        public static CurrencyInfo GetCurrencyInfoByISOSymbol(string isoSymbol)
        {
            return GetCurrencies()
              .Where(i => i.ISOSymbol == isoSymbol)
              .FirstOrDefault();
        }

        public string EnglishName
        {
            get;
            set;
        }

        public string NativeName
        {
            get;
            set;
        }

        public string Symbol
        {
            get;
            set;
        }

        public string ISOSymbol
        {
            get;
            set;
        }
    }
}
