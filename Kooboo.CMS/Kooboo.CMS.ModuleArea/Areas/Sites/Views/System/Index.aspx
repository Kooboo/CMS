<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Site>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Settings".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var isOnline = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.IsOnline(Request["siteName"]); %>
    <h3 class="title">
        <%: "Settings".Localize()%></h3>
    <div class="common-form">
        <% Html.BeginForm(); %>
        <div id="setting-tab">
            <ul class="tabs clearfix">
                <li class="current"><a href="#basicInfo">
                    <%:"Basic".Localize()%></a></li>
                <li><a href="#smtpInfo">
                    <%:"SMTP".Localize()%></a></li>
            </ul>
            <div class="tab-content" id="basicInfo">
                <fieldset>
                    <table>
                        <tbody>
                            <%:Html.HiddenFor(m => m.Name)%>
                            <%:Html.DisplayFor(m => m.Name)%>
                            <%:Html.EditorFor(m => m.DisplayName, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Repository, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Culture, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Theme, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Domains, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.SitePath, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Version, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.ResourceDomain, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.Mode)%>
                            <%:Html.EditorFor(m => m.InlineEditing, new { @class = "medium" })%>
                            <%:Html.EditorFor(m => m.ShowSitemap)%>
                            <%:Html.EditorFor(m => m.EnableJquery)%>
                            <%:Html.EditorFor(m => m.EnableVersioning)%>
                            <%:Html.EditorFor(m => m.EnableStyleEdting)%>
                            <%:Html.EditorFor(m => m.CustomFields)%>
                        </tbody>
                    </table>
                </fieldset>
            </div>
            <div class="tab-content" id="smtpInfo">
                <%:Html.EditorFor(m => m.Smtp)%>
            </div>
        </div>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize()%></button>
            <%:Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues().Merge("controller", "site"), new RouteValueDictionary(new { @class = "button", id = "site-export" }))%>
            <%: Html.ActionLink("Delete this site".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues().Merge("controller", "site"), new RouteValueDictionary().Merge("id", "deleteSite").Merge("class", "button").Merge("title", "Confirm".Localize()))%>
            <%:Html.ActionLink(isOnline ? "Take offline".Localize() : "Take online".Localize(), "SwitchOffOn", "Site", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "switch-off-on button", OnLine = "Take online".Localize(), OffLine = "Take offline".Localize(), IsOnline = isOnline }))%>
            <%:Html.ActionLink("Clear cache".Localize(), "ClearCache", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button", id = "site-clear-cache" }))%>
        </p>
        <% Html.EndForm(); %>
    </div>
    <script type="text/javascript" language="javascript">
        window.onSuccess = function () {
            //prevent page reload after ajax post success..
        }
        $(function () {
            $('#setting-tab').koobooTab();
            $("#deleteSite").pop({
                width: 350,
                height: 190,
                onclose: function (handle, pop, config) {
                    pop.find('iframe').attr('src', handle.attr('href'));
                }
            });
            $("#site-export").click(function () {
                setTimeout(function () {
                    kooboo.cms.ui.loading().hide()
                }, 1000);
            });
            $('a.switch-off-on').click(function (e) {
                e.preventDefault();
                var handle = $(this);
                var href = handle.attr('href');
                var isOnline = handle.attr('isonline').toLower() == 'true';
                var displayer = $('#current-sitename');
                $.post(href, function (response) {
                    if (response.Success) {
                        if (isOnline) {
                            handle.html(handle.attr('online'));
                            displayer.html(displayer.attr('offline'));
                        } else {
                            handle.html(handle.attr('offline'));
                            displayer.html(displayer.attr('online'));
                        }
                        handle.attr('isonline', !isOnline);
                        kooboo.cms.ui.messageBox().show(response.Messages[0], 'info');
                    } else {
                        kooboo.cms.ui.messageBox().showResponse(response);
                    }
                });
            });
            $('#site-clear-cache').click(function (e) {
                e.preventDefault();
                var handle = $(this);
                var href = handle.attr('href');
                $.post(href, function (response) {
                    if (response.Success) {
                        kooboo.cms.ui.messageBox().show(response.Messages[0], 'info');
                    } else {
                        kooboo.cms.ui.messageBox().showResponse(response);
                    }
                });
            });
        });
    </script>
</asp:Content>
