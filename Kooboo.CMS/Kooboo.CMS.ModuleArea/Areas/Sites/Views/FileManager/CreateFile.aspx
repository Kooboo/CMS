<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.FileResource>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <%: Html.Hidden("directoryPath",ViewContext.RequestContext.GetRequestValue("directoryPath"))%>
        <%: Html.Hidden("SiteName", ViewContext.RequestContext.GetRequestValue("SiteName"))%>
        <fieldset>           
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name)%>
                    <%:Html.EditorFor(m=>m.FileExtension) %>
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
