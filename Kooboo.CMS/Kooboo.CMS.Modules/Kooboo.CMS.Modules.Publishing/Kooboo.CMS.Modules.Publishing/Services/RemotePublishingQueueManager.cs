using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.Publishing.Cmis;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Services;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class RemotePublishingQueueManager : ManagerBase<RemotePublishingQueue>
    {
        #region .ctor
        ICmisSession _cmisSession;
        TextContentManager _textContentManager;
        PageManager _pageManager;
        IRemotePublishingQueueProvider _remotePublishingQueueProvider;
        IPublishingLogProvider _publishingLogProvider;
        IPageProvider _pageProvider;
        public RemotePublishingQueueManager(IRemotePublishingQueueProvider remotePublishingQueueProvider, ICmisSession cmisSession,
            PageManager pageManager, TextContentManager textContentManager, IPublishingLogProvider publishingLogProvider, IPageProvider pageProvider)
            : base(remotePublishingQueueProvider)
        {
            this._remotePublishingQueueProvider = remotePublishingQueueProvider;
            this._cmisSession = cmisSession;
            this._textContentManager = textContentManager;
            this._pageManager = pageManager;
            this._publishingLogProvider = publishingLogProvider;
            this._pageProvider = pageProvider;
        }
        #endregion

        #region Get
        public virtual RemotePublishingQueue Get(string uuid)
        {
            return new RemotePublishingQueue(uuid).AsActual();
        }
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new RemotePublishingQueue(uuid).AsActual();
                this._remotePublishingQueueProvider.Remove(model);
            }
        }
        #endregion

        #region ProcessQueueItem

        public virtual void ProcessQueueItem(RemotePublishingQueue queueItem, DateTime executeTime)
        {
            Exception exception = null;
            bool hasMoreAction;
            QueueStatus logStatus = QueueStatus.OK;
            var goingActionInfo = queueItem.GoingActionInfo;
            RemoteEndpointSetting remoteEndpoint = null;
            if (goingActionInfo.TimeSpanToProcess > TimeSpan.Zero)
            {
                return;
            }
            try
            {
                queueItem.RetryTimes = queueItem.RetryTimes + 1;
                queueItem.Message = null;
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
                                PublishPage(ref queueItem, goingActionInfo.PublishingAction, out remoteEndpoint);
                                break;
                            case PublishingObject.TextContent:
                                PublishTextContent(ref queueItem, goingActionInfo.PublishingAction, out remoteEndpoint);
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
                if (goingActionInfo.HasNextAction)
                {
                    queueItem.RetryTimes = 0;
                }
            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
                queueItem.Status = QueueStatus.Warning;
                queueItem.Message = e.Message;
                exception = e;
                logStatus = QueueStatus.Warning;

                if (remoteEndpoint != null)
                {
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
                else
                {
                    queueItem.Status = QueueStatus.Susppended;
                }
            }

            if (queueItem.Status == QueueStatus.Processed)
            {
                _remotePublishingQueueProvider.Remove(queueItem);
            }
            else
            {
                _remotePublishingQueueProvider.Update(queueItem, queueItem);
            }
            if (goingActionInfo.PublishingAction != PublishingAction.None)
            {
                AddLog(queueItem, logStatus, goingActionInfo.PublishingAction, exception);
            }
        }

        protected virtual void AddLog(RemotePublishingQueue queueItem, QueueStatus logStatus, PublishingAction action, Exception e = null)
        {
            PublishingLog log = new PublishingLog()
            {
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20),
                QueueType = QueueType.Remote,
                QueueUUID = queueItem.UUID,
                ObjectTitle = queueItem.ObjectTitle,
                SiteName = queueItem.SiteName,
                PublishingObject = queueItem.PublishingObject,
                ObjectUUID = queueItem.ObjectUUID,
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
        protected virtual void PublishPage(ref RemotePublishingQueue queueItem, PublishingAction action, out RemoteEndpointSetting remoteEndpoint)
        {
            remoteEndpoint = new RemoteEndpointSetting() { SiteName = queueItem.SiteName, Name = queueItem.RemoteEndpoint }.AsActual();

            if (remoteEndpoint == null)
            {
                NoSuchEndpoint(ref queueItem, queueItem.RemoteEndpoint);
            }

            var site = new Site(queueItem.SiteName).AsActual();
            if (site != null)
            {
                var page = new Page(site, queueItem.ObjectUUID).AsActual();
                if (page != null)
                {
                    var cmisService = _cmisSession.OpenSession(remoteEndpoint.CmisService, remoteEndpoint.CmisUserName, remoteEndpoint.CmisPassword);
                    switch (action)
                    {
                        case PublishingAction.Publish:
                            if (queueItem.PublishDraft)
                            {
                                page = ((IPageProvider)_pageManager.Provider).GetDraft(page);
                            }
                            page.Published = true;
                            cmisService.AddPage(remoteEndpoint.RemoteRepositoryId, page);
                            queueItem.UtcProcessedPublishTime = DateTime.UtcNow;
                            break;
                        case PublishingAction.Unbpulish:
                            page.Published = false;
                            cmisService.DeletePage(remoteEndpoint.RemoteRepositoryId, page.FullName);
                            queueItem.UtcProcessedUnpublishTime = DateTime.UtcNow;
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
        #endregion

        #region NoSuchObjectMessage
        private static void NoSuchObjectMessage(ref RemotePublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such object:{0}".Localize(), queueItem.ObjectUUID);
        }
        #endregion

        private void NoSuchPublishingMapping(ref RemotePublishingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Warning;
            queueItem.Message = string.Format("No such text folder mapping:{0}".Localize(), queueItem.TextFolderMapping);
        }
        private void NoSuchEndpoint(ref RemotePublishingQueue queueItem, string endpoint)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such endpoint:{0}".Localize(), endpoint);
        }
        private void NoSuchMapping(ref RemotePublishingQueue queueItem, string mapping)
        {
            queueItem.Status = QueueStatus.Processed;
            queueItem.Message = string.Format("No such text content :{0}".Localize(), mapping);
        }

        #region RemotePublish
        protected virtual void PublishTextContent(ref RemotePublishingQueue queueItem, PublishingAction action, out RemoteEndpointSetting remoteEndpoint)
        {

            var mapping = new RemoteTextFolderMapping(queueItem.TextFolderMapping).AsActual();
            if (mapping == null)
            {
                NoSuchPublishingMapping(ref queueItem);
            }
            remoteEndpoint = new RemoteEndpointSetting(mapping.RemoteEndpoint).AsActual();

            if (remoteEndpoint == null)
            {
                NoSuchEndpoint(ref queueItem, mapping.RemoteEndpoint);
            }
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


                        switch (action)
                        {
                            case PublishingAction.Publish:
                                content.Published = true;
                                cmisService.AddTextContent(remoteEndpoint.RemoteRepositoryId, mapping.RemoteFolderId, content);
                                queueItem.UtcProcessedPublishTime = DateTime.UtcNow;
                                break;
                            case PublishingAction.Unbpulish:
                                cmisService.DeleteTextContent(remoteEndpoint.RemoteRepositoryId, mapping.RemoteFolderId, content.UUID);
                                queueItem.UtcProcessedUnpublishTime = DateTime.UtcNow;
                                break;
                            case PublishingAction.None:
                            default:
                                break;
                        }
                        return;
                    }
                }
            }

            NoSuchObjectMessage(ref queueItem);
        }


        #endregion
        #endregion
    }
}
