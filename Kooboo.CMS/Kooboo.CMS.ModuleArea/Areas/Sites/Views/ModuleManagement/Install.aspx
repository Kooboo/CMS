<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.InstallModuleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Install module".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3 class="title">
            <%: "Upload".Localize()%></h3>
        <ul class="step clearfix">
            <li class="current">
                <%:"Upload".Localize() %></li>
            <li>&gt;</li>            
            <li>
                <%: "Finish".Localize()%></li>
        </ul>
        <div class="step-content">
            <div class="common-form">
                <%using (Html.BeginForm())
                  {%>
                <fieldset>
                    <table>
                        <tbody>
                            <%:Html.EditorFor(m => m.ModuleFile, new { @class = "medium" })%>
                        </tbody>
                    </table>
                </fieldset>
                <p class="buttons">
                    <button type="submit" class="button step-link">
                        <%:"Next".Localize()%>
                        »</button>
                </p>
                <%} %>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
