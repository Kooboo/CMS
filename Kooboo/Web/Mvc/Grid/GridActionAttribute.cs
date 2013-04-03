using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc.Grid
{
    public interface IColumnVisibleArbiter
    {
        bool IsVisible(ViewContext viewContext);
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GridActionAttribute : Attribute
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public string ConfirmMessage { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        /// <summary>
        /// Gets or sets the route value properties.
        /// <example>
        /// 1. "Name,Property2"
        /// 2. "Name=FullName" : "FullName" is the model property, "Name" is the alias for RouteValueName in RouteValueDictionary.
        /// </example>
        /// </summary>
        /// <value>The route value properties.</value>
        public string RouteValueProperties { get; set; }

        public string CellVisibleProperty { get; set; }
        /// <summary>
        /// A type of  IVisibleArbiter
        /// </summary>
        /// <value>The visible arbiter.</value>
        public Type CellVisibleArbiter { get; set; }
        /// <summary>
        /// typeof IGridActionRouteValuesGetter
        /// </summary>
        public Type RouteValuesGetter { get; set; }
        /// <summary>
        /// A type of IColumnVisibleArbiter
        /// </summary>
        /// <value>
        /// The column visible arbiter.
        /// </value>
        public Type ColumnVisibleArbiter { get; set; }
        /// <summary>
        /// type of IGridItemActionRender
        /// </summary>
        public Type Renderer { get; set; }

        private bool inheritRouteValues = true;
        public bool InheritRouteValues { get { return inheritRouteValues; } set { inheritRouteValues = value; } }
        public int Order { get; set; }

        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }
}
