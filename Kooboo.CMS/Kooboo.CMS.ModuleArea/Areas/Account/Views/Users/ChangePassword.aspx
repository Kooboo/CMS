<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Account.Models.ChangePasswordModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= "ChangePassword".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<% using (Html.BeginForm())
	 { %>
		<fieldset>
			<table>
				<tbody>
					<%:Html.EditorFor(m=>m.OldPassword) %>
					<%:Html.EditorFor(m => m.NewPassword) %>
					<%:Html.EditorFor(m => m.NewPassword1)%>
				</tbody>
			</table>
		</fieldset>
		<p class="buttons">
			<button type="submit">
				<%:"Save".Localize() %></button>
		</p>
		<%} %>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
