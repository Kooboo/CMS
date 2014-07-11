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
 
using Kooboo.Common.ComponentModel;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web.Grid.Design;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(StyleFile))]
    [GridAction(ActionName = "DeleteStyle",
        ConfirmMessage = "Are you sure you want to delete this file?",
        Order = 3,
        RouteValueProperties = "FileName", DisplayName = "Delete"
        )]
    [GridAction(ActionName = "EditStyle",
        Order = 0,
        DisplayName = "Edit", RouteValueProperties = "FileName", Icon = "Edit.gif"
        )]
    public class Style_Metadata
    {
        [GridColumn()]
        public string Name { get; set; }
    }
}