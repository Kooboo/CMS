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
using System.Web;
using Kooboo.Common.Web.Menu;
using System.Web.Mvc;


using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
namespace Kooboo.CMS.Web.Areas.Contents.Menu
{
    public class MediaMenuItems : FolderMenuItems<MediaFolder>
    {
        protected override FolderManager<MediaFolder> FolderManager
        {
            get { return ServiceFactory.MediaFolderManager; }
        }
    }
}