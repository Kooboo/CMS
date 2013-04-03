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
@if(Model==null)
{{
    @(""The content was deleted."".Localize())
}}
else
{{
    var schema = (Kooboo.CMS.Content.Models.Schema)ViewData[""Schema""];
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    var workflowItem  = Model._WorkflowItem_;
    var hasWorkflowItem = workflowItem!=null;
    var availableEdit = hasWorkflowItem || (!hasWorkflowItem && allowedEdit);

    using(Html.BeginForm(ViewContext.RequestContext.AllRouteValues()[""action""].ToString()
            , ViewContext.RequestContext.AllRouteValues()[""controller""].ToString()
            , ViewContext.RequestContext.AllRouteValues()
            , FormMethod.Post, new RouteValueDictionary(new {{ enctype = ""{0}"" }})))
{{
    <div class=""common-form"">
    <fieldset>
    <table>",
                    FormHelper.Enctype(schema)));


            foreach (var item in schema.Columns.OrderBy(it => it.Order))
            {
                sb.Append(item.Render(schema, true));
            }

            sb.Append(@"
    @Html.Action(""Categories"", ViewContext.RequestContext.AllRouteValues())
    </table>
    </fieldset>
     <p class=""buttons"">
        @if(availableEdit){
           <button type=""submit"">@(""Save"".Localize())</button>
            if(Model.IsLocalized !=null && Model.IsLocalized == false){<button type=""submit"" name=""Localize"" value=""true"">Localize</button>}            
            <a href=""javascript:;"" class=""dialog-close button"">@(""Close"".Localize())</a>            
        }
        else
        {
            <a href=""javascript:;"" class=""dialog-close button"">@(""Close"".Localize())</a>
            <a href=""@Url.Action(""WorkflowHistory"",""PendingWorkflow"",ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(Model.UserKey)).Merge(""UUID"",(object)(Model.UUID)))"" title=""@(""View workflow history"".Localize())"" class=""button  dialog-link"">@(""View workflow history"".Localize())</a>
            <a href=""javascript:;"" class=""tooltip-link"" title=""@(""The content is approving or you have not permission to publish."".Localize())""></a>
        }
       
     </p>
   </div>   
  }
}");
            return sb.ToString();
        }
        #endregion
    }
}
