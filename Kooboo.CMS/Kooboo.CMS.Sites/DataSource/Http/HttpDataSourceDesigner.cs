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

namespace Kooboo.CMS.Sites.DataSource.Http
{
    [Dependency(typeof(IDataSourceDesigner), Key = "HTTP")]
    public class HttpDataSourceDesigner : IDataSourceDesigner
    {
        public string Name
        {
            get { return "HTTP"; }
        }

        public IDataSource CreateDataSource()
        {
            return new HttpDataSource();
        }

        public bool IsEditorFor(IDataSource dataSource)
        {
            return dataSource is HttpDataSource;
        }

        public string DesignerVirtualPath
        {
            get
            {
                return Kooboo.Common.Web.UrlUtility.Combine("~","Areas", "Sites", "Views", "DataSource", "Http", "_Http.cshtml");
            }
        }
    }
}
