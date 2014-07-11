#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    [Dependency(typeof(IDataSourceDesigner), Key = "Content datasource")]
    public class ContentDataSourceDesigner : IDataSourceDesigner
    {
        public string Name
        {
            get { return "Content datasource"; }
        }

        public IDataSource CreateDataSource()
        {
            return new FolderDataSource();
        }

        public bool IsEditorFor(IDataSource dataSource)
        {
            return dataSource is FolderDataSource;
        }

        public string DesignerVirtualPath
        {
            get
            {
                return Kooboo.Common.Web.UrlUtility.Combine("~", "Areas", "Sites", "Views", "DataSource", "ContentDatabase", "_Index.cshtml");
            }
        }
    }
}
