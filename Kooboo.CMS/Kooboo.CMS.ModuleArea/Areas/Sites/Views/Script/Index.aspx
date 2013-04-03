<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<Kooboo.CMS.Sites.Models.ScriptFile>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Scripts".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Scripts".Localize() %></h3>
    <div class="command clearfix">
        <%: Html.ActionLink("Create".Localize(), "Create", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button dialog-link",title="Create script"}))%>
        <%: Html.ActionLink("Import".Localize(), "Import", ViewContext.RequestContext.AllRouteValues(),new RouteValueDictionary(new { @class = "button", name = "import" }))%>
        <%: Html.Partial("Import", Kooboo.CMS.Web.Areas.Sites.Models.ImportModel.Default)%>
        <%: Html.ActionLink("Export".Localize(), "Export", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button exportCommand" }))%>
        <%: Html.ActionLink("Delete".Localize(), "Delete", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class="button deleteCommand" }))%>
    </div>
    <%: Html.GridForModel() %>
    <%:Html.Hidden("SortUrl",this.Url.Action("Sort",ViewContext.RequestContext.AllRouteValues())) %>
    <script language="javascript" type="text/javascript">
        $(function () {
            $("div.table-container table").tableSorter({
                cancel: ".foldertr",
                beforeUp: function (tr) {
                    if (tr.prev() && tr.prev().hasClass("foldertr")) {
                        return false;
                    }
                },
                move: function () {
                    var url = $("#SortUrl").val();
                    var filesOrder = [];
                    $("div.table-container table tbody").find("tr").each(function () {
                        var current = $(this);
                        filesOrder.push(current.find("input:checkbox").val());
                    });

                    var tempForm = kooboo.cms.ui.formHelper.tempForm(filesOrder, url, "filesOrder", { method: "post" });

                    $.post(url, tempForm.form.serialize());
                },
                showUp: function (tr) {
                    if (tr.prev().length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                },
                showDown: function (tr) {
                    if (tr.next().length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            });
        });
    </script>
</asp:Content>
