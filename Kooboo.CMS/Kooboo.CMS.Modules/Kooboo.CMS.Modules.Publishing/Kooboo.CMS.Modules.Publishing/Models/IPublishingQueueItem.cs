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

namespace Kooboo.CMS.Modules.Publishing.Models
{
    public interface IPublishingQueueItem
    {

        string UUID { get; set; }

        string SiteName { get; set; }

        PublishingObject PublishingObject { get; set; }

        string ObjectUUID { get; set; }

        string ObjectTitle { get; set; }

        DateTime? UtcTimeToPublish { get; set; }

        DateTime? UtcTimeToUnpublish { get; set; }

        DateTime UtcCreationDate { get; set; }

        string UserId { get; set; }

        bool PublishDraft { get; set; }

        QueueStatus Status { get; set; }

        string Message { get; set; }

        DateTime? UtcProcessedPublishTime { get; set; }

        DateTime? UtcProcessedUnpublishTime { get; set; }

        GoingActionInfo GoingActionInfo { get; }
    }
}
