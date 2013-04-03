using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public static class FolderTreeNodeExtensions
    {
        public static IEnumerable<SelectListItemTree> ToSelectListItemTrees<T>(this IEnumerable<FolderTreeNode<T>> nodes)
            where T : Folder
        {
            return nodes.Select(it => ToSelectListItemTree(it));
        }
        public static SelectListItemTree ToSelectListItemTree<T>(FolderTreeNode<T> node)
            where T : Folder
        {
            SelectListItemTree listItem = new SelectListItemTree()
            {
                Text = node.Folder.FriendlyText,
                Value = node.Folder.FullName
            };
            listItem.Items = node.Children.ToSelectListItemTrees();
            return listItem;
        }
    }
    public class IMediaFolderDataSource : ISelectListDataSource
    {

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            var folders = Kooboo.CMS.Content.Services.ServiceFactory.MediaFolderManager.FolderTrees(Repository.Current);
            return folders.ToSelectListItemTrees();
        }
    }
    public class MoveMediaContent
    {
        [RemoteEx("TargetFolderAvailable", "*", RouteFields = "FolderName,SiteName")]
        [UIHint("DropDownListTree")]
        [DataSource(typeof(IMediaFolderDataSource))]
        public string TargetFolder { get; set; }
    }
}