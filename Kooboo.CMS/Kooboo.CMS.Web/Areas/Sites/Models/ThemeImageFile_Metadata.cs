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
using Kooboo.Web.Mvc.Grid;
using Kooboo.Web;
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web.Grid.Design;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(ThemeImageFile))]
    [GridAction(ActionName = "DeleteImage", ConfirmMessage = "Are you sure you want to delete this image?", DisplayName = "Delete", RouteValueProperties = "Theme.Name,FileName")]
    public class ThemeImageFile_Metadata
    {
        [GridColumn(Order = 0)]
        public string Name { get; set; }
        [GridColumn(Order = 1, ItemRenderType = typeof(ImageRender), HeaderText = "Preview")]
        public string VirtualPath { get; set; }


    }
}