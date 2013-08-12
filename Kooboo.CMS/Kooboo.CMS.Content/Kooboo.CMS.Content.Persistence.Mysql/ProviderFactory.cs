﻿#region License
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
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Content.Persistence.Mysql
{

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProviderFactory), Order = 2)]
    public class ProviderFactory : Default.ProviderFactory
    {
        public const string ProviderName = "Mysql";

        #region IProviderFactory Members

        public override string Name
        {
            get { return ProviderName; }
        }

        #endregion


        //public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        //{
        //    containerManager.AddComponent<IProviderFactory, ProviderFactory>();
        //    containerManager.AddComponent<IRepositoryProvider, RepositoryProvider>();
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
