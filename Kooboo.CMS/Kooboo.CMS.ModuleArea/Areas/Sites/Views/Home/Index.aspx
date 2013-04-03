<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.Web.Mvc.Paging.PagedList<Kooboo.CMS.Sites.Models.SiteTree>>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Start".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        Site cluster</h3>
    <% if (ServiceFactory.UserManager.AllowCreatingSite(User.Identity.Name))
       {%>
    <div class="command clearfix">
        <a class="button dialog-link" href="<%=Url.Action("Create","Site") %>" title="<%:"Create a new site".Localize() %>">
            <%:"Create a new site".Localize()%></a> <a title="<%:"Create a sub site".Localize() %>"
                class="button dialog-link" href="<%=Url.Action("Create","Site", new { createType = "CreateSubSite"}) %>">
                <%:"Create a subsite".Localize()%></a> <a class="button dialog-link" href="<%=Url.Action("ImportSite","Site") %>"
                    title="<%:"Import site".Localize() %>">
                    <%:"Import site".Localize()%></a>
        <%:Html.Partial("Search")%>
    </div>
    <%} %>
    <div class="main-block clearfix">
        <%foreach (var siteTree in Model)
          {%>
        <div class="sitetrees">
            <%: Html.Partial("SiteNode", siteTree.Root)%>
        </div>
        <%} %>
        <div class="pagination right">
            <%: Html.Pager(Model, "", ViewContext.RequestContext.AllRouteValues(),null)%>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $(".site-export").click(function () {
                setTimeout(function () {
                    kooboo.cms.ui.loading().hide()
                }, 1000);
            });

            //Flowmap
            $('.sitetrees').flowmap({
                typeset: 'vertical'
            }).css({
                overflow: 'auto'
            }).each(function () {
                $(this).height(Math.min(this.scrollHeight + 60, 450));
                $(this).width(this.scrollWidth);
            }).css({
                overflow: 'visible'
            });

            $("a.deleteNode").pop({
                width: 350,
                height: 190,
                frameConfig: {
                    scrolling: "no",
                    frameborder: "no"
                }
            });

            //            $("a.deleteNode").click(function () {
            //                var handle = $(this);



            //                kooboo.confirm('<%:"Are you sure you want to delete this site?".Localize() %>', function (result) {

            //                    if (result) {
            //                        var tempForm = kooboo.cms.ui.formHelper.tempForm([], handle.attr('href'), 'model').form;

            //                        tempForm.ajaxSubmit({
            //                            success: function (response) {
            //                                if (response.Success) {
            //                                    document.location.reload();
            //                                } else {
            //                                    kooboo.cms.ui.messageBox().showResponse(response);
            //                                }
            //                            },
            //                            type: 'post'
            //                        });
            //                    }
            //                });

            //                return false;
            //            });

            //Map Item
            $('.map-item a').click(function () {
                $(this).toggleClass("active");
                $(this).siblings('ul').slideToggle("fast");
            }).blur(function () {
                var handle = this;
                setTimeout(function () {
                    $(handle).removeClass("active");
                    $(handle).siblings('ul').slideUp("fast");
                }, 200);
            });

            $('#import-site-btn').pop({
                useContent: true,
                contentId: 'import-site'
            });

            $('a.switch-off-on').click(function (e) {
                e.preventDefault();
                var handle = $(this),
                href = handle.attr('href'),
                node = handle.parents('.fm-node'),
                isOnline = handle.attr('isonline').toLower() == 'true',
                link = node.find('h4 a'),
                offlineClass = 'unpublished';
                $.post(href, function (response) {
                    if (response.Success) {
                        if (isOnline) {
                            node.addClass(offlineClass);
                            handle.html(handle.attr('online'));
                            link.html(node.attr('offline'));
                        } else {
                            node.removeClass(offlineClass);
                            handle.html(handle.attr('offline'));
                            link.html(node.attr('online'));
                        }
                        handle.attr('isonline', !isOnline);
                    } else {
                        kooboo.cms.ui.messageBox().showResponse(response);
                    }
                });
                return false;
            });
        });
    </script>
    <%:Html.Partial("ImportSite",new Kooboo.CMS.Web.Areas.Sites.Models.ImportSiteModel()) %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
