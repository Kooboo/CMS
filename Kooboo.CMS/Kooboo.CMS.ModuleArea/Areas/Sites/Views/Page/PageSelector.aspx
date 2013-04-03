<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%var pageTree = Kooboo.CMS.Web.Areas.Sites.Models.PageDropDownListTreeDataSource.Instance().GetSelectListItems(ViewContext.RequestContext); %>
    <div class="select-tree" id="page-tree-container">
        <%:Html.Partial("PageTree", pageTree)%>
    </div>
    <br />
    <br />
    <p class="buttons">
        <button id='page-select-add'>
            <%:"Add".Localize()%></button>
    </p>
    <script language="javascript" type="text/javascript">
        $(function () {
            var tree = $('#page-tree-container ul[nodetype=tree-root]').treeview({
                collapsed: true
            });

            tree.find('a').each(function () {
                var handle = $(this);
                if (handle.parent().is('.expandable')) {
                    handle.click(function () {
                        handle.parent().find('div.hitarea').click();
                    });
                }
            });

            $('input:checkbox.selected-page').change(function () {
                var handle = $(this);
                $('input:checkbox.selected-page').attr('checked', false);
                handle.attr('checked', true);
            });

            $('#page-select-add').click(function () {
                var selected = $('input:checkbox.selected-page:checked');
                if (selected.length) {
                    top.kooboo.data('tinymce.onPageSelect')(selected.val(), selected.attr('text'));
                    $.popContext.getCurrent().close();
                } else {
                    kooboo.cms.ui.messageBox().show('<%:"Please select a page." %>', 'warning');
                }
            });

            setTimeout(function () {
                var selectedPage = top.kooboo.data('tinymce.pageselector.selected');

                var selectedCheckbox = tree.find('input:checkbox[value="' + selectedPage + '"]').attr('checked', true);


                if (selectedCheckbox.length) {
                    selectedCheckbox.parents('li').each(function () {
                        $(this).find('div.hitarea').click();
                    });
                }
            }, 200);


        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
