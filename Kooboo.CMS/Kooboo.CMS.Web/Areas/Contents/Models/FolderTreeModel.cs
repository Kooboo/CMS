using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class FolderTreeModel
    {
        public FolderTreeModel(string repositoryName)
        {
            BindDataSource(new Repository(repositoryName).AsActual());
            Values = new List<string>();
        }

        public FolderTreeModel()
        {
            BindDataSource(Repository.Current);
        }

        private void BindDataSource(Repository repository)
        {
            this.DataSource = Kooboo.CMS.Content.Services.ServiceFactory.TextFolderManager.FolderTrees(repository);
        }

        public bool IsSingle { get; set; }
        public string InputName { get; set; }
        public bool AllowNull { get; set; }
        public object HtmlAttr { get; set; }

        public List<string> Values { get; set; }

        public IEnumerable<FolderTreeNode<TextFolder>> DataSource { get; set; }
    }
}