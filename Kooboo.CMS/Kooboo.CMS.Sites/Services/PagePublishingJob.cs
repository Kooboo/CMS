#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Kooboo.CMS.Sites.Services
{
    public class PagePublishingJob : IJob
    {
        public virtual void Execute(object executionState)
        {
            foreach (var site in ServiceFactory.SiteManager.All())
            {
                PublishSitePages(site);
            }

        }
        private void PublishSitePages(Site site)
        {
            var queue = ServiceFactory.PageManager.PagePublishingProvider.All(site).Select(it => it.AsActual());
            foreach (var item in queue)
            {
                var removeQueueItem = false;
                var page = new Page(site, item.PageName).AsActual();
                if (page != null)
                {
                    if (DateTime.UtcNow > item.UtcDateToPublish)
                    {
                        if (page.Published == false)
                        {
                            ServiceFactory.PageManager.Publish(page, item.PublishDraft, item.UserName);
                            removeQueueItem = true;
                        }

                        if (item.Period)
                        {
                            removeQueueItem = false;
                            if (DateTime.UtcNow > item.UtcDateToOffline)
                            {
                                if (page.Published == true)
                                {
                                    ServiceFactory.PageManager.Unpublish(page, item.UserName);
                                    removeQueueItem = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    removeQueueItem = true;
                }
                if (removeQueueItem)
                {
                    ServiceFactory.PageManager.PagePublishingProvider.Remove(item);
                }
            }
        }
        public virtual void Error(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
        }
    }
    public class StartPagePublishingJobModule : IHttpModule
    {
        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            //Job.Jobs.Instance.AttachJob("PagePublishingJob", new PagePublishingJob(), 60000, null);
        }
    }
}
