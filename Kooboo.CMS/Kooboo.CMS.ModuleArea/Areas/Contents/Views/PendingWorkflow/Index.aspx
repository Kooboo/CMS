<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Content.Models.PendingWorkflowItem>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Pending workflow items".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <span>
            <%:"Pending workflow items".Localize() %></span>
    </h3>
    <div class="command">        
    </div>
    <%:Html.GridForModel() %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
