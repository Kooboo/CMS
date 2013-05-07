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
using System.Web;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.ComponentModel;


namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class DataRuleFormModel
    {
        public IEnumerable<TextFolder> FolderList { get; set; }
        public IEnumerable<TextContent> ContentList { get; set; }
        public TextFolder Current { get; set; }
        public DataRuleSetting DataRuleSetting { get; set; }
    }

    public class DataRuleSettingRouteGetter : IGridActionRouteValuesGetter
    {
        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, ViewContext viewContext)
        {
            routeValueDictionary["from"] = viewContext.HttpContext.Request["from"];
            routeValueDictionary["fromName"] = viewContext.HttpContext.Request["fromName"];
            return routeValueDictionary;
        }
    }
    [MetadataFor(typeof(DataRuleSetting))]
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name=DataName", RouteValuesGetter = typeof(DataRuleSettingRouteGetter))]
    [GridAction(ActionName = "Delete", RouteValueProperties = "Name=DataName", RouteValuesGetter = typeof(DataRuleSettingRouteGetter), ConfirmMessage = "Are you sure you want to delete this item?")]
    public class DataRuleSetting_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 1)]
        public string DataName { get; set; }
        [UIHint("DataRuleBase")]
        public IDataRule DataRule { get; set; }
        [GridColumn(Order = 5)]
        [UIHint("DropDownList")]
        [EnumDataType(typeof(TakeOperation))]
        public TakeOperation TakeOperation { get; set; }
    }
    [MetadataFor(typeof(DataRuleBase))]
    public class DataRuleBase_Metadata
    {
        public IEnumerable<WhereClause> WhereClauses { get; set; }
        public string SortField { get; set; }
        [UIHint("DropDownList")]
        [EnumDataType(typeof(SortDirection))]
        public SortDirection SortDirection { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string Top { get; set; }
    }
}