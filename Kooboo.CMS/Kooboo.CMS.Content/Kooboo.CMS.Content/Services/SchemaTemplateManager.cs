#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(SchemaTemplateManager))]
    public class SchemaTemplateManager : ItemTemplateManager
    {
        protected override string TemplatePath
        {
            get
            {
                return AreaHelpers.CombineAreaFilePhysicalPath("Contents", "Templates", "Schema");
            }
        }
    }
}
