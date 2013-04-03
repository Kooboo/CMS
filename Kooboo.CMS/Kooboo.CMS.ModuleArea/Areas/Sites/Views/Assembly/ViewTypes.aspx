<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Web.Areas.Sites.Models.PluginType_Metadata>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ViewTypes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<h3 class="title">
		<%:"ViewTypes".Localize() %></h3>
	<%: Html.GridForModel() %>
</asp:Content>
