using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources
{
    public class TextFoldersDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            var repository = Site.Current.GetRepository();

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
                Text = (parent == null ? "" : parent.Text + "/") + textFolder.FriendlyText,
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