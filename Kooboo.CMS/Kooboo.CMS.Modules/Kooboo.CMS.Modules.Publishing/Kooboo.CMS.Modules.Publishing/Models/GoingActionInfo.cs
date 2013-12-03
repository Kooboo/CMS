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
    public class GoingActionInfo
    {
        IPublishingQueueItem _queueItem;
        DateTime _processTime;
        public GoingActionInfo(IPublishingQueueItem queueItem, DateTime processTime)
        {
            _queueItem = queueItem;
            _processTime = processTime;
            DeterminePublishingAction(queueItem);
        }
        private void DeterminePublishingAction(IPublishingQueueItem queueItem)
        {
            this.PublishingAction = PublishingAction.None;
            this.HasNextAction = false;

            if (queueItem.UtcTimeToPublish != null && queueItem.UtcTimeToUnpublish != null)
            {
                if (queueItem.UtcTimeToUnpublish.Value > queueItem.UtcTimeToPublish.Value)
                {
                    if (queueItem.UtcProcessedPublishTime == null)
                    {
                        PublishingAction = PublishingAction.Publish;
                        HasNextAction = true;

                    }
                    else if (queueItem.UtcProcessedUnpublishTime == null)
                    {
                        PublishingAction = PublishingAction.Unbpulish;
                        HasNextAction = false;
                    }
                }
                else
                {
                    if (queueItem.UtcProcessedUnpublishTime == null)
                    {
                        PublishingAction = PublishingAction.Unbpulish;
                        HasNextAction = true;
                    }
                    else if (queueItem.UtcProcessedPublishTime == null)
                    {
                        PublishingAction = PublishingAction.Publish;
                        HasNextAction = false;
                    }
                }
            }
            else if (queueItem.UtcTimeToPublish != null)
            {
                PublishingAction = PublishingAction.Publish;
            }
            else if (queueItem.UtcTimeToUnpublish != null)
            {
                PublishingAction = PublishingAction.Unbpulish;
            }
            else
            {
                PublishingAction = PublishingAction.None;
            }

            switch (PublishingAction)
            {
                case PublishingAction.Publish:
                    this.ExpectedProcessTime = this._queueItem.UtcTimeToPublish;
                    break;
                case PublishingAction.Unbpulish:
                    this.ExpectedProcessTime = this._queueItem.UtcTimeToUnpublish;
                    break;
                case PublishingAction.None:
                    break;
                default:
                    break;
            }
        }
        public PublishingAction PublishingAction { get; private set; }
        public DateTime? ExpectedProcessTime { get; private set; }
        public bool HasNextAction { get; private set; }
        public TimeSpan? TimeSpanToProcess
        {
            get
            {
                if (ExpectedProcessTime != null)
                {
                    if (ExpectedProcessTime.Value > _processTime)
                    {
                        return ExpectedProcessTime.Value - _processTime;
                    }
                    else
                    {
                        return TimeSpan.Zero;
                    }

                }
                return null;
            }
        }
    }
}
