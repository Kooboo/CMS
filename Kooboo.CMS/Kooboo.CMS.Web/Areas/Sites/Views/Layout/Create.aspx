<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Layout>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Create layout".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Create layout".Localize() %><%:Html.ActionLink("Back to list".Localize(),"Index",ViewContext.RequestContext.AllRouteValues()) %></h3>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <%:Html.EditorFor(m=>m.Name) %>
                </tbody>
            </table>
            <table>
                <tbody>
                    <%:Html.Partial("TemplateEditor", Model)%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit" name="pageRedirect" value="false">
                <%:"Save".Localize() %></button>
            <button type="submit" name="pageRedirect" value="true">
                <%:"Save & return".Localize()%></button>
        </p>
        <% } %>
    </div>
</asp:Content>
