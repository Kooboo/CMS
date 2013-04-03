using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using System.Web.Routing;

namespace Kooboo.Web.Mvc.Grid
{
    public class GridModel
    {
        public GridModel(Type modelType, IEnumerable dataSource, ViewContext viewContext)
        {
            this.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => null, modelType);

            DataSource = dataSource;

            Initialize(ModelMetadata, dataSource, viewContext);
        }

        public ModelMetadata ModelMetadata { get; private set; }
        public IEnumerable DataSource { get; private set; }
        public IEnumerable<GridColumn> GridColumns { get; private set; }
        public IEnumerable<GridItem> GridItems { get; private set; }
        public IEnumerable<GridAction> GridActions { get; private set; }
        public IEnumerable<GridCommandAttribute> GridCommandDefinitions { get; private set; }
        public PageSizeAttribute PageSizeDifinition { get; private set; }
        public bool Checkable { get; set; }
        public IVisibleArbiter CheckVisible { get; set; }
        public string IdPorperty { get; set; }
        public bool IsMasterDetail
        {
            get
            {
                return !string.IsNullOrEmpty(ItemDetailView);
            }
        }
        public string ItemDetailView { get; set; }

        public IEnumerable<PageSizeCommand> GetPageSizeCommands(ViewContext viewContext)
        {
            if (PageSizeDifinition != null)
            {
                foreach (var item in PageSizeDifinition.SizeOptions.Split(','))
                {
                    yield return new PageSizeCommand(viewContext, PageSizeDifinition.PageIndexName, item);
                }
            }
        }
        public IEnumerable<GridCommand> GetCommands(ViewContext viewContext)
        {
            foreach (GridCommandAttribute command in GridCommandDefinitions)
            {
                yield return new GridCommand()
                {
                    ActionName = command.ActionName,
                    CommandName = command.CommandName,
                    ConfirmMessage = command.ConfirmMessage,
                    ControllerName = command.ControllerName,
                    DisplayName = string.IsNullOrEmpty(command.DisplayName) ? command.ActionName : command.DisplayName,
                    RouteValues = command.InheritRouteValues ? viewContext.RequestContext.AllRouteValues() : new RouteValueDictionary()
                };
            }
        }

        protected virtual IEnumerable<GridItem> GetItems(IEnumerable dataSource, IEnumerable<GridColumn> columns, IEnumerable<GridAction> gridActions)
        {
            List<GridItem> items = new List<GridItem>();

            if (dataSource != null)
            {
                int i = 0;
                foreach (var dataItem in dataSource)
                {
                    items.Add(new GridItem(i, IdPorperty, dataItem, columns, GridActions, this.CheckVisible));
                    i++;
                }
            }

            return items;
        }

        protected virtual void Initialize(ModelMetadata modelMetadata, IEnumerable dataSource, ViewContext viewContext)
        {
            if (ModelMetadata is KoobooModelMetadata)
            {
                var gridAttribute = ((KoobooModelMetadata)modelMetadata).Attributes.OfType<GridAttribute>().FirstOrDefault();
                if (gridAttribute != null)
                {
                    this.Checkable = gridAttribute.Checkable;
                    if (gridAttribute.CheckVisible != null)
                    {
                        this.CheckVisible = (IVisibleArbiter)Activator.CreateInstance(gridAttribute.CheckVisible);
                    }
                    this.IdPorperty = gridAttribute.IdProperty;
                    this.ItemDetailView = gridAttribute.ItemDetailView;
                }
            }

            PageSizeDifinition = ((KoobooModelMetadata)modelMetadata).Attributes.OfType<PageSizeAttribute>().FirstOrDefault();

            GridColumns = GetColumns(ModelMetadata, viewContext);
            GridActions = GetActions(ModelMetadata, viewContext);
            GridItems = GetItems(this.DataSource, GridColumns, GridActions);
            GridCommandDefinitions = GetGridCommandDefinitions(ModelMetadata);
        }
        protected virtual IEnumerable<GridAction> GetActions(ModelMetadata modelMetadata, ViewContext viewContext)
        {
            List<GridAction> actions = new List<GridAction>();
            if (modelMetadata is KoobooModelMetadata)
            {
                foreach (var item in ((KoobooModelMetadata)modelMetadata).Attributes.OfType<GridActionAttribute>().OrderBy(it => it.Order))
                {
                    if (item.ColumnVisibleArbiter != null)
                    {
                        var columnArbiter = (IColumnVisibleArbiter)Activator.CreateInstance(item.ColumnVisibleArbiter);
                        if (!columnArbiter.IsVisible(viewContext))
                        {
                            continue;
                        }
                    }

                    var gridAction = new GridAction()
                    {
                        ActionName = item.ActionName,
                        ControllerName = item.ControllerName,
                        DisplayName = item.DisplayName ?? item.ActionName,
                        ConfirmMessage = item.ConfirmMessage,
                        VisibleProperty = item.CellVisibleProperty,
                        Icon = item.Icon,
                        Title = item.Title,
                        Class = item.Class,
                        InheritRouteValues = item.InheritRouteValues
                    };

                    if (!string.IsNullOrEmpty(item.RouteValueProperties))
                    {
                        List<GridActionRouteValuesSetting> settingList = new List<GridActionRouteValuesSetting>();
                        foreach (var property in item.RouteValueProperties.Split(','))
                        {
                            var setting = ParseRouteValueSetting(property);
                            if (setting != null)
                            {
                                settingList.Add(setting);
                            }
                        }
                        gridAction.RouteValuesSetting = settingList;
                    }
                    if (item.CellVisibleArbiter != null)
                    {
                        gridAction.VisibleArbiter = (IVisibleArbiter)Activator.CreateInstance(item.CellVisibleArbiter);
                    }
                    if (item.RouteValuesGetter != null)
                    {
                        gridAction.RouteValuesGetter = (IGridActionRouteValuesGetter)Activator.CreateInstance(item.RouteValuesGetter);
                    }
                    if (item.Renderer != null)
                    {
                        gridAction.Renderer = (IGridItemActionRender)Activator.CreateInstance(item.Renderer);
                    }

                    actions.Add(gridAction);
                }
            }
            else
            {
                //other ModelMetadata type....
            }
            return actions;
        }
        private static GridActionRouteValuesSetting ParseRouteValueSetting(string setting)
        {
            if (string.IsNullOrEmpty(setting))
            {
                return null;
            }
            var arr = setting.Split('=');
            GridActionRouteValuesSetting routeValuesSetting = new GridActionRouteValuesSetting();
            if (arr.Length > 1)
            {
                routeValuesSetting.RouteValueName = arr[0].Trim();
                routeValuesSetting.PropertyName = arr[1].Trim();
            }
            else
            {
                routeValuesSetting.RouteValueName = arr[0].Replace('.', '_').Trim();
                routeValuesSetting.PropertyName = arr[0].Trim();
            }
            return routeValuesSetting;
        }
        protected virtual IEnumerable<GridColumn> GetColumns(ModelMetadata modelMetadata, ViewContext viewContext)
        {
            var gridColumns = new List<GridColumn>();

            gridColumns.AddRange(GetGridColumns(modelMetadata));

            foreach (var propertyMetadata in modelMetadata.Properties)
            {
                var column = GetGridColumns(propertyMetadata);
                if (column != null)
                {
                    gridColumns.AddRange(column);
                }
            }
            return gridColumns.OrderBy(go => go.Order).ToArray();
        }
        protected virtual IEnumerable<GridColumn> GetGridColumns(ModelMetadata propertyMetadata)
        {
            if (ModelMetadata is KoobooModelMetadata)
            {
                foreach (var att in ((KoobooModelMetadata)propertyMetadata).Attributes.OfType<GridColumnAttribute>())
                {
                    GridColumn gridColumn = new GridColumn();
                    gridColumn.PropertyName = propertyMetadata.PropertyName;
                    gridColumn.HeaderText = att.HeaderText ?? propertyMetadata.GetDisplayName();
                    gridColumn.HeaderFormatString = att.HeaderFormatString;
                    gridColumn.ItemFormatString = att.ItemFormatString ?? propertyMetadata.DisplayFormatString;
                    gridColumn.Class = att.Class;
                    gridColumn.Order = att.Order;

                    if (att.HeaderRenderType != null)
                    {
                        gridColumn.HeaderRender = (IColumnHeaderRender)Activator.CreateInstance(att.HeaderRenderType);
                    }
                    if (att.ItemRenderType != null)
                    {
                        gridColumn.ItemRender = (IItemColumnRender)Activator.CreateInstance(att.ItemRenderType);
                    }
                    if (att.AlternatingItemRenderType != null)
                    {
                        gridColumn.AlternatingItemRender = (IItemColumnRender)Activator.CreateInstance(att.AlternatingItemRenderType);
                    }
                    yield return gridColumn;
                }

            }
            else
            {
                ////get from othe ModelMetadata type.

                //return null;
            }
        }
        protected virtual IEnumerable<GridCommandAttribute> GetGridCommandDefinitions(ModelMetadata modelMetadata)
        {
            if (modelMetadata is KoobooModelMetadata)
            {
                return ((KoobooModelMetadata)modelMetadata).Attributes.OfType<GridCommandAttribute>().OrderBy(it => it.Order).ToArray();
            }
            else
            {
                return new GridCommandAttribute[0];
            }
        }
    }
}
