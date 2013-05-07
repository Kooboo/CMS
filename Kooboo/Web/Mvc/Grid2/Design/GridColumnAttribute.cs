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

namespace Kooboo.Web.Mvc.Grid2.Design
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public class GridColumnAttribute : Attribute
    {
        public string HeaderText { get; set; }
        /// <summary>
        /// 实现IGridColumn接口
        /// 用于自定义输出列头
        /// </summary>
        /// <value>
        /// The type of the grid column.
        /// </value>
        public Type GridColumnType { get; set; }

        /// <summary>
        /// 实现IGridItemColumn接口
        /// 用于自定义输出行的列
        /// </summary>
        /// <value>
        /// The type of the grid item column.
        /// </value>
        public Type GridItemColumnType { get; set; }

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
