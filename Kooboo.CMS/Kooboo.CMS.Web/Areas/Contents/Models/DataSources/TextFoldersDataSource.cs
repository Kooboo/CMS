#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class TextFoldersDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            var repository = Repository.Current;

            List<SelectListItem> list = new List<SelectListItem>();
            ServiceFactory.TextFolderManager.All(repository, "")
                .ForEach((textFolder, index) =>
                {
                    AddSelectListItem(textFolder, list);
                });

            return list;
        }


        private void AddSelectListItem(TextFolder textFolder, List<SelectListItem> list, SelectListItem parent = null)
        {
            SelectListItemTree item = new SelectListItemTree
            {
                Text = (parent == null ? "" : parent.Text + " >> ") + textFolder.FriendlyText,
                Value = textFolder.FullName
            };

            list.Add(item);

            var childFolders = ServiceFactory.TextFolderManager.ChildFolders(textFolder);

            if (childFolders != null)
            {
                childFolders.ForEach((folder, index) =>
                {
                    AddSelectListItem(folder, list, item);
                });
            }
        }
    }

}