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
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Menus
{
    public class ViewMenuItemInitializer : DataRuleMenuItemInitializer
    {
        public override string From
        {
            get { return "view"; }
        }
    }
}