<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<bool?>" %>
<% 
    string prefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
    var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
    var guid = Guid.NewGuid();
    
%>
<tr id="tr-<%:guid %>">
    <th>
        <label class="label-checkbox" for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: "Keep content status".Localize()%></label>
            <a href="javascript:;" class="tooltip-link" title="<%: "Keep original content publishing status or change into unpublished and decide manually".Localize() %>">
        </a>
        <%
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
    </th>
    <td>
        <p class="clearfix">
            <input type="radio" id="radio-<%:guid %>-1" value="True" name="<%:propertyName %>" checked="checked" />
            <label for="radio-<%:guid %>-1">
                <%:"Keep content status".Localize() %>
            </label>
        </p>
        <p class="clearfix">
            <input type="radio" id="radio-<%:guid %>-2" value="False" name="<%:propertyName %>" />
            <label for="radio-<%:guid %>-2">
                <%:"Decide myself".Localize() %>
            </label>
        </p>       
        <p class="clearfix">
            <%: Html.ValidationMessage(ViewData.ModelMetadata, new { name = ViewData["name"] })%></p>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $(function () {
        var tr = $('#tr-<%:guid %>'),
        modelVal = '<%:ViewData.ModelMetadata.Model %>';
        tr.find('input:radio').filter('[value=' + modelVal + ']').attr('checked', 'checked');
    });
</script>
