using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Kooboo.Globalization
{
    public class Element
    {
        public string Name
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }
		private string _culture ;
        public string Culture
        {
            get{
				if(string.IsNullOrEmpty (_culture))
				{
					_culture="en-US";					
				}
				return _culture;
			}
            set{
				_culture = value;
			}
        }

        
        public virtual CultureInfo GetCultureInfo()
        {
             return System.Globalization.CultureInfo.GetCultureInfo(this.Culture);     
        }

        public string Category
        {
            get;
            set;
        }

        static Element _Empty;
        public static Element Empty
        {
            get
            {
                if (_Empty == null)
                {
                    _Empty = new Element
                    {
                        Name = "",
                        Value = "",
                        Category = "",
                        Culture = ""
                    };
                }

                return _Empty;
            }
        }
        
    }
}
