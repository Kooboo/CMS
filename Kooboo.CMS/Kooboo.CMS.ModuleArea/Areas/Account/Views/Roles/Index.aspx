<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Site.Master"
	Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Account.Models.Role>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
<%: "Roles".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h3 class="title">
		<%: "Roles".Localize() %></h3>
	<div class="command clearfix">
		<%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link" , title="Create role".Localize()}))%>
<%:Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button delete deleteCommand", errMsg = "Please select schemas!".Localize(), confirmMsg = "Are you sure you want to delete these items?".Localize() }))%>
	</div>
	<%: Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
