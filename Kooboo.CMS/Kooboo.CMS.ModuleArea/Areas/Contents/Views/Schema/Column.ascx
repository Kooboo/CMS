<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Content.Models.Column>" %>
<%var guid = Guid.NewGuid();
  //Model.Name = Model.Name ?? guid.ToString();
  var preId = string.Format("Columns[{0}].", Model.Name ?? guid.ToString());
  var isFromFolder = Model.SelectionSource== SelectionSource.TextFolder;
%>
<div id="Column<%:Model.Name%>-form" class="Column<%:Model.Name%> ColumnForm  common-form">
    <input type="hidden" name="Columns.Index" value="<%: Model.Name??guid.ToString() %>" />
    <%using (Html.BeginForm("ColumnForms", "Schema", ViewContext.RequestContext.AllRouteValues()))
      { %>
    <div id="<%: Model.Name %>tabs" koobootab="koobooTab">
        <ul class="tabs clearfix" id="<%: Model.Name %>-templateTabs">
            <li><a href="#<%: Model.Name %>-tabBasicInfo">
                <%="Basic".Localize() %></a></li>
            <li><a href="#<%: Model.Name %>-tabMoreOption">
                <%="Advanced".Localize()%></a></li>
            <li><a href="#<%: Model.Name %>-tabValidation">
                <%="Validation".Localize()%></a></li>
            <li id="<%: preId %>selectItemListTab"><a href="#<%: Model.Name %>-tabSelectListItemsremove">
                <%="Selection value".Localize()%></a></li>
        </ul>
        <div class="tab-content" id="<%: Model.Name %>-tabBasicInfo">
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name, new { id=preId+"Name",name=preId+"Name", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Label, new { id = preId + "Label", name = preId + "Label", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.ControlType, new { id = preId + "ControlType", name = preId + "ControlType", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.DataType, new { id = preId + "DataType", name = preId + "DataType", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.DefaultValue, new { id = preId + "DefaultValue", name = preId + "DefaultValue", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Order, new { id = preId + "Order", name = preId + "Order", @class = "medium" })%>
                    <%:Html.EditorFor(m => m.Summarize, new { id = preId + "Summarize", name = preId + "Summarize" })%>
                    <%:Html.EditorFor(m => m.ShowInGrid, new { id = preId + "ShowInGrid", name = preId + "ShowInGrid" })%>
                </tbody>
            </table>
        </div>
        <div class="tab-content" id="<%: Model.Name %>-tabMoreOption">
            <fieldset>
                <table>
                    <tbody>
                        <%:Html.EditorFor(m => m.Tooltip, new { id = preId + "Tooltip", name = preId + "Tooltip", @class = "medium" })%>
                        <%:Html.EditorFor(m => m.Length, new { id = preId + "Length", name = preId + "Length", @class = "medium" })%>
                        <%:Html.EditorFor(m => m.AllowNull, new { id = preId + "AllowNull", name = preId + "AllowNull" })%>
                        <%:Html.EditorFor(m => m.Modifiable, new { id = preId + "Modifiable", name = preId + "Modifiable" })%>
                        <%:Html.EditorFor(m => m.CustomSettings, new { FullPropertyName = preId + "CustomSettings" })%>
                        <%--<%:Html.EditorFor(m => m.Indexable, new { id = preId + "Indexable", name = preId + "Indexable" })%>--%>
                    </tbody>
                </table>
            </fieldset>
        </div>
        <div class="tab-content" id="<%: Model.Name %>-tabSelectListItemsremove">
            <div class="clearfix">
                <p class="left">
                    <input type="radio" name="<%:preId %>SelectionSourceRadio" id="<%:preId %>-manual" <%if(!isFromFolder){%>
                        checked <%} %> value="0" />
                    <label for="<%:preId %>-manual" class="inline">
                        <%:"Manual".Localize() %></label>
                </p>
                <p>
                    <input type="radio" id="<%:preId %>-fromfolder" name="<%:preId %>SelectionSourceRadio" <%if(isFromFolder){%>
                        checked <%} %> value="1" />
                    <label for="<%:preId %>-fromfolder" class="inline">
                        <%:"From folder".Localize() %></label>
                </p>
                <input type="hidden" name="<%:preId  %>SelectionSource" value="<%: (int)Model.SelectionSource %>" />
            </div>
            <fieldset class="flex-list from-folder <%if(!isFromFolder){%> hide <%} %>">
                <%:Html.EditorFor(o => o.SelectionFolder, new { id = preId + "SelectionFolder", name = preId + "SelectionFolder", @class = "medium" })%>
            </fieldset>
            <div class="<%if(!isFromFolder){%> hide <%} %>">
            </div>
            <%:Html.EditorFor(m => m.SelectionItems, new { id = preId + "SelectionItems",isFromFolder = isFromFolder })%>
        </div>
        <div class="tab-content" id="<%: Model.Name %>-tabValidation">
            <%:Html.EditorFor(m => m.Validations,new { id = preId + "Validations" })%>
        </div>
    </div>
    <p class="buttons">
        <input type="submit" class="button submit" id="<%: Model.Name %>-save" value="<%="Save".Localize() %>" />
    </p>
    <%} %>
</div>
<script type="text/javascript" language="javascript">
    $(function () {

        kooboo.cms.content.schema.column.init('<%:Model.Name %>');
    });
</script>
