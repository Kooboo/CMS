#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Menu
{
    public class ScriptFolderMenuItems : FileFolderMenuItems
    {
        public override FileManager FileManager
        {
            get { return Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<ScriptManager>(); }
        }
    }
}