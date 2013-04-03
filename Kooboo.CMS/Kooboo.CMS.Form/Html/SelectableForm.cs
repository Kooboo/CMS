using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html
{
    public class SelectableForm : ISchemaForm
    {
        static string Table = @"
@model Kooboo.CMS.Web.Areas.Contents.Models.SelectableViewModel
@using Kooboo.CMS.Content.Query
@using Kooboo.CMS.Content.Models
@using Kooboo.CMS.Web.Areas.Contents.Controllers
@{{
    var schema = (Kooboo.CMS.Content.Models.Schema)ViewData[""Schema""];
    var folder = (Kooboo.CMS.Content.Models.TextFolder)ViewData[""Folder""];
    var routes = ViewContext.RequestContext.AllRouteValues();
    this.ViewBag.IsEven = true;
    var parentUUID = routes[""parentUUID""] ?? """";
    var pagedList = Model.Contents;
    //used in Content_Order
    this.ViewContext.Controller.ViewData[""ContentPagedList""] = pagedList;
    
    var parent = folder.Parent;

    var nameList = new List<string>() {{ folder.FriendlyText }};

    while (parent != null)
    {{
        parent = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(parent);
        if (parent != null)
        {{
            nameList.Add(parent.FriendlyText);
            parent = parent.Parent;
        }}
        
    }}

    nameList.Reverse();   
}}
<input type=""hidden"" name=""FriendlyFolderName"" value=""@string.Join(""/"", nameList)""/>
<div class=""command clearfix"">
    @Html.Partial(""Search"")
</div>
<form id=""selectedForm"" action="""">
<div class=""table-container clearfix"">
    <table class=""datasource"">
        <thead>
            <tr>
                <th class=""checkbox"">
                </th>
                {0}
            </tr>
        </thead>
 @if (Model.ChildFolders != null)
        {{
            <tbody>
                @foreach (dynamic item in Model.ChildFolders)
                {{
                    <tr class=""folderTr"">
                        <td>                            
                        </td>
                        <td>
                            @if (!string.IsNullOrEmpty(item.SchemaName))
                            {{
                                <a class=""f-icon folder"" href=""@this.Url.Action(""SelectCategories"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                    @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                            else
                            {{
                                <a class=""f-icon folder"" href=""@this.Url.Action(""SelectCategories"", ViewContext.RequestContext.AllRouteValues().Merge(""controller"", ""TextFolder"").Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                    @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                        </td>
                        <td colspan=""{2}"">
                        </td>                      
                    </tr>
                }}
            </tbody>
        }}
        <tbody>
           @AddTreeItems(pagedList, schema, folder, true)
        </tbody>
    </table>
    <div class=""pager"">
        @Html.Pager(pagedList)
    </div>
</div>
</form>
@helper AddTreeItems(IEnumerable<TextContent> items, Schema schema, TextFolder folder, bool isRoot)
    {{
        {3}
        if (Repository.Current.EnableWorkflow == true)
        {{
            items = Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.GetPendWorkflowItemForContents(Repository.Current, items.ToArray(), User.Identity.Name);
        }}
        if (items.Count() > 0)
        {{
            foreach (dynamic item in items)
            {{
            this.ViewBag.IsEven = (!this.ViewBag.IsEven);
    <tr id=""@item.UUID"" class=""@(schema.IsTreeStyle? ""parent collapsed"":"""") @(this.ViewBag.IsEven ? ""even "" : """")@((!isRoot && !string.IsNullOrEmpty(item.ParentUUID)) ? ""child-of-node-"" + item.ParentUUID : """")"">
        <td>
            @if (Model.SingleChoice)
            {{
                <input type=""radio"" value='@item[""UUID""]' name=""chkContent""/> 
            }}
            else
            {{
                <text>@Html.CheckBox(""chkContent"", false, new {{ value = item[""UUID""], @class = ""checkboxList"" }})</text> 
            }}
        </td>
        {1}
    </tr>     
            }}
        }}
}}

<table id=""treeNode-template"" style=""display: none"" data-model=""JsonModel"">
    <tbody data-bind=""foreach:{{data:Model,as:'item'}}"">  
           
      <tr class=""parent collapsed"" data-bind=""attr:{{id:item.UUID,parentChain:item._ParentChain_}}"">
        <td>
            @if (Model.SingleChoice)
            {{
                <input type=""radio"" name=""chkContent"" data-bind=""attr:{{value:item.UUID}}""/> 
            }}
            else
            {{                
               <input type=""checkbox"" name=""chkContent"" class = ""checkboxList"" data-bind=""attr:{{value:item.UUID}}""/>
            }}
        </td>
{4}
        <td class=""date"" data-bind=""html:item._LocalCreationDate_""></td>
        <td class=""action""><span data-bind=""attr: {{ 'class': item.Published == true?'o-icon tick': 'o-icon cross' }}""></span></td>
    </tr>     
    </tbody>
</table>
@Html.Partial(""_TreeDataScripts"",""readonly"")
";


        public string Generate(ISchema schema)
        {

            StringBuilder sb_head = new StringBuilder();
            StringBuilder sb_body = new StringBuilder();
            StringBuilder columnDataSource = new StringBuilder();
            StringBuilder sb_koTml = new StringBuilder();
            int colspan = 0;
            //StringBuilder sb_categoryData = new StringBuilder();
            foreach (var item in schema.Columns)
            {
                if (item.ShowInGrid)
                {
                    string columnValue = string.Format("@Kooboo.CMS.Form.Html.HtmlCodeHelper.RenderColumnValue(item.{0})", item.Name);
                    if (HasDataSource(item.ControlType))
                    {
                        if (!string.IsNullOrEmpty(item.SelectionFolder))
                        {
                            //                        sb_categoryData.AppendFormat(@"
                            //                          
                            //                         ", item.Name, item.SelectionFolder);
                            columnDataSource.AppendFormat(@"var {0}_data = (new TextFolder(Repository.Current,""{1}"")).CreateQuery().ToArray();", item.Name, item.SelectionFolder);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);                        
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.UUID, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @string.Join("","", {0}_values.Select(it => it.GetSummary()))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                        else if (item.SelectionItems != null && item.SelectionItems.Length > 0)
                        {
                            columnDataSource.AppendFormat(@"var {0}_data = schema[""{0}""].SelectionItems;", item.Name);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);
                      
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.Value, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @string.Join("","", {0}_values.Select(it => it.Text))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                    }
                    sb_head.AppendFormat("\t\t<th class=\"{2}\">{1}</th>\r\n", item.Name.ToLower(), string.IsNullOrEmpty(item.Label) ? item.Name : item.Label, "common");
                    if (item.Name.EqualsOrNullEmpty("Published", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"action\"><span class=\"o-icon @((item.Published!=null && item.Published == true)?\"tick\":\"cross\")\"></span></td>");
                    }
                    else if (item.Name.EqualsOrNullEmpty("UtcCreationDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(DateTime.Parse(item[\"{0}\"].ToString()).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else if (item.DataType == Data.DataType.DateTime)
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(item[\"{0}\"] == null?\"\":((DateTime)item[\"{0}\"]).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    //else if (ControlHelper.IsImage(item.ControlType))
                    //{
                    //    sb_body.AppendFormat("\t\t<td><img src='@Url.Content(item.{0} ?? \"\")' alt='' height='80' width='120'/></td>\r\n", item.Name);
                    //}
                    else
                    {
                        if (colspan == 0)
                        {
                            sb_body.AppendFormat("\t\t<td>\n\t\t<span class=\"expander\" style=\"padding-left: 19px; margin-left: -19px;\"></span>{0}</td>\r\n", columnValue);
                            sb_koTml.AppendFormat("\t\t<td class=\"treeStyle\">\n\t\t<span class=\"expander\" style=\"padding-left: 19px; margin-left: -19px;\"></span><span  data-bind=\"html:item.{0}\"></span></td>", item.Name);
                        }
                        else
                        {
                            sb_body.AppendFormat("\t\t<td>{0}</td>\r\n", columnValue);
                            sb_koTml.AppendFormat("\t\t<td data-bind=\"html:item.{0}\"></td>", item.Name);
                        }

                       
                    }
                    colspan++;
                }
            }
            return string.Format(Table, sb_head, sb_body, colspan - 1, columnDataSource, sb_koTml);
        }

        private bool HasDataSource(string controlType)
        {
            if (string.IsNullOrEmpty(controlType))
            {
                return false;
            }
            var control = ControlHelper.Resolve(controlType);
            return control.HasDataSource;
        }
    }
}
