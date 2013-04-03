<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<Kooboo.CMS.Form.SelectListItem>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
	var guid = Guid.NewGuid();
	var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix;
	var ulId = fullPropertyName.Replace(".", "_");
    var isFromFolder = ViewBag.isFromFolder;

	var preProperty = ViewData["id"] + "SelectionItems";

	ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<fieldset class="flex-list manual <% if(isFromFolder == true){ %> hide <%} %>">
	<table>
		<thead>
			<tr>
				<th>
					<label class="short">
						<%:"Text".Localize() %></label>
					<label class="short">
						<%:"Value".Localize() %>
					</label>
				</th>
				<th>
				</th>
			</tr>
		</thead>
		<tbody id="selectItemContainer<%:guid %>">
			<tr id="selectItemTemplate<%:guid %>">
				<td>
					<input type="text" name="Text" class="short" />
					<input type="text" name="Value" class="short" />
					<a href="javascript:;" class="o-icon remove form-action" title="Remove">Remove</a>
				</td>
				<td>
				</td>
			</tr>
		</tbody>
	</table>
	<a href="javascript:;" class="o-icon add form-action" id="addSelectItemBtn<%:guid %>" title="Add">Add</a>
</fieldset>
<script language="javascript" type="text/javascript">
	$(document).ready(function () {
		kooboo.cms.ui.dynamicListInstance({
			containerId: 'selectItemContainer<%:guid %>', //the container that you put the new item in. 
			templateId:'selectItemTemplate<%:guid %>',
			propertyName: '<%:ViewData["id"]%>', //str 
			addButtonId: 'addSelectItemBtn<%:guid %>',
			data: <%=Model.ToJSON() %>,
			delClass: 'remove',
			parentSelector:'tr:eq(0)'
		});
	});
</script>
