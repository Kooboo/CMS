<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IDictionary>" %>
<%
    var guid = Guid.NewGuid();
    var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix;
    if (ViewBag.FullPropertyName!=null)
    {
        fullPropertyName = ViewBag.FullPropertyName.ToString();
    }
    var ulId = fullPropertyName.Replace(".", "_");
%>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<tr>
    <th>
        <label for="<%: ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString()%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        <ul id="container-<%:guid %>" class="key-value">
            <li id="template-<%:guid %>" title="keyVal">
                <input type="text" title='key' name="Key" class="textbox" />
                <input type="text" title='value' name="Value" class="textbox marginL10 long" />
                <a href="#" class="o-icon remove inline-action ">Remove</a> </li>
            <li title='title'>
                <label class="textbox">
                    Key</label><label class="textbox marginL10">Value</label></li>
        </ul>
        <div class="clearfix">
            <a href="#" id="add-<%:guid %>" class="o-icon add key-value-add">Add</a>
        </div>
    </td>
</tr>
<%  var jsonData = Model == null ? "[]" : ((Dictionary<string, string>)Model).ToList().ToJSON();%>
<script language="javascript" type="text/javascript">
	$(function () { 
	
	        kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>', //the container that you put the new item in. 
            templateId:'template-<%:guid %>',
            propertyName: '<%:fullPropertyName%>', //str 
            addButtonId: 'add-<%:guid %>', //add button
            data: <%=jsonData %>, //json data to fill the form field.
			delClass: 'remove'
        });
	
	});
</script>
