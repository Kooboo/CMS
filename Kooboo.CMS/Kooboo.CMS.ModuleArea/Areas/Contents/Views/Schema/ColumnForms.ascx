<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Content.Models.Schema>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<div id="grid-field-template">
	<%: Html.GridFor(typeof(Kooboo.CMS.Content.Models.Column), Model.Columns.OrderBy(it=>it.Order))%>
</div>
<div id="form-fields" class="hide">
	<% foreach (var column in Model.Columns)
	{%>
	<div id="<%= column.Name %>-detail-form" style="" class="edit-column-form <%:column.Name %>">
		<%: Html.Partial("Column", column)%>
	</div>
	<%} %>
</div>
<script language="javascript" type="text/javascript">
	kooboo.data("schema-info",<%=Model.ToJSON() %>);
</script>
