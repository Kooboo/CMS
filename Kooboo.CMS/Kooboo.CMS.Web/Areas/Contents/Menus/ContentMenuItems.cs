using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using Kooboo.CMS.Content.Models;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Menus
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