<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<string>>" %>
<%@ Import Namespace="Kooboo.CMS.Web.Areas.Contents.Models" %>
<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<% var guid = Guid.NewGuid();
   ViewData["Guid"] = guid;
   ViewData["InputName"] = "SendingFolder";
%>
<div id="tree-<%:guid %>" class="select-tree clear">
    <ul id="repository-folder-<%:guid %>" class="filetree treeview">
        <%foreach (var m in Model.Where(o => o != Repository.Current.Name))
          {
              if (string.IsNullOrEmpty(m))
              {
                  continue;
              }
              var sourceRepository = new Kooboo.CMS.Content.Models.Repository(m);
              sourceRepository = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(sourceRepository);

              var settingFolders = Kooboo.CMS.Content.Services.ServiceFactory.SendingSettingManager.All(sourceRepository, "");

              var items = settingFolders.Select(it => Kooboo.CMS.Content.Services.ServiceFactory.SendingSettingManager.Get(it.Repository, it.Name)).Select(o => new Kooboo.CMS.Content.Services.FolderTreeNode<TextFolder>()
              {
                  Folder = new TextFolder(sourceRepository, o.FolderName)
              });
          
        %>
        <li class="level-0" sendingrepository="<%:m %>"><span><a href="javascript:;">
            <%:m %></a>
            <input type="hidden" disabled="disabled" name="SendingRepository" value="<%:m %>" />
        </span>
            <%:Html.Partial("FolderTreeNode", items, ViewData)%>
        </li>
        <% } %>
    </ul>
</div>
<script language="javascript" type="text/javascript">
    $(function () {
        var treeContainer = $('#tree-<%:guid %>');
        $('#repository-folder-<%:guid %> ul').removeClass('filetree treeview ');
        $('#repository-folder-<%:guid %>').children().each(function () {
            var li = $(this);
            if (li.find('ul').length == 0) {
                li.remove();
            }
        });
        treeContainer.find(':checkbox.folder-checkbox').change(function () {
            treeContainer.find(':checkbox.folder-checkbox[checked]').attr('checked', false);
            var handle = $(this).attr('checked', true);
            $('input:hidden[name=SendingRepository]').attr('disabled', 'disabled');
            handle.parents('li.level-0').find('input:hidden[name=SendingRepository]').removeAttr('disabled');
        });
        var tree = $("#repository-folder-<%:guid %>").treeview();
        tree.find('span').click(function () {
            $(this).prev().click();
        });
        kooboo.cms.ui.event.ajaxSubmit(function () {
            var folderName = treeContainer.find(':checkbox.folder-checkbox[checked]').val();
            if (!folderName) {
                kooboo.cms.ui.messageBox().show('<%:"Sending folder is required.".Localize() %>', 'error');
            }
            return !!folderName;
        });
    });
</script>
