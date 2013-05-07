#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Models
{
    [Grid()]
    public class RelationModel
    {
        [GridColumn(HeaderText = "Used in")]
        public string RelationName { get; set; }
        [GridColumn(HeaderText = "Usage")]
        public string RelationType { get; set; }
    }
}