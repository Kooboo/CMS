<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.ScriptFile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:"Create".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<% using (Html.BeginForm())
	 { %>
		<%--sitename for remote validation--%>
		<%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
		<%:Html.ValidationSummary(true) %>
		<fieldset>			
			<table>
				<tbody>
					<%:Html.EditorFor(m=>m.Name) %>
					<%:Html.Hidden("FileExtension",".js")%>
				</tbody>
			</table>
            <table>
				<tbody>
					<%:Html.EditorFor(m=>m.Body) %>
				</tbody>
			</table>
		</fieldset>
		<p class="buttons">
			<button type="submit">
				<%:"Save".Localize() %></button>
		</p>
		<% } %>
	</div>
</asp:Content>
