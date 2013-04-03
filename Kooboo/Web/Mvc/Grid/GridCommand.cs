using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Kooboo.Web.Mvc.Grid
{
    public class GridCommand
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string DisplayName { get; set; }
        public string ConfirmMessage { get; set; }

        public string CommandName { get; set; }

        public RouteValueDictionary RouteValues { get; set; }
    }
}
