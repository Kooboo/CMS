#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow.Args
{
    public class PreMapPageEventArgs : EventArgs
    {
        public PreMapPageEventArgs(ControllerContext controllerContext, Site site)
        {
            this.ControllerContext = controllerContext;
            this.Site = site;
        }
        public ControllerContext ControllerContext { get; private set; }
        public Site Site { get; private set; }


    }
}
