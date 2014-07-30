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
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class SubmissionUrlExtensions
    {
        public static IHtmlString SubmissionUrl(this FrontUrlHelper frontUrl, string submissionName)
        {
            return SubmissionUrl(frontUrl, submissionName, null);
        }
        public static IHtmlString SubmissionUrl(this FrontUrlHelper frontUrl, string submissionName, object routeValues)
        {
            var routes = RouteValuesHelper.GetRouteValues(routeValues);
            routes["SubmissionName"] = submissionName;
            return frontUrl.WrapperUrl(frontUrl.Url.Action("Submit", "Submission", routes));
        }
    }
}
