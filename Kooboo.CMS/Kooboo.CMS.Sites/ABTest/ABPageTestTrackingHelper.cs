#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.ABTest
{
    public static class ABPageTestTrackingHelper
    {
        public static string ComposeTrackingToken(PageMatchedContext matchedContext)
        {
            var trackingValue = string.Format("{0}||{1}", matchedContext.ABPageSetting.UUID, matchedContext.MatchedPage.FullName);
            var trackingToken = SecurityHelper.Encrypt(matchedContext.Site, trackingValue);
            return trackingToken;
        }
        public static void ParseTrackingToken(Site site, string trackingToken, out string pageVisitRuleName, out string matchedPage)
        {
            pageVisitRuleName = null;
            matchedPage = null;

            if (!string.IsNullOrEmpty(trackingToken))
            {
                try
                {
                    string trackingValue = SecurityHelper.Decrypt(site, trackingToken);

                    string[] values = trackingValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                    if (values.Length > 0)
                    {
                        pageVisitRuleName = values[0];
                        matchedPage = values[1];
                    }
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }
            }
        }
    }
}
