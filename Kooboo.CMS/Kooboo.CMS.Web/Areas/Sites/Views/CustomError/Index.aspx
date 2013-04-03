<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.CustomError>>" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
<%: "Custom errors".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h3 class="title">
		 <span><%:"Custom errors".Localize()%></span> <a href="javascript:;" title="<%:"Define your error redirect pages".Localize() %>"
			class="tooltip-link right"></a></h3>
	<div class="command clearfix">
		<%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button dialog-link",title="Create custom error".Localize() }))%>
		<%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button", name = "import" }))%>
		<%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
		<%: Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button" }))%>
		<%: Html.ActionLink("Delete".Localize(), "Delete",ViewContext.RequestContext.AllRouteValues(),  new RouteValueDictionary(new { @class="button deleteCommand" }))%>
	</div>
	<%: Html.GridForModel() %>
</asp:Content>
