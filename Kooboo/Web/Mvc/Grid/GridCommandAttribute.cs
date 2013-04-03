using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.Grid
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GridCommandAttribute : Attribute
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public string ConfirmMessage { get; set; }

        public string CommandName { get; set; }

        private bool inheritRouteValues = true;
        public bool InheritRouteValues
        {
            get
            {
                return inheritRouteValues;
            }
            set { inheritRouteValues = value; }
        }

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
