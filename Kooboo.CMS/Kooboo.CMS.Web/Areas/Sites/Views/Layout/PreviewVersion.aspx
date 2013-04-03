<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Layout>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Preview layout version".Localize()%></h3>
    <%: Html.DisplayFor(m => m.Body) %>
    <% using (Html.BeginForm(ViewContext.RequestContext.AllRouteValues().Merge("Action", "Revert")))
       { %>
       <br />
    <p class="buttons">
        <a href='<%=Url.Action("Revert",ViewContext.RequestContext.AllRouteValues()) %>'
            class="button ajax-post-link" confirm="<%:"Are you sure you want to rever to this version?".Localize()%>">
            <%:"Revert".Localize() %></a> <a href="javascript:;" class="dialog-close button">
                <%:"Close".Localize()%></a>
    </p>
    <% } %>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('.dialog-close').click(function () {
                kooboo.cms.ui.status().pass();
            });
        });
    </script>
</asp:Content>
