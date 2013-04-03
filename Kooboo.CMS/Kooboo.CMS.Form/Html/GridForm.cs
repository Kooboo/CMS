using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.Extensions;


namespace Kooboo.CMS.Form.Html
{
    public class GridForm : ISchemaForm
    {
        string Table = @"
@model Kooboo.CMS.Web.Areas.Contents.Models.TextContentGrid
@using Kooboo.CMS.Content.Query
@using Kooboo.CMS.Content.Models
@using Kooboo.CMS.Web.Areas.Contents.Controllers
@{{
    var schema = (Kooboo.CMS.Content.Models.Schema)ViewData[""Schema""];
    var folder = (Kooboo.CMS.Content.Models.TextFolder)ViewData[""Folder""];
    var routes = ViewContext.RequestContext.AllRouteValues();    
    var allowedEdit = (bool)ViewData[""AllowedEdit""];
    var allowedView = (bool)ViewData[""AllowedView""];
    this.ViewBag.IsEven = true;
    var parentUUID = ViewContext.RequestContext.GetRequestValue(""parentUUID"") ?? """";
    var pagedList = Model.ContentQuery.ToPageList(Model.PageIndex, Model.PageSize);
    //used in Content_Order
    ViewData[""GridModel""] = Model;    

    var folderPermission = Kooboo.CMS.Web.Authorizations.AuthorizationHelpers.Authorize(ViewContext.RequestContext, Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission);
    
}}
<h3 class=""title"">
   @folder.FriendlyText
</h3>
<div class=""command clearfix"">   
        <div class=""dropdown-button"">
            <span>@(""Create New"".Localize())</span>
            <div class=""hide"">
                <ul>
                    <li>@Html.ActionLink(folder.FriendlyText.Singularize(), ""Create"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""doc dialog-link"", title = ""Create "".Localize() + schema.Name }}))</li>
                    @if (folderPermission  && string.IsNullOrEmpty(parentUUID))
                    {{
                        <li>@Html.ActionLink(""Folder"".Localize(), ""Create"", ""TextFolder"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""folder dialog-link"", title = ""Create Folder"".Localize() }}))</li>
                    }}
                </ul>
            </div>
        </div>
    <div class=""dropdown-button"">
        <span>
            @(""Batch actions"".Localize())</span>
        <div class=""hide"">
            <ul>
                <li>
                    @Html.ActionLink(""Delete"".Localize(), ""Delete"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""batch delete commandWithoutStyle"", confirmMsg = ""Are you sure you want to remove these items?"".Localize(), alertMsg = ""Please select items?"".Localize() }}))</li>                
                    <li>
                        @Html.ActionLink(""Publish"".Localize(), ""BatchPublish"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""batch commandWithoutStyle"" }}))</li>
                    <li>
                        @Html.ActionLink(""Unpublish"".Localize(), ""BatchUnpublish"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""batch commandWithoutStyle"" }}))</li>
                    <li>
                        @Html.ActionLink(""Copy"".Localize(), ""Copy"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""batch commandWithoutStyle"" }}))</li>
                    <li>
                        @Html.ActionLink(""Re-broadcast"".Localize(), ""Rebroadcast"", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new {{ @class = ""batch commandWithoutStyle"" }}))</li>
            </ul>
        </div>
    </div>
    @if (!string.IsNullOrEmpty(routes[""FolderName""] as string) && folderPermission)
    {{
        <text>@Html.ActionLink(""Folder setting"".Localize(), ""Edit"", ""TextFolder"", ViewContext.RequestContext.AllRouteValues().Merge(""FullName"", routes[""FolderName""]).Merge(""FolderName"", null).Merge(""Folder"", null), new RouteValueDictionary(new {{ @class = ""button dialog-link"", title = ""Folder setting"".Localize() }}))</text>
    }}
    @Html.Partial(""Search"")
</div>
<div class=""table-container"">
    <table>
        <thead>
            <tr>
                <th class=""optional-selector checkbox draggable"">
                    <div>
                        <input type=""checkbox"" class="" select-all"" />
                        <ul class=""hide"">
                            <li>Select:</li>
                            <li class=""all""><a href=""javascript:;"">All Elements</a></li>
                            <li class=""docs""><a href=""javascript:;"">Only Documents</a></li>
                            @if (folderPermission)
                            {{
                                <li class=""folders""><a href=""javascript:;"">Only Folders</a></li>
                            }}
                            <li class=""none""><a href=""javascript:;"">None</a></li>
                        </ul>
                    </div>
                </th>
                {0}
                @if (folder.EmbeddedFolders != null)
                {{
                    foreach (var s in folder.EmbeddedFolders)
                    {{
                    <th class=""action"">@Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s)).FriendlyText
                    </th>
                    }}
                }}
                @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                {{
                    <th class=""action"">
                        @(""Workflow"".Localize())
                    </th>
                }}
                @if (Repository.Current.AsActual().EnableVersioning.Value == true)
                {{
                    <th class=""action"">
                        @(""Versions"".Localize())
                    </th>
                }}
                <th class=""action"">
                    @(""Edit"".Localize())
                </th>
                @if (schema.IsTreeStyle)
                {{
                    <th class=""action"">
                    </th> 
                }}
            </tr>
        </thead>
        @if (Model.ChildFolders != null)
        {{
            <tbody>
                @foreach (dynamic item in Model.ChildFolders)
                {{
                    <tr class=""folderTr"">
                        <td class=""undraggable"">
                            @if (folderPermission)
                            {{
                                <input type=""checkbox"" name=""Selected"" class=""select folders"" id=""@item.FullName"" value=""@item.FullName"" />
                            }}
                        </td>
                        <td>
                            @if (!string.IsNullOrEmpty(item.SchemaName))
                            {{
                                <a class=""f-icon folder"" href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                    @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                            else
                            {{
                                <a class=""f-icon folder"" href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""controller"", ""TextFolder"").Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                    @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                        </td>
                        <td colspan=""{2}"">
                        </td>
                        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                        {{
                            <td colspan=""1"">
                            </td>
                        }}
                        @if (Repository.Current.AsActual().EnableVersioning.Value == true)
                        {{
                            <td colspan=""1"">
                            </td>
                        }}
                        @if (folder.EmbeddedFolders != null)
                        {{
                            <td colspan=""@folder.EmbeddedFolders.Count()"">
                            </td>
                        }}
                        <td class=""action"">
                            @if (folderPermission)
                            {{
                                <a class=""o-icon edit dialog-link"" href=""@this.Url.Action(""Edit"", ""TextFolder"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                    @(""Edit"".Localize())</a>
                            }}
                        </td>
                        @if (schema.IsTreeStyle)
                        {{
                            <td class=""action"">
                            </td>
                        }}
                    </tr>
                }}
            </tbody>
        }}
        <tbody>
            @AddTreeItems(pagedList, schema, allowedEdit, folder, """")
        </tbody>
    </table>
    <div class=""pagination"">
        @Html.Pager(pagedList)
    </div>
</div>
<script language=""javascript"" type=""text/javascript"">
kooboo.cms.content.textcontent.initGrid('@(""Are you sure you want to delete these items?"".Localize())', '@(""You have not select any item!"".Localize())');
</script>
@helper AddTreeItems(IEnumerable<TextContent> items, Schema schema, bool allowedEdit, TextFolder folder, string parentChain)
    {{
        var isRoot = string.IsNullOrEmpty(parentChain);
        //column datasource
        {3}
        if (Repository.Current.EnableWorkflow == true)
        {{
            items = Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.GetPendWorkflowItemForContents(Repository.Current, items.ToArray(), User.Identity.Name);
        }}
        foreach (dynamic item in items)
        {{
            this.ViewBag.IsEven = (!this.ViewBag.IsEven);
            var workflowItem = item._WorkflowItem_;
            var hasworkflowItem = workflowItem != null;
            var availableEdit = hasworkflowItem || (!hasworkflowItem && allowedEdit);            
    <tr pid=""@item.ParentUUID"" id=""@item.UUID"" class= ""@(schema.IsTreeStyle? ""parent collapsed"":"""") @(this.ViewBag.IsEven ? ""even "" : """") @((item.IsLocalized != null && item.IsLocalized == false) ? ""unlocalized"" : """") @(hasworkflowItem ? ""hasWorkflowItem"" : """")"" >
        <td class=""draggable"">         
            @if (availableEdit)
            {{
                <input type=""checkbox"" name=""Selected"" class=""select docs"" id=""@item.UUID"" value=""@item.UUID"" />
            }}
        </td>
       {1}
        @if (folder.EmbeddedFolders != null)
        {{
            foreach (var s in folder.EmbeddedFolders)
            {{
                var embeddedFolder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s));
            <td class=""action"">
                @Html.ActionLink(embeddedFolder.FriendlyText, ""SubContent"", ""TextContent"", new {{ SiteName = ViewContext.RequestContext.GetRequestValue(""SiteName""), RepositoryName = ViewContext.RequestContext.GetRequestValue(""RepositoryName""), ParentFolder = ViewContext.RequestContext.GetRequestValue(""FolderName""), Folder = s, FolderName = s, parentUUID = (object)(item.UUID) }}, new {{ @class = ""dialog-link"", title = embeddedFolder.FriendlyText }})
            </td>
            }}
        }}
        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
        {{
            <td class=""action"">
                @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)))"" title=""@(""Process workflow"".Localize())"" class=""o-icon process dialog-link"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)))"" title=""@(""View workflow history"".Localize())"" class=""o-icon workflow dialog-link"">@(""View workflow history"".Localize())</a>                      
                }}
            </td>
        }}
        @if (Repository.Current.AsActual().EnableVersioning.Value == true)
        {{
            <td class=""action "">
                <a class=""o-icon version dialog-link"" title=""@(""Versions"".Localize())"" href=""@this.Url.Action(""Versions"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)))"" >@(""Version"").Localize())</a>
            </td>
        }}
        <td class=""action"">
            <input type=""hidden"" name=""Sequence"" value=""@item.Sequence""/>
            <a class=""o-icon edit dialog-link"" href=""@this.Url.Action(""Edit"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)))"" >@(""Edit"".Localize())</a>
        </td>@if (schema.IsTreeStyle)
         {{<td class=""action"">
            <a class=""o-icon add dialog-link""  href=""@this.Url.Action(""Create"", ViewContext.RequestContext.AllRouteValues().Merge(""ParentUUID"", (object)(item.UUID)))"" >@(""Create"".Localize())</a>
        </td>}}
    </tr>
        if (Model.ShowTreeStyle)
        {{
    var nextParentChain  = parentChain + @item.UUID + ""="";   
        }}
        }}
}}

<table id=""treeNode-template"" style=""display: none"" data-model=""JsonModel"">
    <tbody data-bind=""foreach:{{data:Model,as:'item'}}"">
        <tr class=""parent collapsed"" data-bind=""attr:{{id:item.UUID,parentChain:item._ParentChain_}}"">
            <td class=""draggable"">
                <input type=""checkbox"" name=""Selected"" class=""select docs"" data-bind=""attr:{{id:item.UUID, value:item.UUID}}"" />
            </td>
            {4}
            <td class=""date"" data-bind=""html:item._LocalCreationDate_""></td>
            <td class=""action"">
            @if(allowedEdit)
            {{
                <a data-bind=""attr: {{href:item._PublishUrl_, 'class': item.Published == true?'boolean-ajax-link o-icon tick': 'boolean-ajax-link o-icon cross' }}""></a>
            }}
            else
            {{
                <span data-bind=""attr: {{ 'class': item.Published == true?'o-icon tick': 'o-icon cross' }}""></span>
            }}
            </td>
            <!-- ko foreach: {{data:_EmbeddedFolders_,as:'folder'}} -->
            <td class=""action"">

                <a data-bind=""text:folder.Text,attr:{{href:folder.Link}}"" class=""dialog-link""></a>

            </td>
            <!-- /ko -->
            @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
            {{
                <td class=""action"">
                    @* @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)))"" title=""@(""Process workflow"".Localize())"" class=""o-icon process dialog-link"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)))"" title=""@(""View workflow history"".Localize())"" class=""o-icon workflow dialog-link"">@(""View workflow history"".Localize())</a>                      
                }}*@
                </td>
            }}
            @if (Repository.Current.AsActual().EnableVersioning.Value == true)
            {{
                <td class=""action "">
                    <a class=""o-icon version dialog-link"" title=""@(""Versions"".Localize())"" data-bind=""attr:{{href:item._VersionUrl_}}"" >@(""Version"").Localize())</a>
                </td>
            }}
            <td class=""action"">
                <input type=""hidden"" name=""Sequence"" data-bind=""value:item.Sequence"" />
                <a class=""o-icon edit dialog-link"" data-bind=""attr:{{href:item._EditUrl_}}"">@(""Edit"".Localize())</a>
            </td>
            @if (schema.IsTreeStyle)
            {{
                <td class=""action"">
                    <a class=""o-icon add dialog-link"" title=""Create a sub item"" data-bind=""attr:{{href:item._CreateUrl_}}"">@(""Create"".Localize())</a>
                </td>
            }}
        </tr>
    </tbody>
</table>
@Html.Partial(""_TreeDataScripts"","""")
";

        #region ISchemaTemplate Members

        public string Generate(ISchema schema)
        {
            StringBuilder sb_head = new StringBuilder();

            StringBuilder sb_body = new StringBuilder();

            StringBuilder columnDataSource = new StringBuilder();
            StringBuilder sb_koTml = new StringBuilder();

            int colspan = 0;
            foreach (var item in schema.Columns.OrderBy(it => it.Order))
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
                    sb_head.AppendFormat("\t\t<th class=\"{1}\">{0}</th>\r\n", string.IsNullOrEmpty(item.Label) ? item.Name : item.Label, colspan > 0 ? "common" : "");
                    if (item.Name.EqualsOrNullEmpty("Published", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"action\">@if(allowedEdit){{<a href=\"@Url.Action(\"Publish\",\"TextContent\",ViewContext.RequestContext.AllRouteValues().Merge(\"UserKey\", (object)(item.UserKey)).Merge(\"UUID\",(object)(item.UUID)))\" class=\"boolean-ajax-link o-icon @((item.Published!=null && item.Published == true)?\"tick\":\"cross\")\" ></a>}} else {{<span class='o-icon @((item.Published!=null && item.Published == true)?\"tick\":\"cross\")'></span>}}</td>");
                    }
                    else if (item.Name.EqualsOrNullEmpty("UtcCreationDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(DateTime.Parse(item[\"{0}\"].ToString()).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else if (item.DataType == Data.DataType.DateTime)
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(item[\"{0}\"] == null?\"\":((DateTime)item[\"{0}\"]).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    //else if (IsImage(item.ControlType))
                    //{
                    //    sb_body.AppendFormat("\t\t<td><img src='@Url.Content(item.{0} ?? \"\")' alt='' height='80' width='120'/></td>\r\n", item.Name);
                    //}
                    else
                    {
                        if (colspan == 0)
                        {
                            sb_body.AppendFormat("\t\t<td>\n\t\t<span class=\"expander\" style=\"padding-left: 19px; margin-left: -19px;\"></span>\n<a class=\"f-icon document dialog-link \" href=\"@this.Url.Action(\"Edit\",\"TextContent\",ViewContext.RequestContext.AllRouteValues().Merge(\"UserKey\", (object)(item.UserKey)).Merge(\"UUID\",(object)(item.UUID)))\" >{1}</a></td>\r\n", schema.Name, columnValue);

                            sb_koTml.AppendFormat("\t\t<td class=\"treeStyle\">\n\t\t<span class=\"expander\" style=\"padding-left: 19px; margin-left: -19px;\"></span>\n\t\t<a class=\"f-icon document dialog-link \" data-bind=\"text:item.{0},attr:{{href:item._EditUrl_}}\"></a></td>", item.Name);
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
        #endregion
    }
}