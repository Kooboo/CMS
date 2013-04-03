<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.Schema>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Create content type".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Create content type".Localize() %>
    </h3>
    <%:Html.Partial("Nav") %>
    <div class="common-form">
        <% using (Html.BeginForm(ViewContext.RequestContext.GetRequestValue("action")
               , ViewContext.RequestContext.GetRequestValue("controller")
               , ViewContext.RequestContext.AllRouteValues()
               , FormMethod.Post, new RouteValueDictionary().Merge("id", "schemaForm")))
           { %>
        <%:Html.ValidationSummary(true) %>
        <%:Html.Hidden("RepositoryName", ViewContext.RequestContext.GetRequestValue("RepositoryName"))%>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.Name, new { @class="medium" })%>
                    <%:Html.EditorFor(m=>m.IsTreeStyle) %>
                </tbody>
            </table>
        </fieldset>
        <div class="command clearfix">
            <input type="button" class="button" value="Create field" id="btn_CreateColumn" title="Create" />
        </div>
        <div id="grid-field">
        </div>
        <p class="buttons">
            <%if (Kooboo.CMS.Content.Models.Repository.Current.EnableCustomTemplate)
              {%>
            <button type="submit" class="button">
                <%:"Next".Localize()%>
                »</button>
            <%} %>
            <button type="submit" id="finishBtn" class="button">
                <%:"Save Change".Localize()%></button>
            <%=Html.Hidden("Finish", false)%>
        </p>
        <% } %>
    </div>
    <%:Html.Partial("Schema.Script") %>
    <%:Html.Partial("ColumnForms",Model??new Kooboo.CMS.Content.Models.Schema()) %>
    <div class="hide">
        <%:Html.Partial("Column", new Kooboo.CMS.Content.Models.Column())%>
    </div>
</asp:Content>
