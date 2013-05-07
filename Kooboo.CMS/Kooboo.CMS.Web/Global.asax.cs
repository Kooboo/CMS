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
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Permissions;
using System.Security;
using Kooboo.Collections;
using System.Web.Http;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : Kooboo.CMS.Common.HttpApplicationHooker
    {     
    }
}