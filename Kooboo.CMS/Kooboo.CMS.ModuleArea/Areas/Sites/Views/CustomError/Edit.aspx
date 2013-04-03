<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
	Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.CustomError>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="common-form">
		<% Html.BeginForm(); %>
		<%:Html.ValidationSummary(true) %>
		<%--sitename for remote validation--%>
		<%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
		<%:Html.Hidden("old_Key", Model.StatusCode)%>
		<%-- <%:Html.Hidden("success",Request.HttpMethod.ToLower() == "post" && ViewData.ModelState.IsValid) %>--%>
		<fieldset>			
			<table>
				<tbody>
					<%:Html.EditorFor(m => m.StatusCode, new { @class = "medium" })%>
					<%:Html.EditorFor(m => m.RedirectUrl, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.RedirectType, new { @class = "medium" })%>
				</tbody>
			</table>
		</fieldset>
		<p class="buttons">
			<button type="submit">
				<%:"Save".Localize() %></button></p>
		<% Html.EndForm(); %>
	</div>
</asp:Content>
