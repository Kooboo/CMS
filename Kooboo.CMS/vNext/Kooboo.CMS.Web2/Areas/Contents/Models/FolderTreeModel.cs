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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Kooboo.CMS.Web2.Areas.Contents.Models
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
        public bool IsEditing { get; set; }
        public bool IsSingle { get; set; }
        public string InputName { get; set; }
        public bool AllowNull { get; set; }
        public IDictionary<string, object> InputAttributes { get; set; }

        public List<string> Values { get; set; }

        public IEnumerable<FolderTreeNode<TextFolder>> DataSource { get; set; }
    }
}