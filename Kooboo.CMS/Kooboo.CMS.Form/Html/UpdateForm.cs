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


namespace Kooboo.CMS.Form.Html
{
    public class UpdateForm : ISchemaForm
    {
        #region ISchemaForm Members

        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"
@using Kooboo.CMS.Content.Models;
@using Kooboo.CMS.Content.Query;
@{{
    var schema = (Kooboo.CMS.Content.Models.Schema)ViewData[""Schema""];
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    var workflowItem  = Model._WorkflowItem_;
    var hasWorkflowItem = workflowItem!=null;
    var availableEdit = hasWorkflowItem || (!hasWorkflowItem && allowedEdit);
}}
    @using(Html.BeginForm(ViewContext.RequestContext.AllRouteValues()[""action""].ToString()
            , ViewContext.RequestContext.AllRouteValues()[""controller""].ToString()
            , ViewContext.RequestContext.AllRouteValues()
            , FormMethod.Post, new RouteValueDictionary(new {{ enctype = ""{0}"" }})))
{{
    <table>",
                    FormHelper.Enctype(schema)));


            foreach (var item in schema.Columns.OrderBy(it => it.Order))
            {
                sb.Append(item.Render(schema, true));
            }

            sb.Append(@"
    @Html.Action(""Categories"", ViewContext.RequestContext.AllRouteValues())
    </table>   
}");
            return sb.ToString();
        }
        #endregion
    }
}
