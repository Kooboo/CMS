#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Job;
using Kooboo.Globalization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Modules.Publishing.Services;

namespace Kooboo.CMS.Modules.Publishing.Jobs
{
    public class ProcessRemotePublishingQueueJob : IJob
    {
        #region .ctor
        IRemotePublishingQueueProvider _remotePublishingQueueProvider;
        RemotePublishingQueueManager _remotePublishingQueueManager;
        public ProcessRemotePublishingQueueJob(IRemotePublishingQueueProvider remotePublishingQueueProvider, RemotePublishingQueueManager remotePublishingQueueManager)
        {
            this._remotePublishingQueueProvider = remotePublishingQueueProvider;
            this._remotePublishingQueueManager = remotePublishingQueueManager;
        }
        #endregion

        #region Execute
        public void Execute(object executionState)
        {
            var utcExecutionTime = DateTime.UtcNow;
            var items = _remotePublishingQueueProvider.GetJobItems(utcExecutionTime, 20);
            foreach (var item in items)
            {
                _remotePublishingQueueManager.ProcessQueueItem(item, utcExecutionTime);
            }
        }
        #endregion

        #region Error
        public void Error(Exception e)
        {
            Kooboo.HealthMonitoring.Log.LogException(e);
        }
        #endregion
    }
}
