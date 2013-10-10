#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    public interface IIncomeDataManager
    {
        #region TextContent
        string AddTextContent(Site site, TextFolder textFolder, NameValueCollection values, HttpFileCollectionBase files,
  IEnumerable<TextContent> categories, string userid, string vendor);

        string UpdateTextContent(Site site, TextFolder folder, string integrateId, NameValueCollection values, string userid, string vendor);

        void DeleteTextContent(Site site, TextFolder textFolder, string integrateId, string vendor);
        #endregion

        #region Page
        string AddPage(Site site, Page page, string vendor);
        string UpdatePage(Site site, Page page, string vendor);
        void DeletePage(Site site, string pageId, string vendor);
        #endregion
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IIncomeDataManager))]
    public class IncomeDataManager : IIncomeDataManager
    {
        #region .ctor
        TextContentManager _textContentManager;
        PageManager _pageManager;
        public IncomeDataManager(TextContentManager textContentManager, PageManager pageManager)
        {
            _textContentManager = textContentManager;
            _pageManager = pageManager;
        }
        #endregion

        #region TextContent
        public string AddTextContent(Site site, TextFolder textFolder, NameValueCollection values, HttpFileCollectionBase files, IEnumerable<TextContent> categories, string userid, string vendor)
        {
            var textContent = _textContentManager.Add(textFolder.Repository, textFolder, values, files, categories, "");
            return textContent.IntegrateId;
        }

        public string UpdateTextContent(Site site, TextFolder textFolder, string integrateId, NameValueCollection values, string userid, string vendor)
        {
            var integrate = new ContentIntegrateId(integrateId);
            var textContent = _textContentManager.Update(textFolder.Repository, textFolder, integrate.ContentUUID, values, "");
            return textContent.IntegrateId;
        }

        public void DeleteTextContent(Site site, TextFolder textFolder, string integrateId, string vendor)
        {
            var integrate = new ContentIntegrateId(integrateId);
            _textContentManager.Delete(textFolder.Repository, textFolder, integrate.ContentUUID);
        }
        #endregion

        #region Page

        public string AddPage(Site site, Page page, string vendor)
        {
            _pageManager.Add(site, page);
            return page.FullName;
        }

        public string UpdatePage(Site site, Page page, string vendor)
        {
            _pageManager.Update(site, page, page);
            return page.FullName;
        }

        public void DeletePage(Site site, string pageId, string vendor)
        {
            var page = new Kooboo.CMS.Sites.Models.Page(site, pageId);
            _pageManager.Remove(site, page);
        }
        #endregion
    }
}
