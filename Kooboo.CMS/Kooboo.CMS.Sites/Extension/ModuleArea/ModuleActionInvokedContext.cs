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
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleActionInvokedContext
    {
        public ModuleActionInvokedContext(ControllerContext controllerContext, ActionResult actionResult)
        {
            this.ControllerContext = controllerContext;
            this.ActionResult = actionResult;
        }
        public ControllerContext ControllerContext { get; private set; }
        public virtual ActionResult ActionResult { get; private set; }
    }
}
