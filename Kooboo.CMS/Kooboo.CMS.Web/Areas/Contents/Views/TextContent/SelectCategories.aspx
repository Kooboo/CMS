<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Contents.Models.SelectableViewModel>" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial(ViewData["Template"].ToString(), Model)%>
    <p class="buttons">
        <button class="form-button save">
            <%:"OK".Localize() %></button>
        <button class="form-button cancel">
            <%:"Cancel".Localize() %></button>
    </p>
</asp:Content>
