#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.CMIS.WcfExtensions;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IService))]
    [ExceptionHandlerBehaviour(typeof(ExceptionHandler))]
    public partial class Service : IService
    {
        #region .ctor
        SiteManager _siteManager;
        TextFolderManager _textFolderManager;
        RepositoryManager _repositoryManager;
        SchemaManager _schemaManager;
        IIncomeDataManager _incomeDataManager;
        ITextContentProvider _textContentProvider;
        public Service(RepositoryManager repositoryManager, SchemaManager schemaManager, TextFolderManager textFolderManager, SiteManager siteManager
            , IIncomeDataManager incomeDataManager, ITextContentProvider textContentProvider)
        {
            _textFolderManager = textFolderManager;
            _repositoryManager = repositoryManager;
            _schemaManager = schemaManager;
            _incomeDataManager = incomeDataManager;

            _siteManager = siteManager;
            _textContentProvider = textContentProvider;
        }
        #endregion
    }
}
