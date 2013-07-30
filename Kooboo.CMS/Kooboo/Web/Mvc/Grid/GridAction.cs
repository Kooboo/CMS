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
using System.Web.Routing;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Grid
{
    public interface IVisibleArbiter
    {
        bool IsVisible(object dataItem, ViewContext viewContext);
    }
    public interface IGridActionRouteValuesGetter
    {
        RouteValueDictionary GetValues(object dataItem, RouteValueDictionary routeValueDictionary, ViewContext viewContext);
    }
    public class GridActionRouteValuesSetting
    {
        public string RouteValueName { get; set; }
        public string PropertyName { get; set; }
    }
    public class GridAction
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public string ConfirmMessage { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        public string VisibleProperty { get; set; }

        public IVisibleArbiter VisibleArbiter { get; set; }

        public IEnumerable<GridActionRouteValuesSetting> RouteValuesSetting { get; set; }

        public IGridActionRouteValuesGetter RouteValuesGetter { get; set; }

        public IGridItemActionRender Renderer { get; set; }

        private bool inheritRouteValues = true;
        public bool InheritRouteValues { get { return inheritRouteValues; } set { inheritRouteValues = value; } }

    }
}
