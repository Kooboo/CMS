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

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Persistence;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Modules.Publishing.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Modules.CMIS.Services.Implementation.IIncomeDataManager))]
    public class CmisIncomeDataManager : Kooboo.CMS.Modules.CMIS.Services.Implementation.IIncomeDataManager
    {
        #region .ctor
        IIncomingQueueProvider _incomeQueueProvider;
        ITextContentBinder _textContentBinder;
        public CmisIncomeDataManager(IIncomingQueueProvider incomeQueueProvider, ITextContentBinder binder)
        {
            this._incomeQueueProvider = incomeQueueProvider;
            this._textContentBinder = binder;
        }
        #endregion

        #region TextContent
        public string AddTextContent(Site site, TextFolder textFolder, System.Collections.Specialized.NameValueCollection values, [System.Runtime.InteropServices.OptionalAttribute][System.Runtime.InteropServices.DefaultParameterValueAttribute("")]string userid, string vendor)
        {
            var schema = textFolder.GetSchema();

            var textContent = new TextContent();
            foreach (string key in values)
            {
                textContent[key] = values[key];
            }
            textContent.Repository = textFolder.Repository.Name;
            textContent.SchemaName = textFolder.SchemaName;
            textContent.FolderName = textFolder.FullName;

            textContent = _textContentBinder.Bind(schema, textContent, values);

            if (!string.IsNullOrEmpty(values["UUID"]))
            {
                textContent.UUID = values["UUID"];
            }

            IncomingQueue incomeQueue = new IncomingQueue()
            {
                Message = null,
                Object = new Dictionary<string, object>(textContent),
                ObjectUUID = textContent.IntegrateId,
                ObjectTitle = textContent.GetSummary(),
                Vendor = vendor,
                PublishingObject = PublishingObject.TextContent,
                Action = PublishingAction.Publish,
                SiteName = site.FullName,
                Status = QueueStatus.Pending,
                UtcCreationDate = DateTime.UtcNow,
                UtcProcessedTime = null,
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
            };
            _incomeQueueProvider.Add(incomeQueue);

            return textContent.IntegrateId;
        }

        public string UpdateTextContent(Site site, TextFolder textFolder, string uuid, System.Collections.Specialized.NameValueCollection values, [System.Runtime.InteropServices.OptionalAttribute][System.Runtime.InteropServices.DefaultParameterValueAttribute("")]string userid, string vendor)
        {
            var schema = textFolder.GetSchema();
            var textContent = new TextContent(textFolder.Repository.Name, textFolder.SchemaName, textFolder.FullName);

            textContent = _textContentBinder.Bind(schema, textContent, values);

            IncomingQueue incomeQueue = new IncomingQueue()
            {
                Message = null,
                Object = new Dictionary<string, object>(textContent),
                ObjectUUID = textContent.IntegrateId,
                ObjectTitle = textContent.GetSummary(),
                Vendor = vendor,
                PublishingObject = PublishingObject.TextContent,
                Action = PublishingAction.Publish,
                SiteName = site.FullName,
                Status = QueueStatus.Pending,
                UtcCreationDate = DateTime.UtcNow,
                UtcProcessedTime = null,
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
            };
            _incomeQueueProvider.Add(incomeQueue);

            return textContent.IntegrateId;
        }

        public void DeleteTextContent(Site site, TextFolder textFolder, string uuid, string vendor)
        {
            var integrateId = new ContentIntegrateId(uuid);
            var content = textFolder.CreateQuery().WhereEquals("UUID", integrateId.ContentUUID).FirstOrDefault();
            if (content != null)
            {
                IncomingQueue incomeQueue = new IncomingQueue()
                {
                    Message = null,
                    Object = null,
                    ObjectUUID = uuid,
                    ObjectTitle = content.GetSummary(),
                    Vendor = vendor,
                    PublishingObject = PublishingObject.TextContent,
                    Action = PublishingAction.Unbpulish,
                    SiteName = site.FullName,
                    Status = QueueStatus.Pending,
                    UtcCreationDate = DateTime.UtcNow,
                    UtcProcessedTime = null,
                    UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
                };
                _incomeQueueProvider.Add(incomeQueue);
            }
        }

        #endregion

        #region Page
        public string AddPage(Site site, Page page, string vendor)
        {
            IncomingQueue incomeQueue = new IncomingQueue()
            {
                Message = null,
                Object = page,
                ObjectUUID = page.FullName,
                ObjectTitle = page.FullName,
                Vendor = vendor,
                PublishingObject = PublishingObject.Page,
                Action = PublishingAction.Publish,
                SiteName = site.FullName,
                Status = QueueStatus.Pending,
                UtcCreationDate = DateTime.UtcNow,
                UtcProcessedTime = null,
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
            };
            _incomeQueueProvider.Add(incomeQueue);

            return page.FullName;
        }

        public string UpdatePage(Site site, Page page, string vendor)
        {
            IncomingQueue incomeQueue = new IncomingQueue()
            {
                Message = null,
                Object = page,
                ObjectUUID = page.FullName,
                ObjectTitle = page.FullName,
                Vendor = vendor,
                PublishingObject = PublishingObject.Page,
                Action = PublishingAction.Publish,
                SiteName = site.FullName,
                Status = QueueStatus.Pending,
                UtcCreationDate = DateTime.UtcNow,
                UtcProcessedTime = null,
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
            };
            _incomeQueueProvider.Add(incomeQueue);

            return page.FullName;
        }

        public void DeletePage(Site site, string pageId, string vendor)
        {
            IncomingQueue incomeQueue = new IncomingQueue()
            {
                Message = null,
                Object = null,
                ObjectUUID = pageId,
                ObjectTitle = pageId,
                Vendor = vendor,
                PublishingObject = PublishingObject.Page,
                Action = PublishingAction.Unbpulish,
                SiteName = site.FullName,
                Status = QueueStatus.Pending,
                UtcCreationDate = DateTime.UtcNow,
                UtcProcessedTime = null,
                UUID = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10)
            };
            _incomeQueueProvider.Add(incomeQueue);
        }
        #endregion
    }
}
