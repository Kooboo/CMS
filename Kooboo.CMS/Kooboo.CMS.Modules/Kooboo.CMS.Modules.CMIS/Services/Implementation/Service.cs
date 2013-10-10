#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Modules.CMIS.WcfExtensions;
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
        TextFolderManager _textFolderManager;
        RepositoryManager _repositoryManager;
        SchemaManager _schemaManager;
        IIncomeDataManager _incomeDataManager;
        public Service(RepositoryManager repositoryManager, SchemaManager schemaManager, TextFolderManager textFolderManager
            , IIncomeDataManager incomeDataManager)
        {
            _textFolderManager = textFolderManager;
            _repositoryManager = repositoryManager;
            _schemaManager = schemaManager;
            _incomeDataManager = incomeDataManager;
        }
        #endregion
    }
}
