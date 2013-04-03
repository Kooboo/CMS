<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.ScriptFile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:"Edit".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<% using (Html.BeginForm())
	 { %>
		<%--sitename for remote validation--%>
		<%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
		<%:Html.Hidden("old_Key",Model.FileName) %>
		<%:Html.ValidationSummary(true) %>
		<fieldset>			
			<table>
				<tbody>
					<%:Html.HiddenFor(m=>m.FileExtension) %>
					<%:Html.HiddenFor(m => m.FileName)%>
					<%:Html.DisplayFor(m => m.FileName)%>
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
