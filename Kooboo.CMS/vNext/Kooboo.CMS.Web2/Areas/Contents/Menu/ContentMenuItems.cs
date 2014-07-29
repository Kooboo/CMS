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

using Kooboo.Common.Web.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web2.Areas.Contents.Menu
{
    public abstract class FolderMenuItems<T> : IMenuItemContainer
        where T : Folder
    {
        protected abstract FolderManager<T> FolderManager { get; }
        protected virtual IEnumerable<MenuItem> GetContentFolderItems(Repository repository)
        {
            if (repository == null)
            {
                return new MenuItem[0];
            }
            var folders = FolderManager.All(repository, "");
            List<MenuItem> items = new List<MenuItem>();
            foreach (var folder in folders)
            {
                items.Add(CreateFolderMenuItem(folder));
            }

            return items;
        }
        protected virtual MenuItem CreateFolderMenuItem(T folder)
        {
            MenuItem menuItem = new FolderMenuItem(folder.AsActual());
            var childFolders = FolderManager.ChildFolders(folder);
            List<MenuItem> items = new List<MenuItem>();
            menuItem.Items = items;
            foreach (var child in childFolders)
            {
                items.Add(CreateFolderMenuItem(child));
            }
            return menuItem;
        }


        #region IMenuItemContainer Members

        public virtual IEnumerable<MenuItem> GetItems(string areaName, ControllerContext controllerContext)
        {
            var repository = Repository.Current;

            return GetContentFolderItems(repository);
        }

        #endregion
    }

    public class ContentMenuItems : FolderMenuItems<TextFolder>
    {
        protected override FolderManager<TextFolder> FolderManager
        {
            get { return ServiceFactory.TextFolderManager; }
        }
    }
}