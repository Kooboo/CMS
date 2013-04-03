<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<%@ Import Namespace="Kooboo.CMS.Content.Services" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Web.Areas.Contents.Models.FolderTreeModel>" %>
<%
    ViewData["InputName"] = Model.InputName;
    var guid = Guid.NewGuid();
    var values = Kooboo.Web.Script.Serialization.JsonHelper.ToJSON(Model.Values ?? new List<string>());
%>
<script language="javascript" type="text/javascript">

    $(function () {
        var isSingle = '<%:Model.IsSingle %>'.toLowerCase() == 'true',
        allowNull = '<%:Model.AllowNull %>'.toLowerCase() == 'true',
        inputName = '<%:Model.InputName %>',
        treeContainer = $('#tree-<%:guid %>');

        var tree = treeContainer.children('ul').treeview();
        tree.find('div').unbind('click').click(function () {
            $(this).siblings('span').click();
        });

        if (isSingle) {
            treeContainer.find(':checkbox.folder-checkbox').change(function () {
                treeContainer.find(':checkbox.folder-checkbox[checked]').attr('checked', false);
                $(this).attr('checked', true);
            });
        }
        if (!allowNull) {
            kooboo.cms.ui.event.ajaxSubmit(function () {
                var val = treeContainer.find('input:checkbox[name="' + inputName + '"][checked]').val();

                if (!val) {
                    setTimeout(function () {
                        kooboo.cms.ui.messageBox().show('<%:"Folder name is required.".Localize() %>', 'error');
                    }, 100);

                }
                return !!val;

            });
        }
    });

</script>
<% ViewData["InputName"] = Model.InputName;
   ViewData["IsSingle"] = Model.IsSingle;
   ViewData["Values"] = Model.Values; %>
<div id="tree-<%:guid %>" class="select-tree clear">
    <%: Html.Partial("FolderTreeNode",Model.DataSource,ViewData) %>
</div>
