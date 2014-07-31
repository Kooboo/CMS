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
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI.Html
{
    public class FrontHtmlHelper : IFrontHtmlHelper
    {
        #region .ctor
        public HtmlHelper Html
        {
            get;
            private set;
        }

        public Page_Context Page_Context
        {
            get;
            private set;
        }
        public FrontHtmlHelper(Page_Context context, HtmlHelper html)
        {
            Page_Context = context;
            Html = html;
        }
        #endregion
    }
}
