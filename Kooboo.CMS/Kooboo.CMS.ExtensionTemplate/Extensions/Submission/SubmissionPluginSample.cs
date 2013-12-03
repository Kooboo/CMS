using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.View;


namespace Kooboo.CMS.ExtensionTemplate.Extensions.Submission
{
    //uncomment below code to use this as an example

    //public class SubmissionPluginSample : ISubmissionPlugin
    //{
    //    public Dictionary<string, object> Parameters
    //    {
    //        //Add your own submission available parameters here. 
    //        get { return new Dictionary<string, object>() { { "Parameter1", "defaultvalue1" }}; }
    //    }

    //    public System.Web.Mvc.ActionResult Submit(Sites.Models.Site site, System.Web.Mvc.ControllerContext controllerContext, Sites.Models.SubmissionSetting submissionSetting)
    //    {
    //        // access to the submission form value
    //        var name = controllerContext.HttpContext.Request.Form["name"];

    //        // access the parameter values. 
    //        var parameter1 = submissionSetting.Settings["Parameter1"];

    //        // process your business logic below.

    //        return new System.Web.Mvc.JsonResult() { Data = true };
    //    }
    //}
}