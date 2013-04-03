<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Site>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% 
            using (Html.BeginForm())
            { %>
        <%:Html.Hidden("SiteName", Request["SiteName"])%>
        <fieldset>
            <p class="clearfix">
                <span class="ui-icon ui-icon-alert" style="float: left; margin-right: 8px;"></span>
                <%="This site will be permanently deleted and cannot be recovered. Are you sure?".Localize()%>
            </p>
            <p class="clearfix" style="padding-left: 25px;">
                <% if (!ServiceFactory.SiteManager.UseSharedDB(Model))
                   {  %>
                <input type="checkbox" id="DeleteRepository" value="True" name="DeleteRepository" /><label
                    class="checkbox-label" for="DeleteRepository">
                    <%="Delete the associated content database.".Localize()%>.
                </label>
                <%} %>
                <input type="hidden" value="False" name="DeleteRepository" />
            </p>
        </fieldset>
        <p class="buttons">
            <a class="dialog-close button" href="javascript:;" style="float: right; margin: 0 0 0 10px;">
                <%:"Cancel".Localize() %>
            </a>
            <button type="submit" style="float: right; margin: 0;">
                <%:"Delete".Localize()%></button>
        </p>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
