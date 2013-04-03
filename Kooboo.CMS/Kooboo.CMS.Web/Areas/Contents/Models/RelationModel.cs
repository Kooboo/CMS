using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{

    public class RelationModel
    {
        [GridColumn()]
        public string RelationName { get; set; }
        [GridColumn()]
        public string RelationType { get; set; }
    }
}