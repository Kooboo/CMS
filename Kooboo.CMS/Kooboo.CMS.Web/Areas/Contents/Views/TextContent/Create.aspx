<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.TextContent>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Create".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%: Html.Partial(ViewData["Template"].ToString(), Model)%>
    <script type="text/javascript" language="javascript">
        $("input[type=file]:not(.no-style)").filestyle();
    </script>
</asp:Content>
