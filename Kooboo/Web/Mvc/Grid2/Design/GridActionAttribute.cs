using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Mvc.Grid2.Design
{
    public class GridActionAttribute : GridColumnAttribute
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public string ConfirmMessage { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string RouteValueProperties { get; set; }
    }
}
