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

using Kooboo.Web.Mvc;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Query;
using Kooboo.Extensions;
using Kooboo.CMS.Web.Authorizations;


namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [RequiredLogOn]
    public class HomeController : ControllerBase
    {
        #region .ctor
        RepositoryManager _manager;
        public HomeController(RepositoryManager manager)
        {
            _manager = manager;
        }
        #endregion
        


        

    }
}
