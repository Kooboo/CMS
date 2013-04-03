<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.Page>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=   "Pages".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "Pages".Localize()%></h3>
    <div class="command clearfix">
        <%
            ViewContext.RequestContext.AllRouteValues()["prevController"] = ViewContext.RequestContext.AllRouteValues()["controller"];
            if (new Kooboo.CMS.Web.Areas.Sites.Models.InheritableGridActionVisibleArbiter().IsVisible(ViewData["page"], ViewContext))
            {%>
        <div class="dropdown-button">
            <span>
                <%="Create New".Localize()%></span>
            <div class="hide">
                <%:Html.Partial("LayoutList",ViewData["LayoutList"]) %>
            </div>
        </div>
        <%: Html.ActionLink("Import".Localize(), "Import", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class = "button", name = "import" })%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <% if (!string.IsNullOrEmpty(ViewContext.RequestContext.GetRequestValue("fullName")))
           {  %>
        <%
               var site = Kooboo.CMS.Sites.Models.Site.Current;
               var page = new Kooboo.CMS.Sites.Models.Page(site, ViewContext.RequestContext.GetRequestValue("fullName"));
               var isStaticPage = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.IsStaticPage(site, page);
               var previewUrl = FrontUrlHelper.Preview(Url, site, page, null);
        %>
        <%if (isStaticPage)
          {  %>
        <a href="<%:previewUrl %>" target="_blank" class="button">
            <%:"Preview".Localize() %></a>
        <%} %>
        <%: Html.ActionLink("Edit".Localize(), "Edit", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = ViewContext.RequestContext.GetRequestValue("fullName"), fromName = Request["fullName"], prevController = "page" }, new { @class = "button" })%>
        <% if (ServiceFactory.PageManager.HasDraft(ViewContext.RequestContext.GetRequestValue("fullName")))
           { %>
        <%: Html.ActionLink("Draft".Localize(), "Draft", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = ViewContext.RequestContext.GetRequestValue("fullName"), fromName = Request["fullName"], prevController = "page" }, new { @class = "button" })%>
        <%  }
           }
            }%>
        <%if (ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, ViewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission))
          {%>
        <div class="dropdown-button">
            <span>
                <%="Batch actions".Localize()%></span>
            <div class="hide">
                <ul>
                    <li>
                        <%: Html.ActionLink("Delete".Localize(), "Delete", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName") }, new { @class = "deleteCommand" })%></li>
                    <li>
                        <%: Html.ActionLink("Publish".Localize(), "BatchPublish", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName") }, new { @class = "commandWithoutStyle" })%></li>
                    <li>
                        <%: Html.ActionLink("Unpublish".Localize(), "BatchUnpublish", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName") }, new { @class = "commandWithoutStyle" })%></li>
                    <li>
                        <%: Html.ActionLink("Export".Localize(), "Export", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, new { @class="exportCommand" })%></li>
                </ul>
            </div>
        </div>
        <%}%>
        <%: Html.Partial("Search") %>
    </div>
    <div class="scrollable-table">
        <%: Html.GridForModel() %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('a.action-localize').click(function () {
                var handle = $(this);
                kooboo.confirm('<%:"Are you sure you want to localize this item" %>', function (r) {
                    if (r) {
                        document.location.href = handle.attr('href');
                    }

                });
                return false;
            });
            $(document).click(function () {
                $('div.dropdown-button').children('div').addClass('hide');
            });
            var dropdown = $('div.dropdown-button').click(function (e) {
                e.stopPropagation();
                var menu = $(this).children('div');
                if (menu.hasClass('hide')) {
                    menu.removeClass('hide');
                } else {
                    menu.addClass('hide');
                }
            }).children().click(function () {
                $('div.dropdown-button').children('div').addClass('hide');
            });

            var fixHelper = function (e, ui) {
                ui.children().each(function () {
                    $(this).width($(this).width());
                });
                return ui;
            };
            $("div.table-container table tbody tr").css("cursor", "move");
            $("div.table-container table tbody").sortable({
                helper: fixHelper,
                update: function (event, ui) {
                    var pageNameOrders = $('a[name="PageName"]').toArray().select(function (el) { return $(el).text(); })
                            .select(function (text, index) { return 'pageNames[' + index + ']=' + text }).join('&');
                    $.ajax({
                        url: '<%= this.Url.Action("SortPages", ViewContext.RequestContext.AllRouteValues())%>',
                        data: pageNameOrders,
                        type: 'post',
                        beforeSend: function () {
                            kooboo.cms.ui.loading().show();
                        },
                        success: function (response) {
                            if (response.Success) {
                            } else {
                                kooboo.cms.ui.messageBox().showResponse(response);
                            }
                            kooboo.cms.ui.loading().hide();
                        },
                        error: function () {
                        }
                    });
                }
            }).disableSelection();
        });
        function afterRemove() {
            window.location.reload();
        }

    </script>
</asp:Content>
