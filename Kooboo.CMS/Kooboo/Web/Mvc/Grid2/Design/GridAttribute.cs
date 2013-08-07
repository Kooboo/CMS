﻿#region License
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

namespace Kooboo.Web.Mvc.Grid2.Design
{
    public class GridAttribute : Attribute
    {
        public bool Checkable { get; set; }
        public bool Draggable { get; set; }
        public string IdProperty { get; set; }
        /// <summary>
        /// The custom grid model type inherit from IGridModel.
        /// </summary>
        public Type GridModelType { get; set; }

        /// <summary>
        /// The custom grid item type inherit from IGridItem
        /// </summary>
        /// <value>
        /// The type of the grid item.
        /// </value>
        public Type GridItemType { get; set; }

        public string EmptyText { get; set; }
    }
}
