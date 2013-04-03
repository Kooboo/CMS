<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Models.TextFolder>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= "Contents".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script language="javascript" type="text/javascript">
        //kooboo.cms.content.folder.initPop();
        function afterRemove() {
            document.location.reload();
        }
    </script>
    <h3 class="title">
        <%= "Contents".Localize()%></h3>
    <div class="command clearfix">
        <% if (Kooboo.CMS.Web.Authorizations.AuthorizationHelpers.Authorize(ViewContext.RequestContext, Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission))
           {%>
        <%: Html.ActionLink("Create Folder".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button dialog-link", title = "Create folder".Localize() }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button deleteFolder" }))%>
        <%: Html.Partial("Search") %>
        <%} %>
    </div>
    <%:Html.GridForModel() %>
    <script language="javascript" type="text/javascript">
        $("a.deleteFolder").click(function () {
            var handle = $(this);

            var selected = $("input:checkbox[name=Selected][checked]");
            if (selected.length == 0) {
                kooboo.alert(grid.messages.empty);
                return false;
            }
            var selectedStr = "";

            var selectedModel = [];

            var trs = [];

            selected.each(function () {
                var current = $(this);
                trs.push(current.parents('tr'));
                //var model = [];
                //model.push(current.val());
                selectedModel.push(current.val());
            });

            kooboo.confirm(grid.messages.confirm, function (result) {
                if (!result) {
                    return false;
                }
                var tempForm = kooboo.cms.ui.formHelper.tempForm(selectedModel, handle.attr("href"), "model", { method: "post" });

                kooboo.cms.ui.loading().show();

                $.post(handle.attr('href'), tempForm.form.serialize(), function (response) {
                    if (response.Success) {
                        if (window.afterRemove) {
                            afterRemove(response);
                        } else {
                            trs.each(function (value) {
                                value.fadeOut(function () {
                                    value.remove();
                                });

                            });
                        }
                        kooboo.cms.ui.messageBox().hide();
                    } else {
                        kooboo.cms.ui.messageBox().showResponse(response);
                        setTimeout(function () {
                            kooboo.cms.ui.messageBox().hide();
                        }, 5000);
                    }
                });

            });

            return false;
        });
    </script>
</asp:Content>
