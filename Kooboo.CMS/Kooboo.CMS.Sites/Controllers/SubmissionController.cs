#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers
{
    public class SubmissionController : FrontControllerBase
    {
        #region .ctor
        SubmissionSettingManager _submissionSettingManager;
        public SubmissionController(SubmissionSettingManager submissionSettingManager)
        {
            this._submissionSettingManager = submissionSettingManager;
        }
        #endregion

        #region Submit
        [ValidateInput(false)]
        public ActionResult Submit(string submissionName)
        {
            if (this.ControllerContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                System.Web.Helpers.AntiForgery.Validate();
            }
            if (string.IsNullOrEmpty(submissionName))
            {
                throw new ArgumentNullException("submissionName");
            }
            var submissionSetting = _submissionSettingManager.Get(Site, submissionName);
            if (submissionSetting == null)
            {
                throw new ArgumentNullException("The submission setting does not exists.");
            }
            var pluginType = Type.GetType(submissionSetting.PluginType);
            var submissionPlugin = (ISubmissionPlugin)TypeActivator.CreateInstance(pluginType);
            return submissionPlugin.Submit(Site, ControllerContext, submissionSetting);

        }
        #endregion
    }
}
