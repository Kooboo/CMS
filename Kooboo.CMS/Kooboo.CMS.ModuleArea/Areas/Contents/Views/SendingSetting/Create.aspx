<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.SendingSetting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Index".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <table>
            <tbody>
                <%:Html.EditorFor(m => m.FolderName, new { @class="long" })%>
                <%:Html.EditorFor(m=>m.SendReceived) %>
                <%-- <%:Html.EditorFor(m => m.AcceptAction, new { @class = "" })%>
                <%:Html.EditorFor(m => m.Published, new { @class = "" })%>--%>
            </tbody>
        </table>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            var selected = <%=ViewData["SelectedJSON"]??"[]" %> ;
            selected.each(function (val, index) {
                $(':checkbox[value="' + val.FolderName + '"]').attr('checked', 'checked');
            });

            var randomCls = 'random-' + Math.random().toString().replace('.','-');

            kooboo.cms.ui.event.ajaxSubmit(function(){
                var form = this;
                form.find('.'+randomCls).remove();
                $(':checkbox[name=FolderName][checked]').each(function(index){
                    $('<input type="hidden" />').val($(this).val()).attr({
                        name:'SendingList['+index+'].FolderName'
                    }).addClass(randomCls).appendTo(form);
                });
            });

        });

        

    </script>
</asp:Content>
