<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<%
	var fullPropName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
	ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<%if (ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).Count() > 0)
  {%>
<%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
<div class="task-block plugins">
	<h3 class="title">
		<span><%="Plugins".Localize() %></span><span class="arrow"></span></h3>
	<div class="content">
		<p class="buttons clearfix">
			<%: Html.DropDownList(ViewData.ModelMetadata.PropertyName, ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).SetActiveItem(Model), new { multiple = "multiple" })%>
			<a class="button add" href="javascript:;" title="<%="Select Plugin".Localize() %>">
				<%="Add".Localize()%></a>
		</p>
		<ul class="plugins list">
		</ul>
	</div>
</div>
<% } %>
