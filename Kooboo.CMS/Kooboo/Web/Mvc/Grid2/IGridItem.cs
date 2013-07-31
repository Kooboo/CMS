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
using System.Web;
using Kooboo.Reflection;
namespace Kooboo.Web.Mvc.Grid2
{
    public interface IGridItem
    {
        /// <summary>
        /// 对应的GridModel对象
        /// </summary>
        IGridModel GridModel { get; }
        /// <summary>
        /// 原始数据对象
        /// </summary>
        object DataItem { get; }
        /// <summary>
        /// 数据下标
        /// </summary>
        /// <value>
        /// The index of the data.
        /// </value>
        int DataIndex { get; }
        /// <summary>
        /// 是否为奇数行
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is odd item; otherwise, <c>false</c>.
        /// </value>
        bool IsOddItem { get; }

        /// <summary>
        /// 该行的主键
        /// </summary>
        object IdPropertyValue { get; }

        /// <summary>
        /// 该行是否可选择
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is checkable; otherwise, <c>false</c>.
        /// </value>
        bool IsCheckable { get; }
        /// <summary>
        /// 输出tr（容器）的HTML属性
        /// </summary>
        /// <returns></returns>
        IHtmlString RenderItemContainerAtts();

        /// <summary>
        /// 返回该行的所有的字段的展现对象
        /// </summary>
        /// <returns></returns>
        IEnumerable<IGridItemColumn> GetItemColumns();
    }

    public class GridItem : IGridItem
    {
        public GridItem(IGridModel gridModel, object dataItem, int dataIndex)
        {
            this.GridModel = gridModel;
            this.DataItem = dataItem;
            this.DataIndex = dataIndex;
            this.IsOddItem = ((DataIndex+1) % 2) != 0;
        }

        /// <summary>
        /// 对应的GridModel对象
        /// </summary>
        public virtual IGridModel GridModel
        {
            get;
            private set;
        }

        /// <summary>
        /// 原始数据对象
        /// </summary>
        public virtual object DataItem
        {
            get;
            private set;
        }

        /// <summary>
        /// 数据下标
        /// </summary>
        /// <value>
        /// The index of the data.
        /// </value>
        public virtual int DataIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为奇数行
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is odd item; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsOddItem
        {
            get;
            private set;
        }

        public virtual object IdPropertyValue
        {
            get
            {
                return GetPropertyValue(DataItem, GridModel.IdPorperty);
            }
        }

        public virtual IHtmlString RenderItemContainerAtts()
        {           
            return new HtmlString("");
        }

        /// <summary>
        /// 返回该行的所有的字段的展现对象
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IGridItemColumn> GetItemColumns()
        {
            List<IGridItemColumn> itemColumns = new List<IGridItemColumn>();
            foreach (var column in GridModel.Columns)
            {
                object propertyValue = null;
                if (!string.IsNullOrEmpty(column.PropertyName))
                {
                    propertyValue = GetPropertyValue(DataItem, column.PropertyName);
                }

                if (column.ColumnAttribute.GridItemColumnType != null)
                {
                    itemColumns.Add((IGridItemColumn)Activator.CreateInstance(column.ColumnAttribute.GridItemColumnType, new object[] { column, DataItem, propertyValue }));
                }
                else
                {
                    itemColumns.Add(new GridItemColumn(column, DataItem, propertyValue));
                }
            }
            return itemColumns;
        }

        private object GetPropertyValue(object dataItem, string propertyName)
        {
            return dataItem.Members().Properties[propertyName];
        }


        public virtual bool IsCheckable
        {
            get
            {
                return true;
            }
        }
    }
}
