<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.SiteMap>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%="Start - Sitemap".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%: "Start".Localize()%></h3>
    <div class="sitemap-panel clearfix">
        <%if (Model.Root == null)
          {%>
        <div class="home-page-create">
            <p>
                <%:"You haven't created any page yet, please click the button below to create a home page!".Localize()%></p>
            <div class="page-create-button">
                <span>
                    <%:"Create home page".Localize()%></span>
                <%:Html.Partial("LayoutList", ViewContext.RequestContext.AllRouteValues().Merge("IsDefault",true).Merge("ReturnUrl", Request.RawUrl))%>
            </div>
        </div>
        <%}
          else
          { %>
        <div>
            <div class="sitemap">
                <%: Html.Partial("SiteMapNode", Model.Root)%>
            </div>
        </div>
        <div class="sitemap-description">
            <ul class="clearfix">
                <li class="dynamic"><span></span>
                    <%:"Dynamic page".Localize()%></li>
                <li class="static"><span></span>
                    <%:"Static page".Localize()%></li>
                <li class="from-parent"><span></span>
                    <%:"Parent page".Localize()%></li>
                <li class="in-menu"><span>B</span><%:"In menu".Localize()%></li>
                <li class="unpublished"><span></span>
                    <%:"Unpublished".Localize()%></li>
            </ul>
        </div>
        <%} %>
    </div>
    <script type="text/javascript">
        $(function () {

            //Flowmap
            $('.sitemap').flowmap({
                typeset: 'vertical'
            }).css({
                overflow: 'auto'
            }).each(function () {
                $(this).height(Math.min(this.scrollHeight + 200, 450));
                $(this).width(this.scrollWidth);
            }).css({
                overflow: 'visible'
            });

            $("a.commandNode").click(function () {
                var handle = $(this);
                var confirm = handle.attr('confirm');
                var fun = function () {
                    var tempForm = kooboo.cms.ui.formHelper.tempForm([{ FullName: handle.attr('FullName')}], handle.attr('href'), 'model').form;

                    tempForm.ajaxSubmit({
                        success: function (response) {
                            if (response.Success) {
                                document.location.reload();
                            } else {
                                kooboo.cms.ui.messageBox().showResponse(response);
                            }
                        },
                        type: 'post'
                    });
                };
                if (confirm) {
                    kooboo.confirm(confirm, function (result) {
                        if (result) { fun(); }
                    });
                } else {
                    fun();
                }

                return false;
            });

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

            //Create home page
            $('.page-create-button').click(function () {
                $(this).toggleClass("active");
            });

            $('a.publish').click(function () {
                var handle = $(this),
                parentNode = handle.parents('div.fm-node');
                if (parentNode.hasClass('unpublished')) {
                    $.pop({ url: handle.attr('publishUrl'), title: '<%:"Click to publish" %>', frameHeight: '100%' });
                } else {
                    $.post(handle.attr('unPublishUrl'), function () {
                        parentNode.addClass('unpublished');
                        handle.html(handle.attr('publishText'));
                    });
                }
            });
        })
    </script>
</asp:Content>
