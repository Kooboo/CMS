<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Account/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Account.Models.CreateUserModel>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<% using(Html.BeginForm()) { %>
		<fieldset>
			<table>
				<tbody>
					<%:Html.EditorFor(m => m.UserName) %>
					<%:Html.EditorFor(m => m.Email) %>
					<%:Html.EditorFor(m => m.Password) %>
					<%:Html.EditorFor(m => m.ConfirmPassword)%>
					<%:Html.EditorFor(m => m.IsAdministrator) %>
                    <%:Html.EditorFor(m=>m.UICulture)%>
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
