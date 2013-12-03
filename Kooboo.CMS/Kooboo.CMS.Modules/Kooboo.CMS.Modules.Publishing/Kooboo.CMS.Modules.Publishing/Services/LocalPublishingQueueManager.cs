using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Globalization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class LocalPublishingQueueManager : ManagerBase<LocalPublishingQueue>
    {
        #region .ctor
        TextContentManager _textContentManager;
        PageManager _pageManager;
        ILocalPublishingQueueProvider _localPublishingQueueProvider;
        IPublishingLogProvider _publishingLogProvider;
        public LocalPublishingQueueManager(ILocalPublishingQueueProvider localPublishingQueueProvider,
            PageManager pageManager, TextContentManager textContentManager, IPublishingLogProvider publishingLogProvider)
            : base(localPublishingQueueProvider)
        {
            this._localPublishingQueueProvider = localPublishingQueueProvider;
            this._textContentManager = textContentManager;
            this._pageManager = pageManager;
            this._publishingLogProvider = publishingLogProvider;
        }
        #endregion

        #region Get
        public virtual LocalPublishingQueue Get(string uuid)
        {
            return new LocalPublishingQueue(uuid).AsActual();
        }
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new LocalPublishingQueue(uuid).AsActual();
                this._localPublishingQueueProvider.Remove(model);
            }
        }
        #endregion

        #region ProcessQueueItem

        public virtual void ProcessQueueItem(LocalPublishingQueue queueItem, DateTime executeTime)
        {
            Exception exception = null;
            QueueStatus logStatus = QueueStatus.OK;
            var goingActionInfo = queueItem.GoingActionInfo;
            if (goingActionInfo.TimeSpanToProcess > TimeSpan.Zero)
            {
                return;
            }
            try
            {
                if (goingActionInfo.TimeSpanToProcess == null && goingActionInfo.HasNextAction == false)
                {
                    queueItem.Status = QueueStatus.Processed;
                }
                else
                {
                    if (goingActionInfo.HasNextAction)
                    {
                        queueItem.Status = QueueStatus.Pending;
                    }
                    else
                    {
                        queueItem.Status = QueueStatus.Processed;
                    }
                    if (goingActionInfo.PublishingAction != PublishingAction.None)
                    {
                        switch (queueItem.PublishingObject)
                        {
                            case PublishingObject.Page:
                                PublishPage(ref queueItem, goingActionInfo.PublishingAction);
                                break;
                            case PublishingObject.TextContent:
                                PublicTextContent(ref queueItem, goingActionInfo.PublishingAction);
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (queueItem.Status == QueueStatus.Warning)
                {
                    logStatus = QueueStatus.Warning;
                }
                else
                {
                    logStatus = QueueStatus.OK;
                }
            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
                queueItem.Status = QueueStatus.Warning;
                queueItem.Message = e.Message;
                exception = e;
            }

            if (queueItem.Status == QueueStatus.Processed)
            {
                _localPublishingQueueProvider.Remove(queueItem);
            }
            else
            {
                _localPublishingQueueProvider.Update(queueItem, queueItem);
            }
            if (goingActionInfo.PublishingAction != PublishingAction.None)
            {
                AddLog(queueItem, logStatus, goingActionInfo.PublishingAction, exception);
            }
        }

        protected virtual void AddLog(LocalPublishingQueue queueItem, QueueStatus logStatus, PublishingAction action, Exception e = null)
        {
            PublishingLog log = new PublishingLog()
            {
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20),
                QueueType = QueueType.Local,
                QueueUUID = queueItem.UUID,
                SiteName = queueItem.SiteName,
                PublishingObject = queueItem.PublishingObject,
                ObjectUUID = queueItem.ObjectUUID,
                ObjectTitle = queueItem.ObjectTitle,
                //PublishingType = queueItem.PublishingType,
                RemoteEndpoint = null,
                TextFolderMapping = null,
                UserId = queueItem.UserId,
                Status = logStatus,
                Vendor = null,
                UtcProcessedTime = DateTime.UtcNow,
                Message = e == null ? queueItem.Message : e.Message,
                StackTrace = e == null ? "" : e.StackTrace,
                PublishingAction = action,
                QueueObject = queueItem
            };

            _publishingLogProvider.Add(log);
        }

        #region PublishPage
        protected virtual void PublishPage(ref LocalPublishingQueue queueItem, PublishingAction action)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                var page = new Page(site, queueItem.ObjectUUID).AsActual();
                if (page != null)
                {
                    if (action == PublishingAction.Publish)
                    {
                        _pageManager.Publish(page, queueItem.PublishDraft, queueItem.UserId);
                        queueItem.UtcProcessedPublishTime = DateTime.UtcNow;
                    }
                    else if (action == PublishingAction.Unbpulish)
                    {
                        _pageManager.Unpublish(page, queueItem.UserId);
                        queueItem.UtcProcessedUnpublishTime = DateTime.UtcNow;
                    }
                }
                else
                {
                    NoSuchObjectMessage(ref queueItem);
                }
            }
        }
        #endregion

        #region NoSuchObjectMessage
        private static void NoSuchObjectMessage(ref LocalPublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such object:{0}".Localize(), queueItem.ObjectUUID);
        }
        #endregion

        #region PublicTextContent
        private void PublicTextContent(ref LocalPublishingQueue queueItem, PublishingAction action)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                var contentIntegrateId = new ContentIntegrateId(queueItem.ObjectUUID);
                var repository = new Repository(contentIntegrateId.Repository).AsActual(); ;
                if (repository != null)
                {
                    var textFolder = new TextFolder(repository, contentIntegrateId.FolderName).AsActual();
                    if (textFolder != null)
                    {
                        var contentUUID = contentIntegrateId.ContentUUID;
                        if (action == PublishingAction.Publish)
                        {
                            _textContentManager.Publish(textFolder, contentUUID, queueItem.UserId);
                            queueItem.UtcProcessedPublishTime = DateTime.UtcNow;
                        }
                        else if (action == PublishingAction.Unbpulish)
                        {
                            _textContentManager.Unpublish(textFolder, contentUUID, queueItem.UserId);
                            queueItem.UtcProcessedUnpublishTime = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        NoSuchObjectMessage(ref queueItem);
                    }
                }
                else
                {
                    NoSuchObjectMessage(ref queueItem);
                }
            }
        }
        #endregion
        #endregion
    }
}
