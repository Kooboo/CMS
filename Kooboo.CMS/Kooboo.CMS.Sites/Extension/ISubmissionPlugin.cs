
#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension
{
    public interface ISubmissionPlugin
    {
        ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting);

        Dictionary<string, object> Parameters { get; }
    }
}
