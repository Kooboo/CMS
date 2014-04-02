﻿using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Globalization;
using Kooboo.Collections;

using Kooboo.CMS.Modules.CMIS.Services.Implementation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Kooboo.CMS.Modules.CMIS.Services;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    public class IncomingQueueManager : ManagerBase<Kooboo.CMS.Modules.Publishing.Models.IncomingQueue>
    {
        #region .ctor
        IIncomingQueueProvider _incomeQueueProvider;
        PageManager _pageManager;
        TextContentManager _textContentManager;
        ITextContentProvider _textContentProvider;
        IPublishingLogProvider _publishingLogProvider;
        IMediaPathField _mediaPathField;
        MediaContentManager _mediaContentManager;
        MediaFolderManager _mediaFolderManager;
        public IncomingQueueManager(IIncomingQueueProvider incomeQueueProvider, PageManager pageManager, ITextContentProvider textContentProvider,
            TextContentManager textContentManager, IPublishingLogProvider publishingLogProvider, IMediaPathField mediaPathField,
            MediaContentManager mediaContentManager, MediaFolderManager mediaFolderManager)
            : base(incomeQueueProvider)
        {
            this._incomeQueueProvider = incomeQueueProvider;
            this._pageManager = pageManager;
            this._textContentProvider = textContentProvider;
            this._textContentManager = textContentManager;
            this._publishingLogProvider = publishingLogProvider;
            this._mediaPathField = mediaPathField;
            this._mediaContentManager = mediaContentManager;
            this._mediaFolderManager = mediaFolderManager;
        }
        #endregion

        #region Get
        public virtual IncomingQueue Get(string uuid)
        {
            return new IncomingQueue(uuid).AsActual();
        }
        #endregion

        #region Delete
        public virtual void Delete(string[] uuids)
        {
            foreach (string uuid in uuids)
            {
                var model = new IncomingQueue(uuid).AsActual();
                this._incomeQueueProvider.Remove(model);
            }
        }
        #endregion

        #region ProcessQueueItem
        public virtual void ProcessQueueItem(IncomingQueue queueItem)
        {
            Exception exception = null;
            QueueStatus logStatus = QueueStatus.OK;
            try
            {
                queueItem.UtcProcessedTime = DateTime.UtcNow;

                switch (queueItem.PublishingObject)
                {
                    case PublishingObject.Page:
                        PublishPage(ref queueItem);
                        break;
                    case PublishingObject.TextContent:
                        PublishTextContent(ref queueItem);
                        break;
                    default:
                        break;
                }
                queueItem.Status = QueueStatus.Processed;

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
                logStatus = QueueStatus.Warning;
                exception = e;
                queueItem.Message = e.Message;
            }

            if (queueItem.Status == QueueStatus.Processed)
            {
                _incomeQueueProvider.Remove(queueItem);
            }
            else
            {
                _incomeQueueProvider.Update(queueItem, queueItem);
            }

            AddLog(queueItem, logStatus, exception);
        }

        protected virtual void AddLog(IncomingQueue queueItem, QueueStatus logStatus, Exception e = null)
        {
            PublishingLog log = new PublishingLog()
            {
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(20),
                QueueType = QueueType.Incoming,
                QueueUUID = queueItem.UUID,
                ObjectTitle = queueItem.ObjectTitle,
                SiteName = queueItem.SiteName,
                PublishingObject = queueItem.PublishingObject,
                ObjectUUID = queueItem.ObjectUUID,
                //PublishingType = PublishingType.Remote,
                RemoteEndpoint = null,
                TextFolderMapping = null,
                UserId = queueItem.Vendor,
                Status = logStatus,
                Vendor = queueItem.Vendor,
                UtcProcessedTime = queueItem.UtcProcessedTime,
                Message = e == null ? queueItem.Message : e.Message,
                StackTrace = e == null ? "" : e.StackTrace,
                PublishingAction = queueItem.Action,
                QueueObject = queueItem
            };

            _publishingLogProvider.Add(log);
        }

        protected virtual void PublishPage(ref IncomingQueue queueItem)
        {
            var site = new Site(queueItem.SiteName).AsActual();
            Page page;
            if (site != null)
            {
                switch (queueItem.Action)
                {
                    case PublishingAction.Publish:
                        var pageProperties = (Page)queueItem.Object;

                        page = new Page(site, queueItem.ObjectUUID);

                        MapProperties(pageProperties, page);

                        if (page.Exists())
                        {
                            _pageManager.Remove(site, page);
                        }
                        _pageManager.Add(site, page);
                        break;
                    case PublishingAction.Unbpulish:
                        var pageId = queueItem.ObjectUUID;
                        page = new Page(site, pageId);
                        _pageManager.Remove(site, page);
                        break;
                    case PublishingAction.None:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                NoSuchSiteMessage(ref queueItem);
            }

        }

        private static void MapProperties(Page properties, Page page)
        {
            page.IsDefault = properties.IsDefault;
            page.EnableTheming = properties.EnableTheming;
            page.EnableScript = properties.EnableScript;
            page.HtmlMeta = properties.HtmlMeta;
            page.Route = properties.Route;
            page.Navigation = properties.Navigation;
            page.Permission = properties.Permission;
            page.Layout = properties.Layout;
            page.PagePositions = properties.PagePositions;
            page.DataRules = properties.DataRules;
            page.Plugins = properties.Plugins;
            page.PageType = properties.PageType;
            page.OutputCache = properties.OutputCache;
            page.CustomFields = properties.CustomFields;
            page.Published = properties.Published;
            page.UserName = properties.UserName;
            page.ContentTitle = properties.ContentTitle;
            page.Searchable = properties.Searchable;
            page.RequireHttps = properties.RequireHttps;
            page.CacheToDisk = properties.CacheToDisk;
        }
        #region NoSuchObjectMessage
        private void NoSuchSiteMessage(ref IncomingQueue queueItem)
        {
            queueItem.Status = QueueStatus.Warning;
            queueItem.Message = string.Format("No such site:{0}".Localize(), queueItem.ObjectUUID);
        }
        #endregion
        protected virtual void PublishTextContent(ref IncomingQueue queueItem)
        {
            var site = new Site(queueItem.SiteName).AsActual();

            if (site != null)
            {
                var repository = site.GetRepository();
                if (repository != null)
                {
                    switch (queueItem.Action)
                    {
                        case PublishingAction.Publish:
                            AddOrUpdateContent(repository, queueItem);
                            return;
                        case PublishingAction.Unbpulish:
                            var integrateId = new ContentIntegrateId(queueItem.ObjectUUID);
                            var textFolder = new TextFolder(repository, integrateId.FolderName).AsActual();
                            if (textFolder != null)
                            {
                                _textContentManager.Delete(repository, textFolder, integrateId.ContentUUID);
                                return;
                            }
                            break;
                        case PublishingAction.None:
                        default:
                            return;
                    }
                }
            }
            NoSuchSiteMessage(ref queueItem);
        }

        private Regex _repositoryNameRegex = new Regex(@"(.*/Cms_Data/Contents/)(\w+)(/Media/(?<mediaFolder>.*?)/([^/./\\:*?\""<>|]+)\.(\w{2,4}))");
        private void AddOrUpdateContent(Repository repository, IncomingQueue queueItem)
        {
            var textContent = (Dictionary<string, object>)queueItem.Object;
            var values = textContent.ToNameValueCollection();
            var files = values.GetFilesFromValues();
            var medias = values.GetMediaFromValues();
            var newVirtualPathDict = new Dictionary<string, string>();
            for (int i = 0, len = medias.Count; i < len; i++)
            {
                var virtualPath = this._repositoryNameRegex.Replace(medias[i].FileName, string.Format("$1{0}$3", repository.Name));
                var folder = this._repositoryNameRegex.Match(virtualPath).Groups["mediaFolder"].Value;

                if (!string.IsNullOrWhiteSpace(virtualPath))
                {
                    newVirtualPathDict.Add(medias[i].FileName, virtualPath);

                    var physicalPath = Kooboo.Web.Url.UrlUtility.MapPath(virtualPath);

                    var mediaFolder = new MediaFolder(repository, folder);
                    if (mediaFolder.AsActual() == null)
                    {
                        this._mediaFolderManager.Add(repository, mediaFolder);
                    }

                    var folderPath = Path.GetDirectoryName(physicalPath);

                    var fileName = Path.GetFileName(physicalPath);

                    this._mediaContentManager.Add(repository, mediaFolder,
                        fileName, medias[i].InputStream, false);
                }
            }

            foreach (var prop in values.AllKeys)
            {
                if (!prop.StartsWith("__"))
                {
                    if (values[prop] != null && values[prop] is string && this._mediaPathField.IsMediaPathField(values[prop].ToString()))
                    {
                        var fieldValue = values[prop].ToString();
                        foreach (var key in newVirtualPathDict.Keys)
                        {
                            fieldValue = fieldValue.Replace(key, newVirtualPathDict[key]);
                        }
                        values[prop] = fieldValue;
                    }
                }
            }

            var categories = values.GetCategories().Select(it => new TextContent(repository.Name, "", it.CategoryFolder) { UUID = it.CategoryUUID }).ToArray();

            var textFolder = new TextFolder(repository, values["FolderName"]);
            _textContentManager.Delete(repository, textFolder, values["UUID"]);
            _textContentManager.Add(textFolder.Repository, textFolder, values, files, categories, values["UserId"]);


        }
        #endregion
    }
}
