<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Content.Models.WorkflowHistory>>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"History".Localize()%></span>
    </h3>
    <div class="command">
    </div>
    <%:Html.GridForModel() %>
    <p class="buttons">
        <a href="javascript:;" class="button dialog-close">
            <%:"Close".Localize() %></a>
    </p>
    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
