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
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.View
{
    /// <summary>
    /// 前台添加，删除，修改数据的辅助类
    /// </summary>
    [Obsolete("Please use  Url.FrontUrl().SubmissionUrl(\"submissionName\") for generate a submission url.")]
    public class SubmissionHelper
    {
        [Obsolete("Please use new SubmssionSetting function for security reason.")]
        public static string CreateContentUrl()
        {
            return GenerateSubmissionUrl(() =>
                Page_Context.Current.Url.Action("Create", new { controller = "ContentService", area = "Contents", siteName = Page_Context.Current.PageRequestContext.Site.FullName, repositoryName = Repository.Current.Name }));
        }
        private static string GenerateSubmissionUrl(Func<string> func)
        {
            return func();
        }
        [Obsolete("Please use new SubmssionSetting function for security reason.")]
        public static string UpdateContentUrl()
        {
            return GenerateSubmissionUrl(() =>
                Page_Context.Current.Url.Action("Update", new { controller = "ContentService", area = "Contents", siteName = Page_Context.Current.PageRequestContext.Site.FullName, repositoryName = Repository.Current.Name }));
        }
        [Obsolete("Please use new SubmssionSetting function for security reason.")]
        public static string DeleteContentUrl(string schemaName, string uuid)
        {
            return GenerateSubmissionUrl(() =>
                Page_Context.Current.Url.Action("Delete", new { controller = "ContentService", area = "Contents", siteName = Page_Context.Current.PageRequestContext.Site.FullName, repositoryName = Repository.Current.Name, schemaName = schemaName, uuid = uuid }));

        }
        [Obsolete("Please use new SubmssionSetting function for security reason.")]
        public static string EmailUrl()
        {
            return GenerateSubmissionUrl(() =>
                Page_Context.Current.Url.Action("Email", new { controller = "Submission", area = "Sites", siteName = Page_Context.Current.PageRequestContext.Site.FullName, repositoryName = Repository.Current.Name }));
        }
    }
}
