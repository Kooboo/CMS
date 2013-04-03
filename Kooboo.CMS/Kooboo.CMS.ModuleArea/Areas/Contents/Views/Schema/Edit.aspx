<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.Schema>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Edit content type".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Edit content type".Localize() %></h3>
    <%:Html.Partial("Nav") %>
    <div class="common-form">
        <% using (Html.BeginForm(ViewContext.RequestContext.GetRequestValue("action")
               , ViewContext.RequestContext.GetRequestValue("controller")
               , ViewContext.RequestContext.AllRouteValues()
               , FormMethod.Post, new RouteValueDictionary().Merge("id", "schemaForm")))
           {%>
        <%:Html.ValidationSummary(true)%>
        <input type="hidden" name="Name" value="<%:Model.Name %>" />
        <%:Html.HiddenFor(m => m.TemplateBuildByMachine)%>
        <%:Html.Hidden("old_Key", Model.Name)%>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m => m.Name, new { @class = "width200" })%>
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
            <button type="submit">
                <%:"Next".Localize()%>
                »</button>
            <% } %>
            <button type="submit" id="finishBtn" tip="<%="Some Templates have been manually Edited !<p>Are you sure you want to continue to reset all template?</p>".Localize() %>">
                <%:"Save Change".Localize()%></button>
            <%=Html.Hidden("Finish", false)%>
            <%=Html.Hidden("Next", true)%>
            <%=Html.Hidden("ResetAll", true)%>
        </p>
        <%}%>
        <%:Html.Partial("ColumnForms",Model) %>
        <div class="hide">
            <%:Html.Partial("Column", new Kooboo.CMS.Content.Models.Column())%>
        </div>
    </div>
    <%:Html.Partial("Schema.Script") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
