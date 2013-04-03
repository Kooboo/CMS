using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.Grid
{
    public class PageSizeAttribute : Attribute
    {
        string pageIndexName = "page";
        public string PageIndexName
        {
            get
            {
                return pageIndexName;
            }
            set
            {
                pageIndexName = value;
            }
        }

        string sizeOptions = "25,50,100";
        /// <summary>
        /// Gets or sets the size options.
        /// <example>25,50,100</example>
        /// </summary>
        /// <value>The size options.</value>
        public string SizeOptions
        {
            get
            {
                return sizeOptions;
            }
            set
            {
                sizeOptions = value;
            }
        }
    }
}
