using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Services;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class PublishingManager
    {
        #region .ctor
        private readonly IPageProvider _provider;
        private readonly TextContentManager _textContentManager;
        private readonly LocalPublishingQueueManager _localPublishingQueueManager;
        private readonly RemotePublishingQueueManager _remotePublishingQueueManager;
        private readonly PageManager _pageManager;
        private readonly IPublishingLogProvider _publishingLogProvider;
        public PublishingManager(IPageProvider provider, TextContentManager textContentManager,
            LocalPublishingQueueManager localPublishingQueueManager, RemotePublishingQueueManager remotePublishingQueueManager, PageManager pageManager, IPublishingLogProvider publishingLogProvider)
        {
            this._provider = provider;
            this._textContentManager = textContentManager;
            this._localPublishingQueueManager = localPublishingQueueManager;
            this._remotePublishingQueueManager = remotePublishingQueueManager;
            this._pageManager = pageManager;
            this._publishingLogProvider = publishingLogProvider;
        }
        #endregion

        #region Publish page
        public virtual bool HasDraft(string pageName)
        {
            var page = new Page(Site.Current, pageName);
            return HasDraft(page);
        }
        public virtual bool HasDraft(Page page)
        {
            var draft = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.Provider.GetDraft(page);
            return draft != null;
        }
        public virtual void PublishPage(Site site, string[] docs, bool localPublish, bool remotePublish, bool publishDraft, string[] remoteEndpoints, string userName)
        {
            PublishPage(site, docs, localPublish, remotePublish, false, publishDraft, remoteEndpoints, null, null, userName);
        }
        public virtual void PublishPage(Site site, string[] docs, bool localPublish, bool remotePublish, bool publishSchedule, bool publishDraft, string[] remoteEndpoints, DateTime? publishDate, DateTime? unpublishDate, string userName)
        {
            foreach (string uuid in docs)
            {
                if (localPublish)
                {
                    if (publishSchedule)
                    {
                        var queue = new LocalPublishingQueue(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                        {
                            ObjectUUID = uuid,
                            ObjectTitle = uuid,
                            UserId = userName,
                            UtcCreationDate = DateTime.UtcNow,
                            PublishingObject = PublishingObject.Page,
                            PublishDraft = publishDraft,
                            Status = QueueStatus.Pending
                        };
                        if (publishDate.HasValue)
                        {
                            queue.UtcTimeToPublish = publishDate.Value.ToUniversalTime();
                        }
                        if (unpublishDate.HasValue)
                        {
                            queue.UtcTimeToUnpublish = unpublishDate.Value.ToUniversalTime();
                        }
                        this._localPublishingQueueManager.Add(queue);
                    }
                    else
                    {
                        var page = new Page(Site.Current, uuid);
                        page = page.AsActual();
                        _pageManager.Publish(page, publishDraft, userName);
                        _publishingLogProvider.Add(new PublishingLog(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(12))
                        {
                            QueueType = QueueType.Local,
                            PublishingObject = PublishingObject.Page,
                            ObjectUUID = uuid,
                            ObjectTitle = uuid,
                            PublishingAction = PublishingAction.Publish,
                            UserId = userName,
                            PublishDraft = publishDraft,
                            Status = QueueStatus.OK,
                            UtcProcessedTime = DateTime.UtcNow
                        });

                    }
                }
                if (remotePublish)
                {
                    foreach (var endpoint in remoteEndpoints)
                    {
                        var queue = new RemotePublishingQueue(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                       {
                           ObjectUUID = uuid,
                           ObjectTitle = uuid,
                           PublishingObject = PublishingObject.Page,
                           UserId = userName,
                           RemoteEndpoint = endpoint,
                           UtcCreationDate = DateTime.UtcNow,
                           PublishDraft = publishDraft,
                           Status = QueueStatus.Pending
                       };
                        if (publishSchedule)
                        {
                            if (publishDate.HasValue)
                            {
                                queue.UtcTimeToPublish = publishDate.Value.ToUniversalTime();
                            }
                            if (unpublishDate.HasValue)
                            {
                                queue.UtcTimeToUnpublish = unpublishDate.Value.ToUniversalTime();
                            }
                        }
                        else
                        {
                            queue.UtcTimeToPublish = DateTime.UtcNow;
                        }
                        this._remotePublishingQueueManager.Add(queue);
                    }
                }
            }
        }
        #endregion

        #region Publish text content
        public virtual void PublishTextContent(Site site, TextFolder textFolder, string[] docs, bool localPublish, bool remotePublish, bool publishSchedule, string[] publishingMappings, DateTime? publishDate, DateTime? unpublishDate, string userName)
        {
            foreach (string uuid in docs)
            {
                var content = textFolder.CreateQuery().WhereEquals("UUID", uuid).FirstOrDefault();
                if (content != null)
                {
                    if (localPublish)
                    {
                        if (publishSchedule)
                        {
                            var queue = new LocalPublishingQueue(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                            {
                                ObjectUUID = content.IntegrateId,
                                ObjectTitle = content.GetSummary(),
                                UserId = userName,
                                UtcCreationDate = DateTime.UtcNow,
                                PublishingObject = PublishingObject.TextContent,
                                Status = QueueStatus.Pending
                            };
                            if (publishDate.HasValue)
                            {
                                queue.UtcTimeToPublish = publishDate.Value.ToUniversalTime();
                            }
                            if (unpublishDate.HasValue)
                            {
                                queue.UtcTimeToUnpublish = unpublishDate.Value.ToUniversalTime();
                            }
                            this._localPublishingQueueManager.Add(queue);
                        }
                        else
                        {
                            this._textContentManager.Publish(textFolder, uuid, userName);
                            _publishingLogProvider.Add(new PublishingLog(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20))
                            {
                                QueueType = QueueType.Local,
                                PublishingObject = PublishingObject.TextContent,
                                ObjectUUID = uuid,
                                ObjectTitle = uuid,
                                PublishingAction = PublishingAction.Publish,
                                UserId = userName,
                                Status = QueueStatus.OK,
                                UtcProcessedTime = DateTime.UtcNow
                            });

                        }
                    }
                    if (remotePublish)
                    {

                        foreach (string mapping in publishingMappings)
                        {
                            var queue = new RemotePublishingQueue(site, Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10))
                            {
                                ObjectUUID = content.IntegrateId,
                                ObjectTitle = content.GetSummary(),
                                PublishingObject = PublishingObject.TextContent,
                                UserId = userName,
                                TextFolderMapping = mapping,
                                UtcCreationDate = DateTime.UtcNow,
                                Status = QueueStatus.Pending
                            };
                            if (publishSchedule)
                            {
                                if (publishDate.HasValue)
                                {
                                    queue.UtcTimeToPublish = publishDate.Value.ToUniversalTime();
                                }
                                if (unpublishDate.HasValue)
                                {
                                    queue.UtcTimeToUnpublish = unpublishDate.Value.ToUniversalTime();
                                }
                            }
                            else
                            {
                                queue.UtcTimeToPublish = DateTime.UtcNow;
                            }
                            this._remotePublishingQueueManager.Add(queue);
                        }
                    }

                }
            }
        }
        #endregion
    }
}
