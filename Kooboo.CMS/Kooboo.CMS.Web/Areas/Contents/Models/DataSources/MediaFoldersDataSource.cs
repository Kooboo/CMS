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
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Models.DataSources
{
    public class MediaFoldersDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var folders = Kooboo.CMS.Content.Services.ServiceFactory.MediaFolderManager.FolderTrees(Repository.Current);

            foreach (var node in folders)
            {
                ToSelectListItem(node, list, null);
            }

            return list;
        }
        private void ToSelectListItem(FolderTreeNode<MediaFolder> node, List<SelectListItem> list, SelectListItem parent)
        {
            SelectListItemTree listItem = new SelectListItemTree()
            {
                Text = (parent == null ? "" : parent.Text + " >> ") + node.Folder.FriendlyText,
                Value = node.Folder.FullName
            };

            list.Add(listItem);

            if (node.Children != null)
            {
                foreach (var item in node.Children)
                {
                    ToSelectListItem(item, list, listItem);
                }
            }
        }
    }
}