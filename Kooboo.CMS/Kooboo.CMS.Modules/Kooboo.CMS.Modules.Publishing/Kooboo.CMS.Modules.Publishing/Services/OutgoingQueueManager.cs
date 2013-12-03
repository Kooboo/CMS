#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.Publishing.Cmis;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class OutgoingQueueManager : ManagerBase<OutgoingQueue>
    {
        #region .ctor
        ICmisSession _cmisSession;
        IOutgoingQueueProvider _outgoingQueueProvider;
        IPublishingLogProvider _publishingLogProvider;
        PageManager _pageManager;
        TextContentManager _textContentManager;
        ITextContentProvider _textContentProvider;
        public OutgoingQueueManager(IOutgoingQueueProvider outgoingQueueProvider, ICmisSession cmisSession, IPublishingLogProvider publishingLogProvider,
            PageManager pageManager, TextContentManager textContentManager)
            : base(outgoingQueueProvider)
        {
            this._outgoingQueueProvider = outgoingQueueProvider;
            this._cmisSession = cmisSession;
            this._publishingLogProvider = publishingLogProvider;
            this._pageManager = pageManager;
            this._textContentManager = textContentManager;
        }
        #endregion

        #region Get
        public virtual OutgoingQueue Get(string uuid)
        {
            return new OutgoingQueue(uuid).AsActual();
        }
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new OutgoingQueue(uuid).AsActual();
                this._outgoingQueueProvider.Remove(model);
            }
        }
        #endregion

        #region ProcessQueueItem
        private void AddLog(OutgoingQueue queueItem, Exception e)
        {
            PublishingLog log = new PublishingLog()
            {
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20),
                QueueType = QueueType.Outgoing,
                QueueUUID = queueItem.UUID,
                SiteName = queueItem.SiteName,
                PublishingObject = queueItem.PublishingObject,
                ObjectUUID = queueItem.ObjectUUID,
                //PublishingType = PublishingType.Remote,
                RemoteEndpoint = null,
                TextFolderMapping = null,
                UserId = null,
                Status = queueItem.Status,
                Vendor = null,
                UtcProcessedTime = queueItem.UtcLastExecutedTime,
                Message = e == null ? queueItem.Message : e.Message,
                StackTrace = e == null ? "" : e.StackTrace,
                PublishingAction = queueItem.Action,
                QueueObject = queueItem
            };

            _publishingLogProvider.Add(log);
        }
        public virtual void ProcessQueueItem(OutgoingQueue queueItem)
        {
            Exception exception = null;
            queueItem.RetryTimes = queueItem.RetryTimes + 1;
            queueItem.UtcLastExecutedTime = DateTime.UtcNow;

            var remoteEndpoint = new RemoteEndpointSetting() { SiteName = queueItem.SiteName, Name = queueItem.RemoteEndpoint }.AsActual();
            if (remoteEndpoint != null)
            {
                try
                {
                    switch (queueItem.PublishingObject)
                    {
                        case PublishingObject.Page:
                            PublishPage(ref queueItem, remoteEndpoint);
                            break;
                        case PublishingObject.TextContent:
                            PublishTextContent(ref queueItem, remoteEndpoint);
                            break;
                        default:
                            break;
                    }
                    queueItem.Status = QueueStatus.Processed;
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                    exception = e;

                    var maxRetryTimes = remoteEndpoint.MaxRetryTimes > 0 ? remoteEndpoint.MaxRetryTimes : 5;
                    if (queueItem.RetryTimes < maxRetryTimes)
                    {
                        queueItem.Status = QueueStatus.Pending;
                    }
                    else
                    {
                        queueItem.Status = QueueStatus.Susppended;
                    }
                }
            }
            else
            {
                NoAnyEndPoint(ref queueItem, queueItem.RemoteEndpoint);
            }
            if (queueItem.Status == QueueStatus.Processed)
            {
                _outgoingQueueProvider.Remove(queueItem);
            }
            else
            {
                _outgoingQueueProvider.Update(queueItem, queueItem);
            }


            AddLog(queueItem, exception);
        }
        protected virtual void PublishPage(ref OutgoingQueue queueItem, RemoteEndpointSetting remoteEndpoint)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                var page = new Page(site, queueItem.ObjectUUID).AsActual();
                if (page != null)
                {
                    var cmisService = _cmisSession.OpenSession(remoteEndpoint.CmisService, remoteEndpoint.CmisUserName, remoteEndpoint.CmisPassword);
                    switch (queueItem.Action)
                    {
                        case PublishingAction.Publish:
                            if (queueItem.PublishDraft)
                            {
                                page = ((IPageProvider)_pageManager.Provider).GetDraft(page);
                            }
                            page.Published = true;
                            cmisService.AddPage(remoteEndpoint.RemoteRepositoryId, page);
                            queueItem.Status = QueueStatus.Processed;
                            break;
                        case PublishingAction.Unbpulish:
                            page.Published = false;
                            cmisService.DeletePage(remoteEndpoint.RemoteRepositoryId, page.FullName);
                            queueItem.Status = QueueStatus.Processed;
                            break;
                        case PublishingAction.None:
                        default:
                            queueItem.Status = QueueStatus.Processed;
                            break;
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
        protected virtual void PublishTextContent(ref OutgoingQueue queueItem, RemoteEndpointSetting remoteEndpoint)
        {
            var contentIntegrateId = new ContentIntegrateId(queueItem.ObjectUUID);
            var repository = new Repository(contentIntegrateId.Repository).AsActual();
            if (repository != null)
            {
                var textFolder = new TextFolder(repository, contentIntegrateId.FolderName).AsActual();
                if (textFolder != null)
                {
                    var contentUUID = contentIntegrateId.ContentUUID;

                    var content = textFolder.CreateQuery().WhereEquals("UUID", contentUUID).FirstOrDefault();
                    if (content != null)
                    {
                        var cmisService = _cmisSession.OpenSession(remoteEndpoint.CmisService, remoteEndpoint.CmisUserName, remoteEndpoint.CmisPassword);

                        switch (queueItem.Action)
                        {
                            case PublishingAction.Publish:
                                content.Published = true;
                                var categories = _textContentProvider.QueryCategories(content);
                                cmisService.AddTextContent(remoteEndpoint.RemoteRepositoryId, queueItem.RemoteFolderId, content, categories);
                                break;
                            case PublishingAction.Unbpulish:
                                cmisService.DeleteTextContent(remoteEndpoint.RemoteRepositoryId, queueItem.RemoteFolderId, content.UUID);
                                break;
                            case PublishingAction.None:
                            default:
                                queueItem.Status = QueueStatus.Processed;
                                break;
                        }
                        return;
                    }
                }
            }
            NoSuchObjectMessage(ref queueItem);
        }

        #region NoSuchObjectMessage
        private void NoSuchObjectMessage(ref OutgoingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Warning;
            queueItem.Message = string.Format("No such object:{0}".Localize(), queueItem.ObjectUUID);
        }
        #endregion

        #region NoAnyEndPoint
        private void NoAnyEndPoint(ref OutgoingQueue queueItem, string endpoint)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such endpoint:{0}".Localize(), endpoint);
        }
        #endregion
        #endregion
    }
}
