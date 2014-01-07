#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    public partial class Service : IPageService
    {
        public string AddPage(string repositoryId, string pageId, Page properties)
        {
            var site = ModelHelper.GetSite(repositoryId);

            var page = new Page(site, pageId);

            MapProperties(properties, page);

            _incomeDataManager.AddPage(site, page, ContextHelper.GetVendor());

            return page.FullName;
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

        public string UpdatePage(string repositoryId, string pageId, Page properties)
        {
            var site = ModelHelper.GetSite(repositoryId);

            var page = new Page(site, pageId);

            MapProperties(properties, page);

            _incomeDataManager.UpdatePage(site, page, ContextHelper.GetVendor());

            return page.FullName;
        }

        public void DeletePage(string repositoryId, string pageId)
        {
            var site = ModelHelper.GetSite(repositoryId);

            _incomeDataManager.DeletePage(site, pageId, ContextHelper.GetVendor());
        }
    }
}
