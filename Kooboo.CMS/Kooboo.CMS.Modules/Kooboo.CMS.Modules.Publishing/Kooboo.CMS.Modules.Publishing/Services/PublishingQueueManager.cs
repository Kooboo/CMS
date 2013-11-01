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
    public class PublishingQueueManager : ManagerBase<PublishingQueue>
    {
        #region .ctor
        TextContentManager _textContentManager;
        PageManager _pageManager;
        IOutgoingQueueProvider _outgoingQueueProvider;
        IPublishingQueueProvider _publishingQueueProvider;
        IPublishingLogProvider _publishingLogProvider;
        public PublishingQueueManager(IPublishingQueueProvider publishingQueueProvider, IOutgoingQueueProvider outgoingQueueProvider,
            PageManager pageManager, TextContentManager textContentManager, IPublishingLogProvider publishingLogProvider)
            : base(publishingQueueProvider)
        {
            this._publishingQueueProvider = publishingQueueProvider;
            this._textContentManager = textContentManager;
            this._pageManager = pageManager;
            this._outgoingQueueProvider = outgoingQueueProvider;
            this._publishingLogProvider = publishingLogProvider;
        }
        #endregion

        #region Get
        public virtual PublishingQueue Get(string uuid)
        {
            return new PublishingQueue(uuid).AsActual();
        }
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new PublishingQueue(uuid).AsActual();
                this._publishingQueueProvider.Remove(model);
            }
        }
        #endregion

        #region ProcessQueueItem

        public virtual void ProcessQueueItem(PublishingQueue queueItem, DateTime executeTime)
        {
            Exception exception = null;
            bool hasMoreAction;
            PublishingAction action = GetPublishingAction(queueItem, out hasMoreAction);
            if (action == PublishingAction.None && hasMoreAction == true)
            {
                return;
            }

            try
            {
                if (action == PublishingAction.None && hasMoreAction == false)
                {
                    queueItem.Status = QueueStatus.Processed;
                }
                else
                {
                    if (hasMoreAction)
                    {
                        queueItem.Status = QueueStatus.Pending;
                    }
                    else
                    {
                        queueItem.Status = QueueStatus.Processed;
                    }
                    if (action != PublishingAction.None)
                    {
                        switch (queueItem.PublishingType)
                        {
                            case PublishingType.Local:
                                LocalPublish(ref queueItem, action);
                                break;
                            case PublishingType.Remote:
                                RemotePublish(ref queueItem, action);
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
                queueItem.Status = QueueStatus.Warning;
                queueItem.Message = e.Message;
                queueItem.UtcProcessedTime = DateTime.UtcNow;
                exception = e;
            }

            if (queueItem.Status == QueueStatus.Processed)
            {
                _publishingQueueProvider.Remove(queueItem);
            }
            else
            {
                _publishingQueueProvider.Update(queueItem, queueItem);
            }


            AddLog(queueItem, action, exception);
        }

        protected virtual void AddLog(PublishingQueue queueItem, PublishingAction action, Exception e = null)
        {
            PublishingLog log = new PublishingLog()
            {
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20),
                QueueType = QueueType.Publishing,
                QueueUUID = queueItem.UUID,
                SiteName = queueItem.SiteName,
                PublishingObject = queueItem.PublishingObject,
                ObjectUUID = queueItem.ObjectUUID,
                //PublishingType = queueItem.PublishingType,
                RemoteEndpoint = null,
                TextFolderMapping = null,
                UserId = queueItem.UserId,
                Status = queueItem.Status,
                Vendor = null,
                UtcProcessedTime = DateTime.UtcNow,
                Message = e == null ? queueItem.Message : e.Message,
                StackTrace = e == null ? "" : e.StackTrace,
                PublishingAction = action,
                QueueObject = queueItem
            };

            _publishingLogProvider.Add(log);
        }
        #region GetPublishingAction
        private PublishingAction GetPublishingAction(PublishingQueue queueItem, out bool hasMoreAction)
        {
            PublishingAction action = PublishingAction.None;
            hasMoreAction = false;
            if (queueItem.UtcTimeToPublish == null && queueItem.UtcTimeToUnpublish == null)
            {
                action = PublishingAction.None;
            }
            else
            {
                if (queueItem.UtcProcessedTime == null)
                {
                    if (queueItem.UtcTimeToUnpublish == null)
                    {
                        action = PublishingAction.Publish;
                    }
                    else if (queueItem.UtcTimeToPublish == null)
                    {
                        action = PublishingAction.Unbpulish;
                    }
                    else
                    {
                        if (queueItem.UtcTimeToPublish.Value > queueItem.UtcTimeToUnpublish.Value)
                        {
                            action = PublishingAction.Unbpulish;
                        }
                        else
                        {
                            action = PublishingAction.Publish;
                        }
                        hasMoreAction = true;
                    }
                }
                else if (queueItem.UtcTimeToPublish != null && queueItem.UtcTimeToUnpublish != null)
                {
                    if (queueItem.UtcTimeToPublish.Value > queueItem.UtcTimeToUnpublish.Value)
                    {
                        if (queueItem.UtcTimeToPublish.Value < DateTime.UtcNow)
                        {
                            action = PublishingAction.Publish;
                        }
                        else
                        {
                            action = PublishingAction.None;
                            hasMoreAction = true;
                        }
                    }
                    else
                    {
                        if (queueItem.UtcTimeToUnpublish.Value < DateTime.UtcNow)
                        {
                            action = PublishingAction.Unbpulish;
                        }
                        else
                        {
                            action = PublishingAction.None;
                            hasMoreAction = true;
                        }
                    }
                }
                else
                {
                    action = PublishingAction.None;
                }
            }

            return action;
        }
        #endregion

        #region LocalPublish
        protected virtual void LocalPublish(ref PublishingQueue queueItem, PublishingAction action)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                switch (queueItem.PublishingObject)
                {
                    case PublishingObject.Page:
                        var page = new Page(site, queueItem.ObjectUUID).AsActual();
                        if (page != null)
                        {
                            if (action == PublishingAction.Publish)
                            {
                                _pageManager.Publish(page, queueItem.PublishDraft, queueItem.UserId);
                            }
                            else if (action == PublishingAction.Unbpulish)
                            {
                                _pageManager.Unpublish(page, queueItem.UserId);
                            }
                        }
                        else
                        {
                            NoSuchObjectMessage(ref queueItem);
                        }
                        break;
                    case PublishingObject.TextContent:
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
                                }
                                else if (action == PublishingAction.Unbpulish)
                                {
                                    _textContentManager.Unpublish(textFolder, contentUUID, queueItem.UserId);
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
                        break;
                    default:
                        NoSuchPublishingObject(ref queueItem);
                        break;
                }
            }

            queueItem.UtcProcessedTime = DateTime.UtcNow;

        }
        #endregion

        #region NoSuchObjectMessage
        private static void NoSuchObjectMessage(ref PublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such object:{0}".Localize(), queueItem.ObjectUUID);
        }
        #endregion

        #region NoAnyEndPoint
        private static void NoAnyEndPoint(ref PublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = "No any remote endpoints".Localize();
        }
        #endregion

        #region RemotePublish
        private void RemotePublish(ref PublishingQueue queueItem, PublishingAction action)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                switch (queueItem.PublishingObject)
                {
                    case PublishingObject.Page:
                        if (queueItem.RemoteEndpoints != null)
                        {
                            var page = new Page(site, queueItem.ObjectUUID).AsActual();
                            if (page != null)
                            {
                                foreach (var remote in queueItem.RemoteEndpoints)
                                {
                                    var outgoingQueue = new OutgoingQueue();
                                    outgoingQueue.SiteName = queueItem.SiteName;
                                    outgoingQueue.PublishingObject = queueItem.PublishingObject;
                                    outgoingQueue.ObjectUUID = queueItem.ObjectUUID;
                                    outgoingQueue.RemoteEndpoint = remote;
                                    outgoingQueue.UtcCreationDate = DateTime.UtcNow;
                                    outgoingQueue.Status = QueueStatus.Pending;
                                    outgoingQueue.RetryTimes = 0;
                                    outgoingQueue.Action = action;
                                    outgoingQueue.PublishDraft = queueItem.PublishDraft;
                                    _outgoingQueueProvider.Add(outgoingQueue);
                                }
                            }
                            else
                            {
                                NoSuchObjectMessage(ref queueItem);
                            }
                        }
                        NoAnyEndPoint(ref queueItem);
                        break;
                    case PublishingObject.TextContent:
                        if (queueItem.PublishingMappings != null)
                        {
                            var contentIntegrateId = new ContentIntegrateId(queueItem.ObjectUUID);
                            var repository = new Repository(contentIntegrateId.Repository).AsActual(); ;
                            if (repository != null)
                            {
                                var textFolder = new TextFolder(repository, contentIntegrateId.FolderName).AsActual();
                                if (textFolder != null)
                                {
                                    var contentUUID = contentIntegrateId.ContentUUID;

                                    foreach (var mappingName in queueItem.PublishingMappings)
                                    {
                                        var mapping = new RemoteTextFolderMapping();//todo:AsActual
                                        if (mapping != null)
                                        {
                                            var outgoingQueue = new OutgoingQueue();
                                            outgoingQueue.SiteName = queueItem.SiteName;
                                            outgoingQueue.PublishingObject = queueItem.PublishingObject;
                                            outgoingQueue.ObjectUUID = queueItem.ObjectUUID;
                                            outgoingQueue.RemoteEndpoint = mapping.RemoteEndpoint;
                                            outgoingQueue.RemoteFolderId = mapping.RemoteFolderId;
                                            outgoingQueue.UtcCreationDate = DateTime.UtcNow;
                                            outgoingQueue.Status = QueueStatus.Pending;
                                            outgoingQueue.RetryTimes = 0;
                                            outgoingQueue.Action = action;
                                            outgoingQueue.PublishDraft = queueItem.PublishDraft;
                                            _outgoingQueueProvider.Add(outgoingQueue);
                                        }
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
                        NoAnyEndPoint(ref queueItem);
                        break;
                    default:
                        NoSuchPublishingObject(ref queueItem);
                        break;
                }
            }
        }

        private static void NoSuchPublishingObject(ref PublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Warning;
            queueItem.Message = string.Format("No such publishing object:{0}".Localize(), queueItem.PublishingObject);
        }
        #endregion
        #endregion
    }
}
