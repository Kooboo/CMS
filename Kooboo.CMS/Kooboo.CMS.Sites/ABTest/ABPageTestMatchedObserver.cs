#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.ABTest
{
    [Dependency(typeof(IABPageMatchedObserver), Key = "GoingPageObserver")]
    public class ABPageTestMatchedObserver : IABPageMatchedObserver
    {     
        public void OnMatched(PageMatchedContext matchedContext)
        {
            ABPageTestTrackingHelper.SetABTestPageCookie(matchedContext);
        }
    }
}
