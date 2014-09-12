#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Module.Publishing.EmailLog
{
    public class PublishingLogProvider : IPublishingLogProvider
    {
        IPublishingLogProvider _publishingLogProvider;
        public PublishingLogProvider(IPublishingLogProvider publishingLogProvider)
        {
            _publishingLogProvider = publishingLogProvider;
        }

        public void Add(Modules.Publishing.Models.PublishingLog item)
        {
            _publishingLogProvider.Add(item);
            try
            {
                SendLog(item);
            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
            }
        }


        private void SendLog(PublishingLog item)
        {
            if (item.Site != null)
            {
                var site = item.Site.AsActual();
                //have set smtp
                if (site != null && site.Smtp != null && !string.IsNullOrEmpty(site.Smtp.Host) && !string.IsNullOrEmpty(site.Smtp.From)
                    && site.Smtp.To != null && site.Smtp.To.Length > 0)
                {
                    var logMessage = @"
Log type    : {0},
Action      : {1},
Item type   : {2},
Title       : {3},
Object UUID : {4}
Remote sites: {5},
Folder mapping:{6},
User        : {7},
Status      : {8}
Processed time:{9},
Message     : {10},
Stack trace : {11}
";
                    logMessage = string.Format(logMessage, item.QueueType,
                        item.PublishingAction,
                        item.PublishingObject,
                        item.ObjectTitle,
                        item.ObjectUUID,
                        string.IsNullOrEmpty(item.RemoteEndpoint) ? "-" : item.RemoteEndpoint,
                        string.IsNullOrEmpty(item.TextFolderMapping) ? "-" : item.TextFolderMapping,
                        string.IsNullOrEmpty(item.UserId) ? "-" : item.UserId,
                        item.Status,
                        item.UtcProcessedTime.Value.ToLocalTime(),
                        string.IsNullOrEmpty(item.Message) ? "-" : item.Message,
                        string.IsNullOrEmpty(item.StackTrace) ? "-" : item.StackTrace);


                    site.SendMailToSiteManager(site.Smtp.From, "Publishing log", logMessage, false);

                }
            }
        }

        public IEnumerable<Modules.Publishing.Models.PublishingLog> All()
        {
            return _publishingLogProvider.All();
        }

        public Modules.Publishing.Models.PublishingLog Get(Modules.Publishing.Models.PublishingLog dummy)
        {
            return _publishingLogProvider.Get(dummy);
        }

        public void Remove(Modules.Publishing.Models.PublishingLog item)
        {
            _publishingLogProvider.Remove(item);
        }

        public void Update(Modules.Publishing.Models.PublishingLog @new, Modules.Publishing.Models.PublishingLog old)
        {
            _publishingLogProvider.Update(@new, old);
        }


        public IEnumerable<PublishingLog> All(Site site)
        {
            return _publishingLogProvider.All(site);
        }
    }
}
