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
using Kooboo.CMS.Sites.Models;

using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [MetadataFor(typeof(Theme))]
	[GridAction(ActionName = "Styles", Order = 0, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "Name")]
	[GridAction(ActionName = "Images", Order = 4, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "Name")]
	[GridAction(ActionName = "Rules", Order = 6, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "Name")]
	[GridAction(ActionName = "Export", Order = 8, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "Name")]
	[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this theme?", Order = 10, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "Name")]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", ConfirmMessage = "Are you sure you want to localize this item?",
	RouteValueProperties = "Name,fromSite=Site", Order = 12, Renderer = typeof(LocalizationRender))]
	public class Theme_Metadata
	{
		[GridColumn(Order = 1)]
		public string Name { get; set; }
		[GridColumn(Order = 2)]
		public Site Site { get; set; }
	}
}