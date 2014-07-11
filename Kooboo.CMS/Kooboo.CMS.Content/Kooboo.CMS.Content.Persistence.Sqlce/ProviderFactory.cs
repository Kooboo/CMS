#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.Common.ObjectContainer;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{

    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProviderFactory), Order = 2)]
    public class ProviderFactory : Default.ProviderFactory
    {
        #region IProviderFactory Members

        public override string Name
        {
            get { return "Sqlce"; }
        }


        #endregion

        //public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        //{
        //    containerManager.AddComponent<IProviderFactory, ProviderFactory>();
        //    containerManager.AddComponent<ISchemaProvider, SchemaProvider>();
        //    containerManager.AddComponent<IContentProvider<TextContent>, TextContentProvider>();
        //    containerManager.AddComponent<ITextContentProvider, TextContentProvider>();
        //}

        //public int Order
        //{
        //    get { return 1; }
        //}
    }
}
