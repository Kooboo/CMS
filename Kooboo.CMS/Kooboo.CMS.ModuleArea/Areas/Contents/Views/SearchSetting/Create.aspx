<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Search.Models.SearchSetting>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true)%>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.EditorFor(m => m.FolderName, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.UrlFormat, new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.IncludeAllFields)%>
                    <%:Html.EditorFor(m => m.Fields, new { @class="fields" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize()%></button>
        </p>
        <% } %>
        <script language="javascript" type="text/javascript">
            $(function () {
                var toggle = function (handle) {
                    var checked = handle.attr('checked');
                    if (checked) {
                        handle.parents('tr:eq(0)').next().hide().find('input,select').attr('disabled', 'disabled');
                    } else {
                        handle.parents('tr:eq(0)').next().show().find('input,select').removeAttr('disabled');
                    }
                }
                var settingToggle = $('#IncludeAllFields').change(function () {
                    toggle($(this));
                });
                toggle(settingToggle);

                var folderNameCheckboxs = $(':checkbox[name=FolderName]').linkageSelect({
                    sub: '.fields',
                    value: 'Name',
                    text: 'Name',
                    url: '<%=Url.Action("GetFields",ViewContext.RequestContext.AllRouteValues()) %>',
                    dataname: 'folderPath'
                }).change(function () {
                    handleIncludeFields(folderNameCheckboxs);
                }); ;


                handleIncludeFields(folderNameCheckboxs);

                function handleIncludeFields(folderchecks) {
                    if (!folderchecks.filter('[checked]').val()) {
                        settingToggle.attr('disabled', 'disabled');
                    } else {
                        settingToggle.removeAttr('disabled');
                    }
                }
            });
        </script>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
