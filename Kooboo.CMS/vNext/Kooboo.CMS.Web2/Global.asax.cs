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
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Kooboo.Common.Globalization;

using Kooboo.CMS.Web2.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Permissions;
using System.Security;
using Kooboo.Collections;
using Kooboo.Common.ObjectContainer;

namespace Kooboo.CMS.Web2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : Kooboo.Common.Web.HttpApplicationEx
    {
    }
}