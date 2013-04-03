using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html
{
    public class CreateForm : ISchemaForm
    {
        #region ISchemaForm Members

        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"
@using Kooboo.CMS.Content.Models;
@using Kooboo.CMS.Content.Query;
@{{ var schema = (Kooboo.CMS.Content.Models.Schema)ViewData[""Schema""];
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    var parentUUID = ViewContext.RequestContext.AllRouteValues()[""parentUUID""];
    var parentFolder=ViewContext.RequestContext.AllRouteValues()[""Folder""];
}}
@using(Html.BeginForm(ViewContext.RequestContext.AllRouteValues()[""action""].ToString()
    , ViewContext.RequestContext.AllRouteValues()[""controller""].ToString()
    , ViewContext.RequestContext.AllRouteValues()
    , FormMethod.Post
    , new RouteValueDictionary(new {{ enctype = ""{0}"" }})))
{{
    <div class=""common-form"">
    <fieldset>
    <table>", FormHelper.Enctype(schema)));

            foreach (var item in schema.Columns.OrderBy(it => it.Order))
            {
                sb.Append(item.Render(schema, false));
            }

            sb.Append(@"
    @Html.Action(""Categories"", ViewContext.RequestContext.AllRouteValues())
    @if (schema.IsTreeStyle && parentUUID!=null)
    {
        <input type=""hidden"" name=""ParentUUID"" value=""@parentUUID""/> 
        <input type=""hidden"" name=""ParentFolder"" value=""@parentFolder"" />
    }
    </table>
    </fieldset>
    <p class=""buttons""><button type=""submit"">@(""Save"".Localize())</button> <a href=""javascript:;"" class=""dialog-close button"">@(""Close"".Localize())</a> </p>
    </div>
}");

            return sb.ToString();
        }
        #endregion
    }
}
