﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Models
{
    [MetadataFor(typeof(Kooboo.CMS.Common.Persistence.Non_Relational.RelationModel))]
    [Grid()]
    public class RelationModel_Metadata
    {
        [GridColumn(HeaderText = "Used in")]
        public string DisplayName { get; set; }
        public string ObjectUUID { get; set; }
        [GridColumn(HeaderText = "Usage")]
        public string RelationType { get; set; }
        public object RelationObject { get; set; }
    }
}