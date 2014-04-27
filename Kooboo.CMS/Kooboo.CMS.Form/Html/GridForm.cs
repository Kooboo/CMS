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

using Kooboo.Extensions;
using Kooboo.CMS.Common;


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
    var parentUUID = ViewContext.RequestContext.GetRequestValue(""parentUUID"") ?? """"; 
    var childFolders = Model.ChildFolders==null? new TextFolder[0]:Model.ChildFolders.ToArray();
    var showBrodcastingColumns = Repository.Current.EnableBroadcasting && 
        Kooboo.CMS.Content.Services.ServiceFactory.ReceiveSettingManager.All(Repository.Current, null).Select(it=>it.AsActual()).Where(it => it.ReceivingFolder.EqualsOrNullEmpty(folder.FullName, StringComparison.OrdinalIgnoreCase)).Count() != 0;
}}
@helper RenderHeader(TextFolder folder,Schema schema,bool showBrodcastingColumns){{
        <thead>
            <tr>
                <th class=""checkbox mutiple"">
                    <div>
                        <input type=""checkbox"" class="" select-all"" />
                        @Html.IconImage(""arrow"")
                        <ul class=""hide"">
                            <li>Select:</li>
                            <li class=""all""><a href=""javascript:;"">All Elements</a></li>
                            <li class=""docs""><a href=""javascript:;"">Only Documents</a></li>
                            @if (ViewBag.FolderPermission)
                            {{
                                <li class=""folders""><a href=""javascript:;"">Only Folders</a></li>
                            }}
                            <li class=""none""><a href=""javascript:;"">None</a></li>
                        </ul>
                    </div>
                </th>
                {0}
                @if (showBrodcastingColumns)
                {{
                    <th class=""IsLocalized @SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, ""IsLocalized"", -1)"">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, ""Is localized"", ""IsLocalized"", -1)</th>
                    <th class=""OriginalRepository @SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, ""OriginalRepository"", -1)"">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, ""Original repository"", ""OriginalRepository"", -1)</th>
                    <th class=""@SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, ""OriginalUpdateTimes"", -1)"">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, ""Original update times"", ""OriginalUpdateTimes"", -1)</th>
                }}
                @if (folder.EmbeddedFolders != null)
                {{
                    foreach (var s in folder.EmbeddedFolders)
                    {{
                    <th>@Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s)).FriendlyText
                    </th>
                    }}
                }}
                @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                {{
                    <th class=""action"">
                        @(""Workflow"".Localize())
                    </th>
                }}
                               
                @if (schema.IsTreeStyle)
                {{
                    <th class=""action"">
                    </th> 
                }}
            </tr>
        </thead>
}}

<div class=""common-table fixed"">
 <div class=""thead"">
    <table>
        @RenderHeader(folder,schema,showBrodcastingColumns)
    </table>
</div>
<div class=""tbody"">
    <table>
       @RenderHeader(folder,schema,showBrodcastingColumns)
       <tbody>
        @if (childFolders.Length == 0 && ViewBag.PagedList.TotalItemCount == 0)
        {{
            <tr class=""empty"">
                <td colspan=""100"">
                    @(""Empty"".Localize())
                </td>
            </tr>
        }}
        else{{
            foreach (dynamic item in childFolders)
                {{
                    <tr class=""foldertr @((item.Hidden == true)? ""hidden-folder"":"""")"">
                        <td class=""checkbox mutiple undraggable"">
                            @if (ViewBag.FolderPermission)
                            {{
                                <input type=""checkbox"" name=""select"" class=""select folder"" id=""@item.FullName"" value=""@item.FullName"" data-id-property=""UUID"" />
                            }}
                        </td>
                        <td>
                            @if (!string.IsNullOrEmpty(item.SchemaName))
                            {{
                                <a href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                            else
                            {{
                                <a href=""@this.Url.Action(""Index"", ViewContext.RequestContext.AllRouteValues().Merge(""controller"", ""TextFolder"").Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                        </td>
                        <td colspan=""{2}"">
                        </td>
                         @if(showBrodcastingColumns)
                        {{
                            <td colspan=""3"">
                            </td>
                        }}
                        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
                        {{
                            <td colspan=""1"">
                            </td>
                        }}
                        @if (folder.EmbeddedFolders != null)
                        {{
                            <td colspan=""@folder.EmbeddedFolders.Count()"">
                            </td>
                        }}
                      
                        @if (schema.IsTreeStyle)
                        {{
                            <td>
                            </td>
                        }}
                    </tr>
                }}
            @AddTreeItems(ViewBag.PagedList, schema, allowedEdit, folder, """",showBrodcastingColumns)
        }}
            
        </tbody>
    </table>
</div>
</div>
@helper AddTreeItems(IEnumerable<TextContent> items, Schema schema, bool allowedEdit, TextFolder folder, string parentChain,bool showBrodcastingColumns)
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
            var workflowItem = item._WorkflowItem_;
            var hasworkflowItem = workflowItem != null;
            var availableEdit = hasworkflowItem || (!hasworkflowItem && allowedEdit);
    <tr id=""@item.UUID"" data-parent-chain='' class= ""doctr  @((item.IsLocalized != null && item.IsLocalized == false) ? ""unlocalized"" : ""localized"") @((item.Published == null || item.Published == false) ? ""unpublished"" : ""published"") @(hasworkflowItem ? ""hasWorkflowItem"" : """")"" >
        <td class=""checkbox mutiple @(ViewBag.Draggable? ""draggable"":"""")"">
            <div>
            @if(ViewBag.Draggable){{
            @Html.IconImage(""drag"")
            }}
            @if (availableEdit)
            {{
                <input type=""checkbox"" name=""select"" class=""select doc"" value=""@item.UUID"" data-id-property=""UUID"" data-sequence=""@item.Sequence""/>
            }}
            </div>
        </td>
       {1}
        @if(showBrodcastingColumns)
        {{
           <td>@Kooboo.CMS.Form.Html.HtmlCodeHelper.RenderColumnValue(item.IsLocalized)</td>
           <td>@Kooboo.CMS.Form.Html.HtmlCodeHelper.RenderColumnValue(item.OriginalRepository)</td>
           <td>@if(item.OriginalUpdateTimes>0){{
                <a href='@Url.Action(""ShowOriginalVersions"",ViewContext.RequestContext.AllRouteValues().Merge(""UUID"",(string)(item.UUID)).Merge(""OriginalRepository"",(string)(item.OriginalRepository)).Merge(""OriginalFolder"",(string)(item.OriginalFolder)).Merge(""OriginalUUID"",(string)(item.OriginalUUID)).Merge(""startVersionId"",(int)(item.OriginalLastestVisitedVersionId)).Merge(""return"", ViewContext.HttpContext.Request.RawUrl))'>
                @Kooboo.CMS.Form.Html.HtmlCodeHelper.RenderColumnValue(item.OriginalUpdateTimes)
                </a>
               }}else{{ @(""-"")}}</td>
        }}
        @if (folder.EmbeddedFolders != null)
        {{
            foreach (var s in folder.EmbeddedFolders)
            {{
                var embeddedFolder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository.Current, s));
            <td class=""action"">
                @Html.ActionLink(embeddedFolder.FriendlyText + "" ("" + ((TextContent)item).Children(s).Count() + "")"", ""SubContent"", new {{ SiteName = ViewContext.RequestContext.GetRequestValue(""SiteName""), RepositoryName = ViewContext.RequestContext.GetRequestValue(""RepositoryName""), ParentFolder = ViewContext.RequestContext.GetRequestValue(""FolderName""), FolderName = s, parentUUID = (object)(item.UUID),@return =ViewContext.HttpContext.Request.RawUrl }})
            </td>
            }}
        }}
        @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
        {{
            <td class=""action"">
                @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)).Merge(""return"",Request.RawUrl))"" class=""o-icon process-workflow"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""return"",Request.RawUrl))"" class=""o-icon workflow-history"">@(""Workflow history"".Localize())</a>                      
                }}
            </td>
        }}
        
       @if (schema.IsTreeStyle)
         {{<td class=""action"">
            <a href=""@this.Url.Action(""Create"", ViewContext.RequestContext.AllRouteValues().Merge(""parentFolder"",ViewContext.RequestContext.GetRequestValue(""FolderName"")).Merge(""ParentUUID"", (object)(item.UUID)).Merge(""return"",Request.RawUrl))"" >@Html.IconImage(""plus small"")</a>
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
        <tr data-bind='css:item._TRClass_,attr:{{id:item.UUID,""data-parent-chain"":item._ParentChain_}}'>
            <td class=""checkbox mutiple @(ViewBag.Draggable? ""draggable"":"""")"">
                <div>
                @if(ViewBag.Draggable){{
                @Html.IconImage(""drag"")
                }}
                <input type=""checkbox"" name=""select"" class=""select doc"" data-bind=""attr:{{id:item.UUID, value:item.UUID, 'data-sequence':item.Sequence}}"" data-id-property=""UUID"" />
                </div>
            </td>
            {4}
            <td class=""date"" data-bind=""html:item._LocalCreationDate_""></td>
            <td><span data-bind=""text : (item.Published == true?'@(""YES"".Localize())': '-')""></span></td>
            @if(showBrodcastingColumns)
            {{
               <td><span data-bind=""text : (item.IsLocalized == null || item.IsLocalized == true?'@(""YES"".Localize())': '-')""></span></td>
               <td><span data-bind=""text : ((item.OriginalRepository == null || item.OriginalRepository =='')? '-':item.OriginalRepository)""></span></td>
                <!-- ko if:item._ViewOriginalContentChangesURL_  -->                
               <td> <a data-bind=""text:item.OriginalUpdateTimes,attr:{{href:item._ViewOriginalContentChangesURL_}}""></a></td>
                <!-- /ko -->
                <!-- ko ifnot:item._ViewOriginalContentChangesURL_  -->                
               <td>-</td>
                <!-- /ko -->
            }}        
            <!-- ko foreach: {{data:_EmbeddedFolders_,as:'folder'}} -->
            <td >
                <a data-bind=""text:folder.Text,attr:{{href:folder.Link}}"" class=""dialog-link""></a>
            </td>
            <!-- /ko -->
            @if (Repository.Current.EnableWorkflow && folder.EnabledWorkflow)
            {{
                <td>
                    @* @if (hasworkflowItem)
                {{
                    <a href=""@Url.Action(""Process"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""RoleName"", (object)(workflowItem.RoleName)).Merge(""Name"", (object)(workflowItem.Name)).Merge(""return"",Request.RawUrl))"" class=""o-icon process-workflow"">@(""Process workflow"".Localize())</a>
                }}
                else
                {{
                    <a href=""@Url.Action(""WorkflowHistory"", ""PendingWorkflow"", ViewContext.RequestContext.AllRouteValues().Merge(""UserKey"", (object)(item.UserKey)).Merge(""UUID"", (object)(item.UUID)).Merge(""return"",Request.RawUrl))"" class=""o-icon workflow-history"">@(""Workflow history"".Localize())</a>
                }}*@
                </td>
            }}           
            @if (schema.IsTreeStyle)
            {{
                <td class=""action"">
                    <a data-bind=""attr:{{href:item._CreateUrl_}}"">@Html.IconImage(""plus small"")</a>
                </td>
            }}
        </tr>
    </tbody>
</table>
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
                            @(string.Join("","", {0}_values.Select(it => it.GetSummary())))
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
                            @(string.Join("","", {0}_values.Select(it => it.Text)))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                    }
                    sb_head.AppendFormat("\t\t<th class=\"{1} @SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, \"{1}\",-1)\">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, \"{0}\", \"{1}\", -1)</th>\r\n", string.IsNullOrEmpty(item.Label) ? item.Name : item.Label, item.Name, colspan);
                    if (item.Name.EqualsOrNullEmpty("Published", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td>{0}</td>", columnValue);
                    }
                    else if (item.Name.EqualsOrNullEmpty("UtcCreationDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">{0}</td>\r\n", columnValue);
                    }
                    else if (item.DataType == DataType.DateTime)
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">{0}</td>\r\n", columnValue);
                    }
                    else
                    {
                        if (colspan == 0)
                        {
                            sb_body.AppendFormat("\t\t<td>@if(Model.ShowTreeStyle){{\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>}}\n<a href=\"@this.Url.Action(\"Edit\",\"TextContent\",ViewContext.RequestContext.AllRouteValues().Merge(\"UserKey\", (object)(item.UserKey)).Merge(\"UUID\",(object)(item.UUID)).Merge(\"return\",Request.RawUrl))\">@Html.IconImage(\"file document\"){1}</a></td>\r\n"
                                , schema.Name, columnValue);

                            sb_koTml.AppendFormat("\t\t<td class=\"treeStyle\">\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>\n\t\t<a data-bind=\"attr:{{href:item._EditUrl_}}\">@Html.IconImage(\"file document\")<!--ko text: item.{0}--><!--/ko--></a></td>"
                                , item.Name);
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
            if (control == null)
            {
                return false;
            }
            return control.HasDataSource;
        }
        #endregion
    }
}