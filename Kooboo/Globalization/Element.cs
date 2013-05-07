#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Globalization;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class Element
    {
        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get;
            set;
        }
        private string _culture;
        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>
        /// The culture.
        /// </value>
        public string Culture
        {
            get
            {
                if (string.IsNullOrEmpty(_culture))
                {
                    _culture = "en-US";
                }
                return _culture;
            }
            set
            {
                _culture = value;
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category
        {
            get;
            set;
        }

        static Element _Empty;
        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
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

        #endregion

        #region Methods
        /// <summary>
        /// Gets the culture info.
        /// </summary>
        /// <returns></returns>
        public virtual CultureInfo GetCultureInfo()
        {
            return System.Globalization.CultureInfo.GetCultureInfo(this.Culture);
        }
        #endregion
    }
}
