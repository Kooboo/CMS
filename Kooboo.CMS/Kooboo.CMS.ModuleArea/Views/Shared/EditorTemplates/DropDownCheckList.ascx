<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%
    var fullPropName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString(); %>
<tr>
    <th>
        <label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
    </th>
    <td>
        <%: Html.DropDownList(propertyName, ViewData.ModelMetadata.GetDataSource()
        .GetSelectListItems(ViewContext.RequestContext).SetActiveItem(Model),
        Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("class", ViewData["class"]).Merge("multiple", "multiple" ))%>
        <%             
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%>
    </td>
</tr>
<script type="text/javascript">
    $(function () {
        $("#<%: fullPropName%>").dropdownchecklist({ width: 200, height: "auto" });
        //$(".ui-dropdownchecklist-item").addClass("clearfix").removeClass("ui-state-default");
        var dropdownWidth = $(".ui-dropdownchecklist-text").width();
        $(".ui-dropdownchecklist-text").width(dropdownWidth - 15);
    });
</script>
