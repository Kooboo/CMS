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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public class Navigation : IComparable
    {
        public bool Show { get; set; }
        public string DisplayText { get; set; }
        public int Order { get; set; }

        private bool? showInCrumb;
        public bool? ShowInCrumb
        {
            get
            {
                if (showInCrumb == null)
                {
                    showInCrumb = true;
                }
                return showInCrumb;
            }
            set
            {
                showInCrumb = value;
            }
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            var nav = (Navigation)obj;
            if (this.DisplayText == nav.DisplayText)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(this.DisplayText))
            {
                return -1;
            }
            return this.DisplayText.CompareTo(nav.DisplayText);
        }
        #endregion
    }
}
