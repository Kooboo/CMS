using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;

namespace Kooboo.Web.Mvc.Grid2
{
    public class GridModel : IGridModel
    {
        #region Create GridModel

        public static IGridModel CreateGridModel(Type modelType, IEnumerable dataSource, ViewContext viewContext)
        {
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => null, modelType);

            if (!(modelMetadata is KoobooModelMetadata))
            {
                throw new InvalidOperationException("Please setup KoobooDataAnnotationsModelMetadataProvider, etc:ModelMetadataProviders.Current = new Kooboo.Web.Mvc.KoobooDataAnnotationsModelMetadataProvider()");
            }
            var gridAttribute = ((KoobooModelMetadata)modelMetadata).Attributes.OfType<GridAttribute>().FirstOrDefault();
            if (gridAttribute != null)
            {
                if (gridAttribute.GridModelType != null)
                {
                    return (IGridModel)Activator.CreateInstance(gridAttribute.GridModelType,
                        new object[] { gridAttribute, modelMetadata, dataSource, viewContext });
                }
                else
                {
                    return new GridModel(gridAttribute, modelMetadata, dataSource, viewContext);
                }
            }
            else
            {
                throw new InvalidOperationException(string.Format("The model metadata type of '{0}' does not have GridAttribute.", modelType));
            }
        }

        #endregion

        #region .ctor
        public GridModel(GridAttribute gridAtt, ModelMetadata modelMetadata, IEnumerable dataSource, ViewContext viewContext)
        {
            this.GridAttribute = gridAtt;

            this.ModelMetadata = modelMetadata;

            DataSource = dataSource;

            this.ViewContext = viewContext;

            Initialize();
        }

        protected virtual void Initialize()
        {
            this.Checkable = GridAttribute.Checkable;
            this.IdPorperty = GridAttribute.IdProperty;
        }
        #endregion

        #region Properties
        public virtual GridAttribute GridAttribute { get; set; }

        public virtual ModelMetadata ModelMetadata { get; set; }

        public virtual IEnumerable DataSource { get; set; }

        public virtual bool Checkable { get; set; }

        public virtual string IdPorperty { get; set; }

        public virtual ViewContext ViewContext { get; set; }

        private IEnumerable<IGridColumn> _columns;
        /// <summary>
        ///  获取所有的列定义
        /// </summary>
        public virtual IEnumerable<IGridColumn> Columns
        {
            get
            {
                if (_columns == null)
                {
                    var columns = new List<IGridColumn>();

                    columns.AddRange(GetColumns(null, ModelMetadata));

                    foreach (var propertyMetadata in ModelMetadata.Properties)
                    {
                        var column = GetColumns(propertyMetadata.PropertyName, propertyMetadata);
                        if (column != null)
                        {
                            columns.AddRange(column);
                        }
                    }
                    _columns = columns.OrderBy(go => go.Order).ToArray();
                }
                return _columns;
            }
        }

        #endregion


        #region GetColumns
        protected virtual IEnumerable<IGridColumn> GetColumns(string propertyName, ModelMetadata propertyMetadata)
        {
            foreach (var att in ((KoobooModelMetadata)propertyMetadata).Attributes.OfType<GridColumnAttribute>())
            {
                IGridColumn gridColumn = null;
                if (att.GridColumnType != null)
                {
                    gridColumn = (IGridColumn)Activator.CreateInstance(att.GridColumnType,
                        new object[] { this, att, propertyName, att.Order });
                }
                else
                {
                    gridColumn = new GridColumn(this, att, propertyName, att.Order);
                }
                yield return gridColumn;
            }
        }
        #endregion

        /// <summary>
        /// 返回集合中每个数据对应的列表行
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IGridItem> GetItems()
        {
            List<IGridItem> items = new List<IGridItem>();

            if (DataSource != null)
            {
                int i = 0;
                foreach (var dataItem in DataSource)
                {
                    if (GridAttribute.GridItemType != null)
                    {
                        items.Add((IGridItem)Activator.CreateInstance(GridAttribute.GridItemType, new object[] { this, dataItem, i }));
                    }
                    else
                    {
                        items.Add(new GridItem(this, dataItem, i));
                    }

                    i++;
                }
            }
            return items;
        }
    }
}
