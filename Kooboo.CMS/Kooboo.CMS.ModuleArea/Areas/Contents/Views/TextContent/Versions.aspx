<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Content.Versioning.VersionInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Versions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="command">
        <%:Html.ActionLink("Diff", "Diff", ViewContext.RequestContext.AllRouteValues(),new RouteValueDictionary( new  {@class="button diff" }))%>
    </div>
    <%=Html.GridForModel() %>
    <script language="javascript" type="text/javascript">
        $(function () {

            var tip = '<%: "please select two versions!".Localize() %>';

            var title = "Diff";

            $("input:checkbox:eq(0)").hide();

            var diffButton = $("a.diff.button").click(function () {

                var current = $(this);

                var selected = $("input:checkbox[checked]");


                if (selected.length < 2) {

                    alert(tip);

                    return false;

                } else {

                    var href = current.attr("href");


                    var request = $.request(href);

                    request.queryString["v1"] = selected.first().val();

                    request.queryString["v2"] = selected.last().val();

                    //alert(request.getUrl());
                    $.pop({

                        url: request.getUrl(),

                        autoOpen: true,

                        width: 900,

                        height: 600,

                        frameHeight: "98%",

                        popupOnTop: true,

                        openOnclick: false,

                        onload: function (handle, pop, config) {
                            pop.iframe.contents().find('div.diffPane').height(500);
                        },

                        onclose: function (handle, pop, config) {

                            pop.destory();

                        },

                        title: title

                    });

                }
                return false;

            });
            $("input:checkbox").change(function () {

                var selectedCount = $("input:checkbox[checked]").length;

                if (selectedCount > 2) {

                    $(this).attr("checked", false);

                    return false;

                }

            });

            $('a.o-icon.revert').click(function (e) {
                e.preventDefault();
                var handle = $(this);
                kooboo.confirm('<%:"Are you sure you want to revert to this version?".Localize() %>', function (r) {
                    if (r) {
                        $.post(handle.attr('href'), function (response) {
                            if (response.Success) {
                                top.kooboo.data('parent-page-reload', true);
                            }
                            kooboo.cms.ui.messageBox().showResponse(response);
                        });
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
